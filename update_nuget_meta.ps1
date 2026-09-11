$ErrorActionPreference = "Stop"
$src = "g:/GCModeller/src/runtime/Darwinism/src"
$pngFile = "G:/GCModeller/src/runtime/Darwinism/.pkg/icons8-repository-96.png"

# Per-project descriptive metadata (Title / PackageTags / Description / PackageReleaseNotes)
$meta = @{
  "networking/networking.vbproj" = @{ Title="Darwinism IPC Networking Library"; Tags="networking;tcp;socket;ipc;hpc;darwinism"; Desc="Provides TCP/IP socket networking primitives for the Darwinism IPC layer, including an asynchronous multithreaded server socket, client/server wrappers, reflection-based protocol request handlers, an MMF-backed pipeline, HTTP crawling helpers, and CAPTCHA image generation."; RN="Initial GPL-3.0-only release of the Darwinism IPC networking component." }
  "Darwinism/Darwinism.vbproj" = @{ Title="Darwinism R# HPC Framework"; Tags="rsharp;ipc;hpc;darwinism;cloud"; Desc="The root R# extension package for the Darwinism high-performance computing framework, exposing IPC parallel environment configuration, parallel math (KNN, Pearson correlation, average distance), Linux/CentOS and Docker tooling, and OSS cloud-storage helpers through R-language bindings."; RN="Initial GPL-3.0-only release of the Darwinism HPC framework package." }
  "CloudKit/Centos/Centos.NET5.vbproj" = @{ Title="Darwinism CentOS Remote Helper"; Tags="centos;linux;ssh;hpc;darwinism"; Desc="Helpers for remote Linux/CentOS management from .NET, including SSH and PuTTY (plink) automation, a bash command-interaction shell, common command parsers, and /proc/net/tcp and /proc/net/udp socket-table readers."; RN="Initial GPL-3.0-only release of the Darwinism CentOS remote helper component." }
  "CloudKit/Docker/Docker.NET5.vbproj" = @{ Title="Darwinism Docker CLI Wrapper"; Tags="docker;container;hpc;darwinism;cloud"; Desc="A .NET wrapper around the Docker CLI that models container run environments and builds docker run command lines with mounts, environment variables, and port forwarding, plus parsing of docker inspect JSON to analyze image layer hierarchies."; RN="Initial GPL-3.0-only release of the Darwinism Docker wrapper component." }
  "CloudKit/ossutil/ossutil.NET5.vbproj" = @{ Title="Darwinism OSS Cloud Storage Helper"; Tags="oss;aliyun;cloud;hpc;darwinism"; Desc="A cloud object-storage helper, primarily for Alibaba Cloud OSS via the ossutil CLI, providing a filesystem-style abstraction over buckets and objects, file copy/upload/download, Aspera (fasp) transfer support, and IDM batch-download integration."; RN="Initial GPL-3.0-only release of the Darwinism OSS cloud-storage component." }
  "computing/IPCDriver/IPCDriver.vbproj" = @{ Title="Darwinism IPC Driver Services"; Tags="ipc;driver;hpc;darwinism;cluster"; Desc="Drivers that host Darwinism IPC protocol handlers as network services, exposing a ProtocolHandler over an HTTP server and over a raw TCP server, plus NanoCluster client-agent and master scaffolding for distributed task submission and heartbeats."; RN="Initial GPL-3.0-only release of the Darwinism IPC driver component." }
  "computing/Parallel/parallel-netcore5.vbproj" = @{ Title="Darwinism HPC Parallel Library"; Tags="parallel;ipc;hpc;darwinism;mmf"; Desc="The Darwinism high-performance parallel compute library for .NET, implementing IPC-based parallel task execution across slave nodes, shared-memory (memory-mapped file) pipes for fast inter-process data exchange, and thread-pool task scheduling."; RN="Initial GPL-3.0-only release of the Darwinism HPC parallel component." }
  "DataScience/DataMining/DataMining.vbproj" = @{ Title="Darwinism Data Mining Library"; Tags="datamining;clustering;knn;hpc;darwinism"; Desc="A data-mining library for the Darwinism framework providing parallel K-nearest-neighbor search, Pearson correlation network construction, genetic-algorithm scaffolding, and typed file serializers for vector and neighbor data."; RN="Initial GPL-3.0-only release of the Darwinism data-mining component." }
  "message/OpenApi3.0/OpenApi3.0.vbproj" = @{ Title="OpenApi3.0 Source Generator"; Tags="openapi;code-generator;ipc;hpc;darwinism"; Desc="A VB.NET source generator that parses OpenAPI 3.0.1 documents and emits strongly-typed model classes, REST API client classes, and HTTP infrastructure from the spec's components/schemas and paths, for building typed IPC/API clients inside the Darwinism HPC stack."; RN="Initial GPL-3.0-only release of the Darwinism OpenAPI 3.0 source generator component." }
  "message/Rpc/Rpc.vbproj" = @{ Title="ONC RPC Client and Portmapper"; Tags="rpc;ipc;oncrpc;xdr;hpc;darwinism"; Desc="An ONC RPC (RFC 5531) style messaging library implementing the RPC call/reply message protocol over XDR, with TCP and UDP connectors and a RpcClient/IRpcClient abstraction, including port-mapper binding protocols and exception types."; RN="Initial GPL-3.0-only release of the Darwinism ONC RPC messaging component." }
  "message/XDRStream/XDRStream.vbproj" = @{ Title="XDR Stream Serialization Library"; Tags="xdr;serialization;rpc;hpc;darwinism"; Desc="A serialization framework for External Data Representation (XDR, RFC 4506) streams, providing abstract Reader/Writer bases plus dynamic-emit mappers for fixed/var/option/opaque encodings, driven by attributes over CLR types. It underpins the Darwinism RPC wire format."; RN="Initial GPL-3.0-only release of the Darwinism XDR stream serialization component." }
  "data/BucketDb/BucketDb.vbproj" = @{ Title="In-Memory Key-Value BucketDb"; Tags="key-value;storage;in-memory;hpc;darwinism"; Desc="A hashcode-bucketed in-memory key-value database with optional disk persistence, an L1 hot cache and an L2 in-memory file index, per-bucket fine-grained locking, and a background worker that flushes dirty indexes and evicts cold cache entries."; RN="Initial GPL-3.0-only release of the Darwinism in-memory key-value BucketDb component." }
  "data/CDF.PInvoke/CDF.PInvoke.vbproj" = @{ Title="netCDF Native P/Invoke Bindings"; Tags="netcdf;pinvoke;scientific-data;hpc;darwinism"; Desc="A managed P/Invoke wrapper around the native netCDF C library, exposing the full module of nc_* functions for dimensions, variables, groups, attributes, and user types, with OpenMode/CreateMode flags, Tensor helpers, and a DataReader convenience layer for scientific array I/O from VB.NET."; RN="Initial GPL-3.0-only release of the Darwinism netCDF P/Invoke bindings component." }
  "data/FullTextBuffer/FullTextBuffer.vbproj" = @{ Title="Full-Text Inverted Index Buffer"; Tags="full-text;inverted-index;search;hpc;darwinism"; Desc="A compact binary (de)serialization library for full-text inverted indexes, reading and writing an InvertedIndex (token to document-id postings) together with document offset tables to/from streams, building on the LINQ project's indexing types."; RN="Initial GPL-3.0-only release of the Darwinism full-text inverted index buffer component." }
  "computing/DistributedCluster/ClusterShared/ClusterShared.vbproj" = @{ Title="HPC Cluster Shared Models"; Tags="hpc;cluster;distributed;computing;darwinism"; Desc="Shared contracts and configuration for the Darwinism distributed cluster, defining head-node/node/worker run modes, HTTP port, SMB data-hub root, poll interval, retry limits, and the storage layout shared across cluster nodes."; RN="Initial GPL-3.0-only release of the Darwinism HPC distributed cluster shared component." }
  "data/LINQ/GQL/GQL.vbproj" = @{ Title="Graph Query Language Interpreter"; Tags="graph;query-language;gql;hpc;darwinism"; Desc="The Graph Query Language (GQL) interpreter component, built on top of the Darwinism LINQ query engine and the scientific graph library, exposing a GQLInterpreter entry point for evaluating graph-structured queries against in-memory graph data."; RN="Initial GPL-3.0-only release of the Darwinism Graph Query Language interpreter component." }
  "data/LINQ/LINQ/LINQ.vbproj" = @{ Title="Darwinism LINQ Query Runtime"; Tags="linq;query-language;full-text;search;hpc;darwinism"; Desc="A LINQ-style query language runtime and CLI that tokenizes .linq scripts and executes projection/filter queries against in-memory data, with full-text, term-hash, Levenshtein, and value-range searches via indexing types, shipping a CLI that outputs results as CSV or console tables."; RN="Initial GPL-3.0-only release of the Darwinism LINQ query language runtime component." }
  "data/LINQ/RQL/RQL.vbproj" = @{ Title="The Resource Query Language"; Tags="resource;query-language;rql;full-text;hpc;darwinism"; Desc="The Resource Query Language (RQL) engine for indexing and querying string/file resources stored in a StreamPack archive, with a trie-based tree index for access and pluggable indexers (CSV, MongoDB) for full-text and hash lookups, layering on top of the LINQ and FullTextBuffer components."; RN="Initial GPL-3.0-only release of the Darwinism Resource Query Language component." }
  "data/LINQ/Drivers/NetCDF/NetCDF.vbproj" = @{ Title="NetCDF DataFrame Driver for Darwinism LINQ"; Tags="netcdf;dataframe;darwinism;hpc;data-driver"; Desc="A data-source driver for the Darwinism LINQ engine that reads NetCDF scientific data files, loading named data variables and yielding each record as a JavaScriptObject row to materialize a dataframe-like stream for HPC pipelines."; RN="Initial GPL-3.0-only release of the Darwinism NetCDF dataframe driver component." }
  "data/LINQ/Drivers/Sqlite3/Sqlite3.vbproj" = @{ Title="SQLite Table Reader Driver for Darwinism LINQ"; Tags="sqlite;table;darwinism;hpc;data-driver"; Desc="A data-source driver for the Darwinism LINQ engine that reads tables from SQLite database files, selecting the target table by name, parsing its schema columns, and yielding each row as a JavaScriptObject for SQLite-backed tabular data access within the Darwinism HPC runtime."; RN="Initial GPL-3.0-only release of the Darwinism SQLite table reader driver component." }
}

