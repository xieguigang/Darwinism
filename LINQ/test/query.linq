FROM x AS row 
IN "E:\GCModeller\src\runtime\Darwinism\LINQ\test\data.csv"
WHERE x.PeakQuality >= 0.999
SELECT x.LipidIon,  
       lipidName = x.Class & x.FattyAcid,   
	   x.Class,	
	   x.FattyAcid,	
	   x.Ion,	
	   x.Formula,
	   x.PeakQuality,
	   x."m-Score"
ORDER BY lipidName
TAKE 15