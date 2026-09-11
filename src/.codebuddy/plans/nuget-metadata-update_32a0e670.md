---
name: nuget-metadata-update
overview: 扫描 Darwinism/src 下全部 vbproj，对其中的 20 个 dll 类库项目统一刷新 NuGet 包元数据（title/tags/description/release notes 等描述性内容根据各项目代码生成），并统一设置 PackageIcon 相对路径、PackageProjectUrl、RepositoryUrl、GPL-3.0-only 授权、Copyright、Authors、Company、Product，同时为缺失打包配置的项目启用 GeneratePackageOnBuild 与图标打包。
todos:
  - id: prep-metadata
    content: 汇总全局元数据模板与相对路径表，核验20个项目当前包属性与冲突字段
    status: completed
  - id: gen-desc
    content: 用 [subagent:code-explorer] 读取目标项目 .vb 源码，产出各项目 title/tags/description/releaseNotes 草稿
    status: completed
    dependencies:
      - prep-metadata
  - id: update-group1
    content: 更新 networking/Darwinism/CloudKit/IPCDriver/Parallel/DataMining 共8个类库元数据与图标打包
    status: completed
    dependencies:
      - prep-metadata
      - gen-desc
  - id: update-group2
    content: 更新 message/data 系列10个类库（OpenApi3.0、Rpc、XDRStream、BucketDb、CDF.PInvoke、FullTextBuffer、ClusterShared、GQL、LINQ、RQL）元数据
    status: completed
    dependencies:
      - prep-metadata
      - gen-desc
  - id: update-legacy
    content: 更新旧式 net48 项目 NetCDF、Sqlite3 元数据并处理打包兼容
    status: completed
    dependencies:
      - prep-metadata
      - gen-desc
  - id: verify-cleanup
    content: 清理冲突字段，校验全部 PackageIcon 相对路径与 XML 合法性，可选 dotnet pack 验证
    status: completed
    dependencies:
      - update-group1
      - update-group2
      - update-legacy
---

## 用户需求概述

扫描 `g:/GCModeller/src/runtime/Darwinism/src` 下全部 `*.vbproj`，识别其中的 **dll 类库** 项目，针对每个类库项目：

1. 读取其 `.vb` 源码内容，总结出 NuGet 包的 **Title、PackageTags、Description、PackageReleaseNotes** 等描述性文本并写入项目文件；
2. 统一写入固定元数据字段（见下）。

## 统一写入的固定字段

- PackageIcon = 项目目录到 `G:\GCModeller\src\runtime\Darwinism\.pkg\icons8-repository-96.png` 的**相对源路径**
- PackageProjectUrl = `https://scibasic.net/`
- RepositoryUrl = `https://github.com/xieguigang/Darwinism.git`
- RepositoryType = `git`
- PackageLicenseExpression = `GPL-3.0-only`
- Copyright = `Copyright © scibasic.net foundation, GuiLin China, 2026`
- Authors = `xieguigang<xie.guigang@gmail.com>`
- Company = `scibasic.net foundation`
- Product = `Darwinism HPC`
- GeneratePackageOnBuild = `true`（缺失者补齐）

## 核心范围（20 个目标 dll 类库）

networking、Darwinism、CloudKit/Centos、CloudKit/Docker、CloudKit/ossutil、computing/IPCDriver、computing/Parallel、DataScience/DataMining、message/OpenApi3.0、message/Rpc、message/XDRStream、data/BucketDb、data/CDF.PInvoke、data/FullTextBuffer、computing/DistributedCluster/ClusterShared、data/LINQ/GQL、data/LINQ/LINQ、data/LINQ/RQL、data/LINQ/Drivers/NetCDF、data/LINQ/Drivers/Sqlite3。

## 明确排除项

- `message/Google.Protobuf`（第三方 protobuf 移植，保留原版权）；`computing/DistributedCluster/Demo`（示例宿主）。
- 所有 `OutputType=Exe` 及命名含 `test` 的项目（batch、HeadNode、Node、Worker、Generator、Spy 及各 test 工程）一律不改动。

## 技术栈与现状

- 工程体系：多数目标为 **.NET SDK 风格** vbproj（`<Project Sdk="Microsoft.NET.Sdk">`，无 `OutputType` 即默认为 Library）；`NetCDF.vbproj`、`Sqlite3.vbproj` 为**旧式 csproj 风格**（含 `My Project\AssemblyInfo.vb`、`TargetFrameworkVersion v4.8`、无 SDK 包体系）。
- 现有包写法差异：`networking` 仅有部分字段；`Centos/Docker/ossutil/Parallel` 使用 `PackageLicenseFile=LICENSE` + 打包 `..\..\..\LICENSE` 的 `<None>` 项，且 Copyright 为 `i@xieguigang.me`；`Parallel` 已有 Title/Tags/Authors/Version；`BucketDb/LINQ/GQL/RQL` 已有 `PackageIcon` 文件名 + 图标 `<None>` 打包项；其余类库几乎无包元数据。

## 实施策略

