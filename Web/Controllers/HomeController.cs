using System.Diagnostics;
using Application.Common.Services;
using Application.Modules.Todos;
using Application.Modules.Todos.Commands.AddTodoItem;
using Application.Modules.Todos.Commands.GetTodoItemById;
using Application.Modules.Todos.Commands.GetTodoItemsByUser;
using Application.Modules.Todos.Commands.UpdateTodoItem;
using Application.Modules.Todos.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ICqrsSender _cqrsSender;
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ICqrsSender cqrsSender, UserManager<UserEntity> userManager, ILogger<HomeController> logger)
    {
        _cqrsSender = cqrsSender;
        _userManager = userManager;
        _logger = logger;

        TodoItemEvents.TodoItemAdded += OnTodoItemAdded;
    }

    protected override void Dispose(bool disposing)
    {
        TodoItemEvents.TodoItemAdded -= OnTodoItemAdded;
        
        base.Dispose(disposing);
    }
    
    private void OnTodoItemAdded(object? _, Guid id)
    {
        _logger.LogInformation("Todo item added: {TodoItemId}", id);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogError("User not found for the current context.");
            ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
            return RedirectToAction(nameof(Index));
        }
        
        var query = new GetTodoItemsByUserQuery(user.Id);
        // var query = new GetTodoItemsByUserQuery(Guid.Empty);
        var result = await _cqrsSender.SendAsync(query, cancellationToken);
        if (result is not { IsSuccess: true })
        {
            result?.AddToModelState(ModelState); // add the validation errors from the backend validator to the model state
            
            _logger.LogError("Failed to retrieve todo items: {Error}", result?.Error?.Message ?? "Result is null");
            return View(new List<TodoItemViewModel>());
        }
        
        return View(result.Value?.Select(TodoItemViewModel.FromDto).ToList() ?? []);
    }

    [HttpGet, Authorize]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(TodoItemViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("User not found for the current context.");
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return View(model);
            }
            
            var command = new AddTodoItemCommand(new TodoItemDto
            {
                UserId = user.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate
            });

            var result = await _cqrsSender.SendAsync(command, cancellationToken);
            if (result is not { IsSuccess: true })
            {
                _logger.LogError("Failed to add todo item: {Error}", result?.Error?.Message ?? "Result is null");
                ModelState.AddModelError(string.Empty, result?.Error?.Message ?? "An error occurred while adding the todo item.");
                
                result?.AddToModelState(ModelState); // add the validation errors from the backend validator to the model state
                
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        
        return View(model);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }
        
        var query = new GetTodoItemByIdQuery(id, user.Id);
        var result = await _cqrsSender.SendAsync(query, cancellationToken);
        
        if (result is not { IsSuccess: true })
        {
            _logger.LogError("Failed to retrieve todo item: {Error}", result?.Error?.Message ?? "Result is null");
            return RedirectToAction(nameof(Index));
        }

        var todoItem = result.Value;
        if (todoItem == null)
        {
            _logger.LogWarning("Todo item with ID {Id} not found.", id);
            return NotFound();
        }

        var model = new TodoItemViewModel
        {
            Title = todoItem.Title,
            Description = todoItem.Description,
            DueDate = todoItem.DueDate
        };

        return View(model);
    }
    
    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TodoItemViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("User not found for the current context.");
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return View(model);
            }
            
            var command = new UpdateTodoItemCommand(new TodoItemDto
            {
                Id = id,
                UserId = user.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate
            });

            var result = await _cqrsSender.SendAsync(command, cancellationToken);
            if (result is not { IsSuccess: true })
            {
                _logger.LogError("Failed to update todo item: {Error}", result?.Error?.Message ?? "Result is null");
                ModelState.AddModelError(string.Empty, result?.Error?.Message ?? "An error occurred while updating the todo item.");
                
                result?.AddToModelState(ModelState); // add the validation errors from the backend validator to the model state
                
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        
        return View(model);
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