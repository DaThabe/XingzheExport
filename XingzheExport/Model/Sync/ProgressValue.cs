using XingzheExport.Model.Http.Api.V1.Workout;

namespace XingzheExport.Model.Sync;

public class ProgressValue
{
    public int Max { get; set; }

    public int Current { get; set; }


    public Exception? Exception { get; set; }
    public WorkoutDetail? WorkoutDetail { get; set; }
}
