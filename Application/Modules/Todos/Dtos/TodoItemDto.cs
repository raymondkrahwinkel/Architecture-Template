using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Modules.Todos.Dtos;

public class TodoItemDto
{
    public Guid? Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    
    public class TodoItemDtoValidator : AbstractValidator<TodoItemDto> 
    {
        private readonly IApplicationDbContext _dbContext;

        public TodoItemDtoValidator(IApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
            RuleFor(x => x.Title).NotNull().Length(0, 200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(2048);
            RuleFor(x => x).Must(TitleMustBeUnique).WithMessage("Title must be unique.");
        }

        private bool TitleMustBeUnique(TodoItemDto todoItem)
        {
            var title = todoItem.Title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                return false; // Title cannot be empty or whitespace
            }
        
            // Check if the title already exists in the database for another item
            var existingItem = _dbContext.TodoItems
                .FirstOrDefault(x => x.Title.ToLower() == title.ToLower() && x.Id != todoItem.Id);
            if (existingItem != null)
            {
                return false; // Title already exists for another item
            }
        
            return true; // Title is unique
        }
    }
    
    public static TodoItemDto FromEntity(TodoItemEntity entity)
    {
        return new TodoItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted,
            DueDate = entity.DueDate
        };
    }
    
    public TodoItemEntity ToEntity(TodoItemEntity? entity = null)
    {
        // Ensure Id is set if not provided
        Id ??= Guid.NewGuid();
        
        entity ??= new TodoItemEntity();
        entity.Id = Id.Value;
        entity.UserId = UserId;
        entity.Title = Title;
        entity.Description = Description;
        entity.IsCompleted = IsCompleted;
        entity.DueDate = DueDate;

        return entity;
    }
}