namespace Formster.Domain.Entities;

public class FormSubmission
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FormName { get; set; } = string.Empty;

    public string JsonData { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
