namespace Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OrderAttribute(int order = int.MaxValue) : Attribute
{
    public int Order { get; init; } = order;
}