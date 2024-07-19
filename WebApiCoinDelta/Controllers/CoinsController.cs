using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;
using WebApiCoinDelta.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiCoinDelta.Controllers;

[ApiController]
[Route("/symbols/")]
public class CoinsController : Controller
{
    private readonly IDbConnection db;

    public CoinsController(IDbConnection _db)
    {
        db = _db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSymbol([FromBody] BarCoin coin)
    {
        var sql = """
        INSERT INTO bar_coin (symbol, exchange, isspot, per, datetime, open, high, low, close, delta_buy, delta_sell, index)
        VALUES (@symbol, @exchange, @isspot, @per, @datetime, @open, @high, @low, @close, @delta_buy, @delta_sell, @index)
        """;

        var result = await db.ExecuteAsync(sql, coin);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdataSymbol(int id, [FromBody] BarCoinUpdate updatecoin)
    {
        var sql = @"
        UPDATE bar_coin
        SET
            datetime = @Datetime,
            open = @Open,
            high = @High,
            low = @Low,
            close = @Close,
            delta_buy = @Delta_buy,
            delta_sell = @Delta_sell
        WHERE id = @Id";

        var parameters = new
        {
            Id = id,
            updatecoin.Datetime,
            updatecoin.Open,
            updatecoin.High,
            updatecoin.Low,
            updatecoin.Close,
            updatecoin.Delta_buy,
            updatecoin.Delta_sell
        };

        var result = await db.ExecuteAsync(sql, parameters);

        if (result > 0)
        {
            return Ok(new { Message = "Bar Coin updated successfully" });
        }

        return NotFound(new { Message = "Bar Coin not found" });
    }

    [HttpGet]
    public async Task<IActionResult> GetSymbols()
    {
        var symbol = HttpContext.Request.Query["symbol"].FirstOrDefault() ?? "BTCUSDT";
        var exchange = HttpContext.Request.Query["exchange"].FirstOrDefault() ?? "binance";
        var per = HttpContext.Request.Query["per"].FirstOrDefault() ?? "5";

        var isspotStr = HttpContext.Request.Query["isspot"].FirstOrDefault();
        bool isspot = false;
        if (!string.IsNullOrEmpty(isspotStr))
        {
            bool.TryParse(isspotStr, out isspot);
        }

        int m = 5;
        try
        {
            m = Int32.Parse(per);
        }
        catch (FormatException)
        {
            m = 5;
        }

        var coin = new BarCoin { Exchange = exchange, Symbol = symbol, Per = m, IsSpot = isspot };

        var sql = $"SELECT * FROM bar_coin " +
            $"WHERE exchange = @exchange AND symbol = @symbol AND per = @per AND isspot = @isspot " +
            $"ORDER BY index ";

        var quotescoin = await db.QueryAsync<BarCoin>(sql, coin);

        return Ok(quotescoin);

    }

}
