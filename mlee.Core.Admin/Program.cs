/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/


using mlee.Core.Core;
using mlee.Core.Library.Configs;
using mlee.Core.StartUp;

new HostApp(new HostAppOptions
{
    //配置后置服务
    ConfigurePostServices = context =>
    {
        //context.Services.AddTiDb(context);

        //添加任务调度
      /*  context.Services.AddTaskScheduler(DbKeys.AppDb, options =>
        {
            options.ConfigureFreeSql = freeSql =>
            {
                freeSql.CodeFirst
                //配置任务表
                .ConfigEntity<TaskInfo>(a =>
                {
                    a.Name("app_task");
                })
                //配置任务日志表
                .ConfigEntity<TaskLog>(a =>
                {
                    a.Name("app_task_log");
                });
            };

            //模块任务处理器
            options.TaskHandler = new TaskHandler(options.FreeSql);
        });*/

        //context.Services.AddOSS();
    },
    //配置后置中间件
    ConfigurePostMiddleware = context =>
    {
        var app = context.App;
        var env = app.Environment;
        var appConfig = app.Services.GetService<AppConfig>();

        #region 新版Api文档
      /*  if (env.IsDevelopment() || appConfig.ApiUI.Enable)
        {
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                var routePath = options.RoutePrefix.NotNull() ? $"{options.RoutePrefix}/" : "";
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
            });
        }*/
        #endregion
    }
}).Run(args);

#if DEBUG
public partial class Program { }
#endif