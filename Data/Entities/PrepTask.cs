namespace CulinaryCommand.Data.Entities;

// prep requirement based on PAR levels
public class PrepTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    // how much the station should have
    public int Par { get; set; }
    // how much on hand
    public int Count { get; set; }
    // how much to prep (par - count)
    public int Prep => Math.Max(Par - Count, 0);
    public int? DurationMin { get; set; } = 30;
    public bool IsDone { get; set; }
}
