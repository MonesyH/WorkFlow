namespace WorkFlow;
public class WorkflowBuilder
{
    private readonly IApplicationBuilder _appBuilder;

    public WorkflowBuilder(IApplicationBuilder appBuilder)
    {
        _appBuilder = appBuilder;
    }

    public WorkflowBuilder Start<T>() where T : IWorkflowStep
    {
        _appBuilder.Use(async (context, next) =>
        {
            var step = (T)Activator.CreateInstance(typeof(T))!;
            await step.ExecuteAsync(context);
            await next();
        });

        return this;
    }

    public WorkflowBuilder Then<T>() where T : IWorkflowStep
    {
        _appBuilder.Use(async (context, next) =>
        {
            var step = (T)Activator.CreateInstance(typeof(T))!;
            await step.ExecuteAsync(context);
            await next();
        });

        return this;
    }

    public WorkflowBuilder End()
    {
        return this;
    }
}