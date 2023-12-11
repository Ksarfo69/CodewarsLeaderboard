using CODL.Data;
using CODL.Exceptions;
using CODL.Repositories;
using CODL.Services;
using Microsoft.EntityFrameworkCore;

namespace CODL;

public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // Add session support
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); 
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; 
        });
        
        // Add logging
        services.AddLogging();
        
        // Add controllers
        services.AddControllers();
        
        // Add business services
        services.AddTransient<IAppUserService, AppUserService>();
        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<ILeaderboardService, LeaderboardService>();
        
        // Add repos
        services.AddTransient<IAppUserRepository, AppUserRepository>();
        services.AddTransient<ICodewarsUserRepository, CodewarsUserRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        
        // Add db
        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("codl_db"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dbContext)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            dbContext.Database.EnsureCreated();
        }

        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();
        
        app.UseSession();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}