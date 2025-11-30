using CulinaryCommand.Data.Entities;

public class TaskAssignment
{
    public int Id { get; set; }
    public int WorkTaskId { get; set; }
    public WorkTask? WorkTask { get; set; }

    //optional targets
    public int? UserId { get; set; }
    public int? PositionId { get; set; }
    public int? StationId { get; set; }
}