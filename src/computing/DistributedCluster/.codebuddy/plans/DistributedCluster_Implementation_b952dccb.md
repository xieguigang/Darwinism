---
name: DistributedCluster_Implementation
overview: 基于 Flute HTTP 服务器在 GCModeller DistributedCluster 模块中实现一套 VB.NET 计算集群运行环境：管理节点(HTTP管理+任务队列+心跳+重试)、计算节点守护进程、反射 worker 子进程宿主，以及纯静态仪表盘。SMB 采用 /mnt/cluster/jobs/{jobId}/{blocks,results,logs}/{guid} 布局。
design:
  architecture:
    framework: html
  styleKeywords:
    - Glassmorphism
    - Dark Tech
    - Neon Cyan
    - Dashboard
    - Micro-animation
  fontSystem:
    fontFamily: Roboto
    heading:
      size: 28px
      weight: 700
    subheading:
      size: 18px
      weight: 600
    body:
      size: 14px
      weight: 400
  colorSystem:
    primary:
      - "#22D3EE"
      - "#3B82F6"
      - "#0EA5E9"
    background:
      - "#0B1120"
      - "#111827"
      - "#1F2937"
    text:
      - "#E5E7EB"
      - "#94A3B8"
    functional:
      - "#22C55E"
      - "#EF4444"
      - "#F59E0B"
todos:
  - id: shared-models
    content: 新建 Shared 模块：Models/SmbPaths/Config 共享模型与路径工具
    status: completed
  - id: headnode-controller
    content: 实现 HeadNode ClusterController 的 REST 路由与静态托管
    status: completed
    dependencies:
      - shared-models
  - id: scheduler
    content: 实现 Scheduler 任务队列、重试与失败日志提取
    status: completed
    dependencies:
      - headnode-controller
  - id: node-daemon
    content: 实现 Node Daemon 轮询拉取与 ProcessRunner 启动回执
    status: completed
    dependencies:
      - shared-models
  - id: worker-host
    content: 实现 Worker 反射宿主与 SMB 读写及 ExitCode
    status: completed
    dependencies:
      - shared-models
  - id: dashboard
    content: 编写 wwwroot 仪表盘 HTML/JS/CSS 并接入 /api/status
    status: completed
    dependencies:
      - headnode-controller
  - id: program-entry
    content: 改写 Program.vb 按 --mode 分发三种角色启动
    status: completed
    dependencies:
      - headnode-controller
      - node-daemon
      - worker-host
---

## 用户需求

基于 VB.NET（dotnet 10）与 Flute HTTP 服务器模块，构建一套分布式计算集群运行环境，包含管理头结点、计算节点守护进程、反射计算子进程宿主，以及用于监控的 HTML 仪表盘。

## 核心功能

- 头结点：基于 Flute 的 HttpRouter 提供 HTTP 管理接口；提供 SMB 共享（挂载点 /mnt/cluster）；将用户提交的计算任务拆分为数据块写入 SMB（按任务分组布局 /mnt/cluster/jobs/{jobId}/{blocks,results,logs}/{guid}），并放入计算队列。
- 计算节点：安装 dotnet 10，挂载同样的 /mnt/cluster；运行轻量守护进程，每秒 HTTP 轮询头结点拉取任务；获得任务后通过命令行启动通用反射 worker 子进程，传入数据块 guid、CLR assembly 路径、CLR 方法名、SMB 任务根路径。
- 反射 worker 子进程：运行时通过 Assembly.LoadFrom + MethodInfo.Invoke 动态加载并执行任意 CLR 代码；从 SMB 读取数据块、计算后将结果写回 SMB；退出前捕获异常记录描述与栈追踪；用 Environment.ExitCode（0 成功 / 1 失败）告知守护进程结果。
- 守护进程：通过 Process 对象持续读取 worker 的 stdout，将 stdout 实时归档写入 SMB 日志文件，并每秒将 stdout 作为心跳/日志 POST 给头结点；依据 ExitCode 判定成功/失败并向头结点回执。
- 头结点管理逻辑：维护任务队列与节点心跳；按队列剩余量展示进度与失败率；失败数据块重新入队重试；重试达上限标记为失败任务并从 SMB 提取归档日志反馈。
- 仪表盘：纯静态 HTML/CSS/JS，由 Flute 托管，每 1-2 秒轮询 REST 接口，展示集群算力、负载、任务进度、失败率与失败调试日志。

