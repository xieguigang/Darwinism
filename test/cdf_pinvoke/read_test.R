require(Darwinism);

imports "CDF" from "Darwinism";

const nc = CDF::open("E:\\GCModeller\\src\\runtime\\Darwinism\\src\\data\\CDF.PInvoke\\temperature.nc");
const tmin = CDF::var_data(nc, "tmin");

print(vars(nc));
print(as.numeric(tmin));

for(name in vars(nc)) {
    print(as.numeric(CDF::var_data(nc, name)));
}