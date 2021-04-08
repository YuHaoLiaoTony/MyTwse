namespace MyTwse.Models.ReportModel
{
    public class StockInfoJsonModel
    {
        /// <summary>
        /// 狀態 EX. OK
        /// </summary>
        public string Stat { get; set; }
        /// <summary>
        /// EX. 20210407
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// EX. 110年04月07日 個股日本益比、殖利率及股價淨值比
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Fields { get; set; }
        public object[][] Data { get; set; }
        public string SelectType { get; set; }
        public string[] Notes { get; set; }
    }
}