$globalVals = @{
  PackageProjectUrl = "https://scibasic.net/"
  RepositoryUrl = "https://github.com/xieguigang/Darwinism.git"
  RepositoryType = "git"
  PackageLicenseExpression = "GPL-3.0-only"
  Copyright = "Copyright © scibasic.net foundation, GuiLin China, 2026"
  Authors = "xieguigang<xie.guigang@gmail.com>"
  Company = "scibasic.net foundation"
  Product = "Darwinism HPC"
}

function GetRelPath($from, $to) {
  $fromUri = New-Object System.Uri(($from.TrimEnd('\', '/') + '/'))
  $toUri = New-Object System.Uri($to)
  $rel = $fromUri.MakeRelativeUri($toUri).ToString()
  $rel -replace '/', '\'
}

function SetOrAdd($parent, $nsmgr, $name, $value) {
  $el = $parent.SelectSingleNode("m:$name", $nsmgr)
  if ($el) {
    $el.InnerText = $value
  } else {
    $child = $parent.OwnerDocument.CreateElement($name, $parent.NamespaceURI)
    $child.InnerText = $value
    $parent.AppendChild($child) | Out-Null
  }
}

foreach ($rel in $meta.Keys) {
  $proj = Join-Path $src $rel
  Write-Host "Processing $rel"
  $xml = New-Object System.Xml.XmlDocument
  $xml.Load($proj)
  $nsmgr = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
  $nsmgr.AddNamespace("m", $xml.DocumentElement.NamespaceURI)

  $projDir = Split-Path $proj
  $relIcon = GetRelPath $projDir $pngFile
  Write-Host "  PackageIcon = $relIcon"

  # main unconditional PropertyGroup
  $pgs = @($xml.SelectNodes("//m:PropertyGroup", $nsmgr))
  $pg = $pgs | Where-Object { -not $_.Condition } | Select-Object -First 1

  $m = $meta[$rel]
  SetOrAdd $pg $nsmgr "Title" $m.Title
  SetOrAdd $pg $nsmgr "PackageTags" $m.Tags
  SetOrAdd $pg $nsmgr "Description" $m.Desc
  SetOrAdd $pg $nsmgr "PackageReleaseNotes" $m.RN
  SetOrAdd $pg $nsmgr "PackageProjectUrl" $globalVals.PackageProjectUrl
  SetOrAdd $pg $nsmgr "RepositoryUrl" $globalVals.RepositoryUrl
  SetOrAdd $pg $nsmgr "RepositoryType" $globalVals.RepositoryType
  SetOrAdd $pg $nsmgr "PackageLicenseExpression" $globalVals.PackageLicenseExpression
  SetOrAdd $pg $nsmgr "Copyright" $globalVals.Copyright
  SetOrAdd $pg $nsmgr "Authors" $globalVals.Authors
  SetOrAdd $pg $nsmgr "Company" $globalVals.Company
  SetOrAdd $pg $nsmgr "Product" $globalVals.Product
  SetOrAdd $pg $nsmgr "PackageIcon" $relIcon
  SetOrAdd $pg $nsmgr "GeneratePackageOnBuild" "true"

  # remove conflicting PackageLicenseFile (switching to expression license)
  $licFile = $pg.SelectSingleNode("m:PackageLicenseFile", $nsmgr)
  if ($licFile) { $pg.RemoveChild($licFile) | Out-Null }

  # icon packing None item
  $nones = @($xml.SelectNodes("//m:None", $nsmgr))
  $iconNone = $nones | Where-Object { $_.Include -like "*icons8-repository-96.png" } | Select-Object -First 1
  if (-not $iconNone) {
    $ig = @($xml.SelectNodes("//m:ItemGroup", $nsmgr)) | Select-Object -First 1
    if (-not $ig) { $ig = $xml.CreateElement("ItemGroup", $xml.DocumentElement.NamespaceURI); $xml.Project.AppendChild($ig) | Out-Null }
    $iconNone = $xml.CreateElement("None", $xml.DocumentElement.NamespaceURI)
    $iconNone.SetAttribute("Include", $relIcon)
    $ig.AppendChild($iconNone) | Out-Null
  } else {
    $iconNone.SetAttribute("Include", $relIcon)
  }
  SetOrAdd $iconNone $nsmgr "Pack" "True"
  SetOrAdd $iconNone $nsmgr "PackagePath" "\"

  # remove LICENSE packing items (no longer needed with expression license)
  $licNones = @($xml.SelectNodes("//m:None", $nsmgr)) | Where-Object { $_.Include -like "*LICENSE" }
  foreach ($ln in $licNones) { $ln.ParentNode.RemoveChild($ln) | Out-Null }

  $xml.Save($proj)
  Write-Host "  Saved."
}
Write-Host "DONE"