## 技术栈

- 语言/运行时：VB.NET / dotnet 10（net10.0）
- HTTP 服务器：复用现有 `G:\GCModeller\src\runtime\httpd\src\Flute`（HttpRouter 控制器反射路由 + HttpSocket 监听）
- 序列化/日志：`Microsoft.VisualBasic.Core` 的 `Serialization.JSON.GetJson` 与 `App.LogException`
- 进程管理：.NET `System.Diagnostics.Process`、`System.Net.Http.HttpClient`
- 前端：纯静态 HTML/CSS/JS（无构建链），由 Flute 托管静态文件
- 工程：复用 `DistributedCluster\Host`（net10.0，已引用 Flute 与 Core），通过 `--mode=headnode|node|worker` 区分角色

## 实现方案

采用单体 Host 工程三角色共存的策略：同一 exe 依据命令行 `--mode` 进入不同入口，保证代码复用与最小部署体积。管理头结点用 `HttpRouter` 注册 REST 控制器（HttpGet/HttpPost），并通过 `HttpSocket`/`HttpDriver` 启动监听；节点守护进程为后台轮询循环；worker 作为被 Process.Start 启动的子进程入口。任务状态、队列、重试集中在头结点的内存调度器线程，配合 SMB 文件系统作为数据/日志中枢。

关键技术决策：

- 复用 `HttpRouter` 的 `RegisterController(Object)`（基于 `HttpGet`/`HttpPost` 特性反射）实现 REST 控制器，避免自行解析路由；静态资源新增一个控制器方法用 `HttpResponse.SendFile` 返回 wwwroot 文件，并设置 `AccessControlAllowOrigin` 解决跨域。
- 任务分块：头结点将大数据按固定大小切分为独立小块，每块一个 guid，写入 `{jobId}/blocks/{guid}`；分发给节点时只传 guid/assembly/method，节点从 SMB 取块、算完写 `{jobId}/results/{guid}`、日志写 `{jobId}/logs/{guid}.log`。
- 心跳与日志：节点守护进程每秒读取 worker stdout（异步），按秒归档到 SMB 日志并 POST 给 `/api/heartbeat`；头结点维护节点最后心跳时间戳用于负载/存活判断。
- 重试与失败：头结点维护每块 retryCount；回执失败则 retryCount+1 并重新入队；达上限标记为 failed 并提取日志。
- 性能：队列与状态用线程安全集合（ConcurrentQueue/Dictionary + lock）；心跳接收为轻量 POST，写内存为主、SMB 日志由节点侧完成；dashboard 轮询间隔 1-2 秒，状态接口只读内存快照，避免阻塞。

## 实现注意事项

- 严格使用已确认的 Flute API：`HttpRouter.RegisterController`/`Register`、`HttpResponse.WriteJSON`/`SendFile`、`HttpRequest.Argument`/`GetArguments`、`HttpPOSTRequest.POSTData`、特性 `HttpGet(url)`/`HttpPost(url)`；控制器方法签名必须为 `Sub(HttpRequest, HttpResponse)`。
- 守护进程读取 stdout 必须使用异步/BeginOutputRead 以避免阻塞，且需处理子进程提前退出与僵尸进程。
- worker 反射加载务必设置 `AppDomain`/AssemblyLoadContext 隔离；捕获所有异常并写日志后设置 ExitCode=1。
- 所有 SMB 路径拼接集中在共享路径工具类，避免路径分隔符错误；失败重试上限、轮询间隔、端口、SMB 根目录通过配置/命令行传入并给默认值。
- 保留 `OPTIONS /ctrl/kill` 远程关闭能力（依赖 configs.shutdown_token）。

## 架构设计

