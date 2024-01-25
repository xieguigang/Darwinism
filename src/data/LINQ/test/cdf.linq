Imports "NetCDF"

FROM x as dataframe("a", "b", "flags", "id")
IN "./dataframe.netcdf"
WHERE x.b > 50
SELECT x.id, x.a, x.b, x.flags, pow = x.a ^ x.b
ORDER BY pow DESCENDING
