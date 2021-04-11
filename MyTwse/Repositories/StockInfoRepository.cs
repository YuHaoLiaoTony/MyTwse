using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyTwse.IRepositories;
using MyTwse.Models;
using MyTwse.Models.QueryModels;
using MyTwse.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MyTwse.Repositories
{
    public class StockInfoRepository : BaseTwseStockRepository<StockInfo>, IStockInfoRepository
    {
        public List<YieldRateIncreaseＭaxDaysReportModel> GetYieldRateIncreaseＭaxDays(YieldRateIncreaseＭaxDaysQueryModel queryModel)
        {
            string sql = @"

WITH 
SI AS
(
	SELECT*,ROW_NUMBER() OVER (ORDER BY [DATE]) AS DateRowNum
	FROM StockInfo
	WHERE CODE = @Code
), 
T3 AS
(
	SELECT 
		MIN([Date])MinDate
	    ,MAX([Date])MaxDate
	    ,COUNT(1) Days
	FROM
	(
		SELECT 
			[DATE]	
			,DateRowNum - ROW_NUMBER() OVER (ORDER BY DateRowNum) AS RowNum
		FROM
		(
			SELECT 
			    S1.[Date]
			    ,CASE 
			    	WHEN (S2.YieldRate-S1.YieldRate) > 0 OR S2.YieldRate IS NULL THEN 1 ELSE 0 END IsIncrease
			    ,S1.DateRowNum
			FROM SI S1
			LEFT JOIN SI S2 ON S1.DateRowNum = S2.DateRowNum - 1
			WHERE S1.CODE = @Code AND S1.[Date] >= @StartDate AND S1.[Date] <= @EndDate
		) AS T1
		WHERE T1.IsIncrease = 1
	)AS T2
	GROUP BY RowNum
)
SELECT * 
FROM T3 
WHERE Days = (SELECT MAX(Days)FROM T3)
";
            return ExecSQL<YieldRateIncreaseＭaxDaysReportModel>(sql, queryModel);
        }
        
        public void Create(List<StockInfo> stockInfos, List<InsertDateLog> insertDateLogs)
        {
            foreach (var item in stockInfos)
            {
                _DB.Add(item);
            }
            foreach (var item in insertDateLogs)
            {
                _DB.Add(item);
            }
            _DB.SaveChanges();
        }
    }
}
