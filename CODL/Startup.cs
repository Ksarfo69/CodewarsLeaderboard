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
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();
        
        app.UseSession();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}