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
    //���ú��÷���
    ConfigurePostServices = context =>
    {
        //context.Services.AddTiDb(context);

        //����������
      /*  context.Services.AddTaskScheduler(DbKeys.AppDb, options =>
        {
            options.ConfigureFreeSql = freeSql =>
            {
                freeSql.CodeFirst
                //���������
                .ConfigEntity<TaskInfo>(a =>
                {
                    a.Name("app_task");
                })
                //����������־��
                .ConfigEntity<TaskLog>(a =>
                {
                    a.Name("app_task_log");
                });
            };

            //ģ����������
            options.TaskHandler = new TaskHandler(options.FreeSql);
        });*/

        //context.Services.AddOSS();
    },
    //���ú����м��
    ConfigurePostMiddleware = context =>
    {
        var app = context.App;
        var env = app.Environment;
        var appConfig = app.Services.GetService<AppConfig>();

        #region �°�Api�ĵ�
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