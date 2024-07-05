using DataAccess.Entities;
using TradeApp_Redis.DTO;

namespace TradeApp_Redis.Controllers
{
    public interface IStockService
    {
        Task<List<StockDTO>> GetStocksAsync();
        Task<Stock> AddStockAsync(NewStockDTO StockDTO);
        Task UpdateStockAsync(UpdateStockDTO StockDTO,string symbol);
        Task<StockDTO> GetStockBySymbolAsync(string symbol);
        Task DeleteStockAsync(string symbol);
    }
}
