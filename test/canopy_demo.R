require(Darwinism);

let testdata = read.csv("\\192.168.1.254\backup3\项目以外内容\2024\动物器官3D重建测试\test3.0\test20240115\tmp\workflow_tmp\spatial_embedding\umap3.csv", 
    row.names = 1, check.names = FALSE);

print(testdata, max.print = 6);

# set number of host process for run IPC parallel
Environment::set_threads(12);

# calculate T2 for canopy method
const T2 = Math::average_distance(testdata);

print(T2);
