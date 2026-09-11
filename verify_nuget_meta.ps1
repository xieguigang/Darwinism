$ErrorActionPreference = "Stop"
$src = "g:/GCModeller/src/runtime/Darwinism/src"
$rels = @(
  "networking/networking.vbproj",
  "Darwinism/Darwinism.vbproj",
  "CloudKit/Centos/Centos.NET5.vbproj",
  "CloudKit/Docker/Docker.NET5.vbproj",
  "CloudKit/ossutil/ossutil.NET5.vbproj",
  "computing/IPCDriver/IPCDriver.vbproj",
  "computing/Parallel/parallel-netcore5.vbproj",
  "DataScience/DataMining/DataMining.vbproj",
  "message/OpenApi3.0/OpenApi3.0.vbproj",
  "message/Rpc/Rpc.vbproj",
  "message/XDRStream/XDRStream.vbproj",
  "data/BucketDb/BucketDb.vbproj",
  "data/CDF.PInvoke/CDF.PInvoke.vbproj",
  "data/FullTextBuffer/FullTextBuffer.vbproj",
  "computing/DistributedCluster/ClusterShared/ClusterShared.vbproj",
  "data/LINQ/GQL/GQL.vbproj",
  "data/LINQ/LINQ/LINQ.vbproj",
  "data/LINQ/RQL/RQL.vbproj",
  "data/LINQ/Drivers/NetCDF/NetCDF.vbproj",
  "data/LINQ/Drivers/Sqlite3/Sqlite3.vbproj"
)
$ok = $true
foreach ($rel in $rels) {
  $p = Join-Path $src $rel
  try { [xml]$x = Get-Content $p } catch { Write-Host "$rel : XML_INVALID"; $ok = $false; continue }
  $ns = $x.DocumentElement.NamespaceURI
  $mgr = New-Object System.Xml.XmlNamespaceManager($x.NameTable)
  $mgr.AddNamespace("m", $ns)
  $pg = @($x.SelectNodes("//m:PropertyGroup", $mgr)) | Where-Object { -not $_.Condition } | Select-Object -First 1
  $icon = $pg.SelectSingleNode("m:PackageIcon", $mgr).InnerText
  $iconOk = Test-Path (Join-Path (Split-Path $p) $icon)
  $licFile = $pg.SelectSingleNode("m:PackageLicenseFile", $mgr)
  $req = @("Title","PackageTags","Description","PackageReleaseNotes","PackageProjectUrl","RepositoryUrl","RepositoryType","PackageLicenseExpression","Copyright","Authors","Company","Product","PackageIcon","GeneratePackageOnBuild") | Where-Object { -not $pg.SelectSingleNode("m:$_", $mgr) }
  $noneNodes = @($x.SelectNodes("//m:None", $mgr)) | Where-Object { $_.Include -like "*icons8-repository-96.png" }
  $packed = (@($noneNodes | Where-Object { $_.Pack -eq "True" -and $_.PackagePath -eq "\" }).Count -gt 0)
  $licNone = @($x.SelectNodes("//m:None", $mgr)) | Where-Object { $_.Include -like "*LICENSE" }
  if ($req -or -not $iconOk -or $licFile -or -not $packed -or $licNone) { $ok = $false }
  Write-Host ("$rel : iconResolves={0} missing=[{1}] licFile={2} iconPacked={3} licNone={4}" -f $iconOk, ($req -join ","), ($null -ne $licFile), $packed, $licNone.Count)
}
Write-Host ("ALL_GOOD={0}" -f $ok)
