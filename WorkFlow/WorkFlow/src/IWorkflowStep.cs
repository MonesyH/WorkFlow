namespace WorkFlow;

public interface IWorkflowStep
{
    Task ExecuteAsync(HttpContext context);
}

public class StartWorkflow : IWorkflowStep
{
    public async Task ExecuteAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Starting workflow...\n");
        Console.WriteLine("Starting workflow...");
    }
}

public class ThenWorkflow : IWorkflowStep
{
    public async Task ExecuteAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Then workflow...\n");
        Console.WriteLine("Then workflow...");
    }
}