1. **新增/集中公共元数据**：在每个目标项目**无条件**的 `<PropertyGroup>` 中写入上述 8 个固定字段 + `GeneratePackageOnBuild=true` + `PackageIcon` 相对路径；保留原有的条件编译 PropertyGroup（`Configurations/Platforms/TargetFrameworks` 等）不变，避免破坏多目标构建。
2. **PackageIcon 处理（按用户确认）**：将 `PackageIcon` 设为项目到 png 的相对源路径；同时把图标 `<None Include="...">` 项（新增或已有）改为同一相对路径，`Pack=true`、`PackagePath="\"`，使图标正确打包到包根目录。
3. **License 处理（按用户确认）**：统一改用 `PackageLicenseExpression=GPL-3.0-only`；**删除** `Centos/Docker/ossutil/Parallel` 中的 `<PackageLicenseFile>LICENSE</PackageLicenseFile>` 及其 `..\..\..\LICENSE` 的 `<None Pack="true" PackagePath="">` 项（与表达式互斥）。
4. **描述性内容生成**：读取各项目 `.vb` 源码（命名空间、模块/类 `'''` Summary、`AssemblyTitle`、公开 API），模板化产出 Title/Tags/Description/ReleaseNotes；优先复用既有 `AssemblyTitle`/注释（如 BucketDb 的 “In-Memory Key-Value BucketDb”、GQL 的 “Graph Query Language Interpreter”、RQL 的 “The Resource Query Language”、Centos 远程 Linux 助手等）。

## 相对路径表（png 基准 = `G:\GCModeller\src\runtime\Darwinism\.pkg\`）

| 目录层级 | PackageIcon 相对路径 |
| --- | --- |
| src 下一级（networking、Darwinism） | `..\..\.pkg\icons8-repository-96.png` |
| src 下两级（CloudKit\Centos、computing\IPCDriver、computing\Parallel、DataScience\DataMining、message\*、data\BucketDb、data\CDF.PInvoke、data\FullTextBuffer） | `..\..\..\.pkg\icons8-repository-96.png` |
| src 下三级（computing\DistributedCluster\ClusterShared、data\LINQ\GQL | LINQ | RQL） | `..\..\..\..\.pkg\icons8-repository-96.png` |
| src 下四级（data\LINQ\Drivers\NetCDF、data\LINQ\Drivers\Sqlite3） | `..\..\..\..\..\.pkg\icons8-repository-96.png` |


## 各项目建议 Title / Tags（Description 由代码注释扩充）

| # | 项目 | Title | Tags |
| --- | --- | --- | --- |
| 1 | networking | Darwinism IPC Networking Library | ipc;networking;hpc;darwinism |
| 2 | Darwinism | Darwinism HPC Core | hpc;darwinism;parallel;distributed |
| 3 | CloudKit/Centos | CentOS Remote Connection Helper | linux;ssh;centos;putty;hpc |
| 4 | CloudKit/Docker | Docker Container Management Helper | docker;container;hpc;cloud |
| 5 | CloudKit/ossutil | Cloud Object Storage (OSS) Helper | oss;cloud-storage;aliyun;hpc |
| 6 | computing/IPCDriver | Darwinism IPC Driver | ipc;driver;hpc |
| 7 | computing/Parallel | Darwinism HPC Parallel Compute Library | parallel;ipc;hpc;unix |
| 8 | DataScience/DataMining | Darwinism DataScience DataMining | datamining;machinelearning;hpc |
| 9 | message/OpenApi3.0 | OpenApi3.0 Source Generator | openapi;codegen;api;hpc |
| 10 | message/Rpc | Darwinism IPC RPC Framework | rpc;ipc;hpc |
| 11 | message/XDRStream | XDR Stream IO Library | xdr;serialization;io;hpc |
| 12 | data/BucketDb | In-Memory Key-Value BucketDb | kv;database;inmemory;repository |
| 13 | data/CDF.PInvoke | NetCDF PInvoke Bindings | netcdf;pinvoke;hdf;hpc |
| 14 | data/FullTextBuffer | Full-Text Index Buffer | fulltext;index;search;hpc |
| 15 | ClusterShared | Distributed Cluster Shared Types | cluster;distributed;shared;hpc |
| 16 | data/LINQ/GQL | Graph Query Language Interpreter | gql;graph;query;hpc |
| 17 | data/LINQ/LINQ | Darwinism LINQ Data Query Engine | linq;query;data;hpc |
| 18 | data/LINQ/RQL | The Resource Query Language | rql;resource;query;hpc |
| 19 | NetCDF | NetCDF Data Driver | netcdf;driver;hdf;hpc |
| 20 | Sqlite3 | SQLite3 Data Driver | sqlite;driver;database;hpc |


ReleaseNotes 默认：`Initial GPL-3.0-only release of the Darwinism HPC component.`（可按代码变更微调）

## 关键执行注意

- **旧式 csproj（NetCDF/Sqlite3）**：在现有 `<PropertyGroup>` 内追加 SDK 包属性；若其无法走 `dotnet pack`，至少补充元数据字段，并在备注中说明兼容性。
- **冲突字段清理**：覆盖旧 `Copyright=i@xieguigang.me`、旧 `RepositoryUrl`（如 `…/Darwinism`、`http://scibasic.net`）、旧 `PackageIcon` 文件名、旧 `Authors`，统一为上文固定值。
- **不做无关重构**：仅新增/修改公共元数据与图标打包项；不动 Exe/Test/排除项目；不改条件编译与 `ProjectReference`。
- **校验**：改完对每个目标项目执行 `dotnet pack -c Release`（可选，待用户确认执行阶段）确认包可生成、图标与许可证无误；检查 XML 合法性。

## Agent Extensions

### SubAgent

- **code-explorer**
- Purpose: 对每个目标 dll 类库项目递归读取其 `.vb` 源文件（命名空间、模块/类 Summary 注释、公开 API、AssemblyTitle），提取功能要点，产出 Title/PackageTags/Description/PackageReleaseNotes 草稿。
- Expected outcome: 为每个目标项目生成一份可直接写入 vbproj 的描述性元数据草稿，保证 Title/Tags 准确贴合代码内容、避免与现有 AssemblyTitle 冲突。