```mermaid
flowchart TB
    User[用户提交任务] --> HN[头结点 HeadNode]
    HN -->|拆分写入| SMB[(SMB /mnt/cluster/jobs/{jobId})]
    Node[计算节点 Daemon] -->|每秒轮询 GET /api/task/pull| HN
    HN -->|返回 guid/assembly/method| Node
    Node -->|Process.Start| Worker[反射 Worker 子进程]
    Worker -->|读 blocks/写 results| SMB
    Worker -->|stdout| Node
    Node -->|每秒 POST /api/heartbeat| HN
    Node -->|POST /api/task/done 或 /api/task/failed| HN
    HN -->|GET /api/status + 静态| Dash[HTML 仪表盘]
```

头结点内部：REST 控制器 + 任务调度线程（队列/重试/失败提取）+ 集群状态快照；节点内部：轮询器 + 进程管理器 + 日志归档器。

## 目录结构

DistributedCluster\Host\
├── Program.vb              # [MODIFY] 入口：解析 --mode 分发 headnode/node/worker；headnode 启动 HttpSocket+Router 并托管 wwwroot。
├── Shared/
│   ├── Models.vb          # [NEW] 共享模型：TaskBlock、NodeHeartbeat、ClusterStatus、TaskResult 等；用 GetJson 序列化。
│   ├── SmbPaths.vb        # [NEW] SMB 路径工具：JobsRoot、BlockPath(guid)、ResultPath(guid)、LogPath(guid) 按 jobId 分组。
│   └── Config.vb          # [NEW] 配置：HTTP 端口、SMB 根、轮询间隔、重试上限、shutdown_token；命令行/默认值解析。
├── HeadNode/
│   ├── ClusterController.vb # [NEW] HttpRouter 控制器：提交/拆分、pull、heartbeat、done、failed、status、静态文件；方法标注 HttpGet/HttpPost。
│   └── Scheduler.vb        # [NEW] 任务队列与重试调度线程：ConcurrentQueue、retryCount、失败标记与日志提取。
├── Node/
│   ├── Daemon.vb          # [NEW] 守护进程主循环：每秒拉取任务、启动 worker、读 stdout 归档+心跳、按 ExitCode 回执。
│   └── ProcessRunner.vb   # [NEW] 封装 Process.Start/输出读取/ExitCode 判定与回执逻辑。
├── Worker/
│   └── ReflectHost.vb     # [NEW] worker 入口：解析参数、Assembly.LoadFrom+MethodInfo.Invoke、SMB 读写、异常日志、ExitCode。
└── wwwroot/
├── index.html         # [NEW] 仪表盘页面骨架（算力/负载/进度/失败率/日志面板）。
├── app.js             # [NEW] 每 1-2 秒轮询 /api/status，渲染状态与失败调试日志。
└── style.css          # [NEW] 仪表盘样式（深色、卡片、状态色）。

## 设计风格

采用深色科技风（Glassmorphism 玻璃拟态 + 霓虹蓝青色）构建集群监控仪表盘。纯静态页面由 Flute 托管，前端每 1-2 秒轮询 `/api/status` 拉取集群快照并局部刷新，无需整页刷新。

## 页面规划（单页仪表盘）

- 顶部导航栏：集群名称、运行模式徽标、当前时间、节点在线数。
- 算力/负载概览卡片区：总节点数、在线节点、CPU/线程负载占比、集群算力指数（在线节点×核心数）。
- 任务进度区：队列剩余、已完成/失败/进行中计数进度条、失败率百分比。
- 节点心跳列表：各节点最近心跳时间、状态（在线/失联）、当前任务 guid。
- 失败调试面板：失败数据块列表，展开可查看从 SMB 归档提取的异常描述与栈追踪日志。
- 日志流：滚动展示各节点实时心跳/stdout 日志（按秒更新）。
- 底部状态栏：SMB 根路径、HTTP 端口、轮询间隔等元信息。

## 交互与响应式

- 卡片 hover 微光效；进度条平滑过渡；失败项可点击展开详情。
- 布局采用 CSS Grid，桌面端多列、窄屏单列自适应。
- 所有数据通过 fetch 轮询更新，避免 WebSocket 依赖，保持零额外前端依赖。