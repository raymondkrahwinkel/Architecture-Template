using System.Diagnostics;
using Application.Common.Interfaces.Messaging;
using Application.Common.Services;
using Application.Modules.Todos.Commands.GetTodoItemsByUser;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IQueryHandler<GetTodoItemsByUserQuery, List<TodoItemEntity>> _queryHandler;
    private readonly ICqrsSender _cqrsSender;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IQueryHandler<GetTodoItemsByUserQuery, List<TodoItemEntity>> queryHandler, ICqrsSender cqrsSender, ILogger<HomeController> logger)
    {
        _queryHandler = queryHandler;
        _cqrsSender = cqrsSender;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // var query = new GetTodoItemsByUserQuery(Guid.NewGuid());
        var query = new GetTodoItemsByUserQuery(Guid.Empty);
        var result = await _cqrsSender.SendAsync(query, cancellationToken);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}