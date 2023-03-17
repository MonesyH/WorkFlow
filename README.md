# 一、思路
假设要把工作流设计成：开始、执行、结束三个阶段，在不同阶段实现不同逻辑。

如下，StartWorkflow和ThenWorkflow是自定义逻辑：
```
var builder = new WorkflowBuilder()
  .Start<StartWorkflow>()
  .Then<ThenWorkflow>()
  .End<>();
```

因此需要一个WorkflowBuilder来接收不同阶段的自定义逻辑，同时自定义的都要实现IWorkflowStep接口的ExecuteAsync，并且把自定义逻辑都放ExecuteAsync里面。

# 二、具体实现
## 1. IWorkflowStep接口
```
public interface IWorkflowStep
{
    Task ExecuteAsync(HttpContext context);
}
```
## 2. WorkflowBuilder
```
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
```
## 3. 自定义start、then类实现IWorkflowStep接口
```
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
```
## 4. 写一个扩展类，以便可以把WorkflowBuiler的方法在startup里面使用
```
public static class WorkflowMiddlewareExtensions
{
    public static IApplicationBuilder UseWorkflow(this IApplicationBuilder appBuilder, Action<WorkflowBuilder> configure)
    {
        var workflowBuilder = new WorkflowBuilder(appBuilder);
        configure(workflowBuilder);
        
        return appBuilder;
    }
}
```
## 5. startup中的confugire方法中实现workflow
```
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseWorkflow(builder =>
        {
            builder.Start<StartWorkflow>()
                .Then<ThenWorkflow>()
                .End();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("End Point");
            });
        });
    }
```

# 三、效果
![效果图](https://upload-images.jianshu.io/upload_images/20387877-84075b57d66ff32c.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)
