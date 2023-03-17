namespace WorkFlow;

public static class WorkflowMiddlewareExtensions
{
    public static IApplicationBuilder UseWorkflow(this IApplicationBuilder appBuilder, Action<WorkflowBuilder> configure)
    {
        var workflowBuilder = new WorkflowBuilder(appBuilder);
        configure(workflowBuilder);
        
        return appBuilder;
    }
}