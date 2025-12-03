public class TaskAssignmentModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Station { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public string Status { get; set; } = "Pending";
    public int? AssigneeId { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today;
    public int LocationId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
