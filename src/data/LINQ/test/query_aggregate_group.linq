# 分组聚合查询：使用index函数创建带有查询索引的数据源
# Class字段上面的词项哈希索引直接提供了分组键以及分组键所对应的数据行偏移
AGGREGATE x as row
IN index("data.csv", terms = ["Class"], range = ["PeakQuality", "CalcMz"])
WHERE x.PeakQuality >= 0.9
GROUP BY x.Class
INTO n = count(x),
     avgQuality = avg(x.PeakQuality),
     maxMz = max(x.CalcMz),
     sumArea = sum(x.Area)
ORDER BY n DESCENDING
TAKE 10
