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
[Route("/for/predict/")]
public class PredictController : Controller
{
    private readonly IDbConnection db;

    public PredictController(IDbConnection _db)
    {
        db = _db;
    }

    [HttpGet]
    public async Task<IActionResult> PredictCoin()
    {
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

        string sql = "";

        var coin = new BarCoin { Exchange = exchange, Per = m };

        if (isspot)
        {

            sql = $"SELECT * FROM bar_coin_spot" +
                $" WHERE exchange = @exchange AND per = @per";

        }
        else
        {

            sql = $"SELECT * FROM bar_coin " +
                $"WHERE exchange = @exchange AND per = @per";

        }

        var quotescoin = await db.QueryAsync<BarCoin>(sql, coin);

        return Ok(quotescoin);

    }

}



