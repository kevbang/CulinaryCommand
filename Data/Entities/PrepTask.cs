namespace CulinaryCommand.Data.Entities;

public class PrepTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public int Par { get; set; }
    public int Count { get; set; }
    public int Prep => Math.Max(Par - Count, 0);
    public int? DurationMin { get; set; } = 30;
    public bool IsDone { get; set; }
}
