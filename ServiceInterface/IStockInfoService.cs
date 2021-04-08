namespace MyTwse.ServiceInterface
{
    public interface IStockInfoService
    {
        void GetStockInfoByRecent(string stockCode, int days);
    }
}