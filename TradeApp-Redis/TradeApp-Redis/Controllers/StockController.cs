using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TradeApp_Redis.Controllers;
using TradeApp_Redis.DTO;
using TradeApp_Redis.RabbitMQ;

[Authorize(Policy = "PublicSecure")]
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IDistributedCache _distributedCache;
    private readonly IRabbitMQProducer _rabitMQProducer;
    public StockController(IStockService stockService, IDistributedCache distributedCache, IRabbitMQProducer rabitMQProducer)
    {
        _stockService = stockService;
        _distributedCache = distributedCache;
        _rabitMQProducer = rabitMQProducer;
    }
    
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetStocksAsync()
     {
        const string cacheKey = "stocks";
        string serializedStocks;
        var stocksFromCache = await _distributedCache.GetStringAsync(cacheKey);

        if (stocksFromCache != null)
        {
            serializedStocks = stocksFromCache;
        }
        else
        {
            var stocks = await _stockService.GetStocksAsync();
            serializedStocks = JsonSerializer.Serialize(stocks);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _distributedCache.SetStringAsync(cacheKey, serializedStocks, cacheOptions);
        }

        var stocksList = JsonSerializer.Deserialize<List<StockDTO>>(serializedStocks);
        return Ok(stocksList);
    }
    [Route("[action]/{symbol}")]
    [HttpGet]
    public async Task<IActionResult> GetStockBySymbolAsync(string symbol)
    {
        const string cacheKeyPrefix = "stock_";
        string cacheKey = cacheKeyPrefix + symbol;
        string serializedStock;
        var stockFromCache = await _distributedCache.GetStringAsync(cacheKey);

        if (stockFromCache != null)
        {
            serializedStock = stockFromCache;
        }
        else
        {
            var stock = await _stockService.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return NotFound();
            }
            serializedStock = JsonSerializer.Serialize(stock);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _distributedCache.SetStringAsync(cacheKey, serializedStock, cacheOptions);
        }

        var stockItem = JsonSerializer.Deserialize<StockDTO>(serializedStock);
        return Ok(stockItem);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AddStockAsync([FromBody] NewStockDTO stockDTO)
    {
        if (stockDTO == null)
        {
            return BadRequest("Stock data is null.");
        }

        var stockExists = await _stockService.GetStockBySymbolAsync(stockDTO.Symbol);
        if (stockExists != null)
        {
            return Conflict("Stock data already exists.");
        }

        var stock = await _stockService.AddStockAsync(stockDTO);
        _rabitMQProducer.SendMessage(stock);
        await _distributedCache.RemoveAsync("stocks");

        return NoContent();
    }

    [Route("[action]/{symbol}")]
    [HttpPut]
    public async Task<IActionResult> UpdateStockAsync(string symbol, [FromBody] UpdateStockDTO UpdateStockDTO)
    {
        if (UpdateStockDTO == null)
        {
            return BadRequest("Stock data is invalid.");
        }

        var existingStock = await _stockService.GetStockBySymbolAsync(symbol);
        if (existingStock == null)
        {
            return NotFound();
        }

        await _stockService.UpdateStockAsync(UpdateStockDTO, symbol);
        await _distributedCache.RemoveAsync("stocks");
        await _distributedCache.RemoveAsync("stock_" + symbol);

        return NoContent();
    }

    [Route("[action]/{symbol}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteStockAsync(string symbol)
    {
        var existingStock = await _stockService.GetStockBySymbolAsync(symbol);
        if (existingStock == null)
        {
            return NotFound();
        }

        await _stockService.DeleteStockAsync(symbol);
        await _distributedCache.RemoveAsync("stocks");
        await _distributedCache.RemoveAsync("stock_" + symbol);

        return NoContent();
    }
}