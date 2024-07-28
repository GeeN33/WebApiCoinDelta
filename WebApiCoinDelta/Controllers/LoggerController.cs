using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;
using WebApiCoinDelta.Models;

namespace WebApiCoinDelta.Controllers;


[ApiController]
[Route("/logger/")]
public class LoggerController : Controller
{
    private readonly IDbConnection db;

    public LoggerController(IDbConnection _db)
    {
        db = _db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLogger([FromBody] Logger logger)
    {
        var sql = """
        INSERT INTO logger_coin (type, description, createdat)
        VALUES (@type, @description, @createdat)
        """;

        var result = await db.ExecuteAsync(sql, logger);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetLoggers()
    {

        var sql = $"SELECT * FROM logger_coin ORDER BY createdat DESC";

        var quotescoin = await db.QueryAsync<Logger>(sql);

        return Ok(quotescoin);

    }

}