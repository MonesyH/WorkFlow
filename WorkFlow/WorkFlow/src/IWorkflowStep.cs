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
    }
}

public class ThenWorkflow : IWorkflowStep
{
    public async Task ExecuteAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Then workflow...\n");
    }
}
