using Microsoft.OpenApi.Models;
using MAI.Backend.Services;

namespace MAI.Backend;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {   
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Report Creator",
                Description = "An ASP.NET Core Web API for creating Reports",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });
        });
        services.AddScoped<IDbService, DbService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddControllers();
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
       

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
    
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}