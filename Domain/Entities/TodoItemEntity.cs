using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class TodoItemEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2048)]
    public string Description { get; set; } = string.Empty;
    
    public DateTimeOffset? DueDate { get; set; }
    
    public bool IsCompleted { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}