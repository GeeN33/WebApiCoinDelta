using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;
using System.Runtime.ConstrainedExecution;
using WebApiCoinDelta.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiCoinDelta.Controllers;


[ApiController]
[Route("/info-coin/")]
public class InfoCoinController : Controller
{
    private readonly IDbConnection db;

    public InfoCoinController(IDbConnection _db)
    {
        db = _db;
    }

    [HttpGet]
    public async Task<IActionResult> InfoCoin()
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

        var coin = new BarCoinLopped { Exchange = exchange, Symbol = symbol, Per = m, IsSpot = isspot };

        var sql = $"SELECT * FROM bar_coin" +
            $" WHERE exchange = @exchange AND symbol = @symbol AND per = @per AND isspot = @isspot " +
            $"ORDER BY index DESC";

        var barCoinLoppedList = await db.QueryAsync<BarCoinLopped>(sql, coin);

        return Ok(barCoinLoppedList);

    }

}

