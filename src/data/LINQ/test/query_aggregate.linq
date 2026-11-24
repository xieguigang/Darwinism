# 全局聚合查询：不带索引的数据源，使用全表扫描来完成聚合计算
AGGREGATE x as row
IN "data.csv"
WHERE x.PeakQuality >= 0.999
INTO n = count(x),
     avgQuality = avg(x.PeakQuality),
     minMz = min(x.CalcMz),
     maxMz = max(x.CalcMz),
     sumArea = sum(x.Area),
     maxScore = max(x.AreaScore)
