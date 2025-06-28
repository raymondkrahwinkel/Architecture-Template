using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class UserEntity : IdentityUser<Guid>
{
    public ICollection<TodoItemEntity> TodoItems { get; set; } = new List<TodoItemEntity>();
}