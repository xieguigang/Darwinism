require(Darwinism);

imports "CDF" from "Darwinism";

const nc = CDF::open("E:\\GCModeller\\src\\runtime\\Darwinism\\src\\data\\CDF.PInvoke\\temperature.nc");
const tmin = CDF::var_data(nc, "tmin");

print(tmin);