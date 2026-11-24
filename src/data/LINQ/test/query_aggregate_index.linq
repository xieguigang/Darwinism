# 全局聚合查询：使用index函数创建带有查询索引的数据源
# 聚合之前会先使用PeakQuality字段上面的数值范围索引来缩小候选的数据行集合
AGGREGATE x as row
IN index("data.csv", range = ["PeakQuality", "CalcMz", "Area"], terms = ["Class"])
WHERE x.PeakQuality >= 0.999
INTO n = count(x),
     avgQuality = avg(x.PeakQuality),
     minMz = min(x.CalcMz),
     maxMz = max(x.CalcMz),
     sumArea = sum(x.Area),
     maxScore = max(x.AreaScore)
