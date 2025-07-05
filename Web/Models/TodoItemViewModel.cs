using System.ComponentModel.DataAnnotations;
using Application.Modules.Todos.Dtos;

namespace Web.Models;

public class TodoItemViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    
    [MaxLength(200), Required]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2048), Required]
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    
    public static TodoItemViewModel FromDto(TodoItemDto dto)
    {
        if (dto.Id == null)
        {
            throw new ArgumentException("DTO Id cannot be null", nameof(dto));
        }
        
        return new TodoItemViewModel
        {
            Id = dto.Id.Value,
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    }
}