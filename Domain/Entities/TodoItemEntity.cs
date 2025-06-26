using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TodoItemEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2048)]
    public string Description { get; set; } = string.Empty;
}