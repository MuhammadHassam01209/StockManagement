using AutoMapper;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TradeApp_Redis.DTO;

namespace TradeApp_Redis.Controllers
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StockDTO>> GetStocksAsync()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return _mapper.Map<List<StockDTO>>(stocks);
        }

        public async Task<Stock> AddStockAsync(NewStockDTO StockDTO)
        {
            var stock = _mapper.Map<Stock>(StockDTO);
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task UpdateStockAsync(UpdateStockDTO UpdateStockDTO, string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
            if (stock == null)
            {
                throw new ArgumentException("Stock not found");
            }

            _mapper.Map(UpdateStockDTO, stock);

            await _context.SaveChangesAsync();
        }

        public async Task<StockDTO> GetStockBySymbolAsync(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
            return _mapper.Map<StockDTO>(stock);
        }

        public async Task DeleteStockAsync(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
            if (stock == null)
            {
                throw new ArgumentException("Stock not found");
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }

    }
}