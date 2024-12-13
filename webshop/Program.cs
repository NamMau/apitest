using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using webshop.Data;
using webshop.MiddleWare;
using webshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add the secret_key header to Swagger
    c.AddSecurityDefinition("secret_key", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "secret_key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "secret_key"
                },
                Scheme = "oauth2",
                Name = "secret_key",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqldbstring")));

// Add services and repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Quartz for job scheduling (if needed)
// builder.Services.AddQuartz(q =>
// {
//     q.UseMicrosoftDependencyInjectionJobFactory();
//     var jobKey = new JobKey("VipStatusJob");
//     q.AddJob<VipStatusJob>(opts => opts.WithIdentity(jobKey));
//     q.AddTrigger(opts => opts
//         .ForJob(jobKey)
//         .WithIdentity("VipStatusJob-trigger")
//         .WithCronSchedule("0 0 * * * ?")); // Every hour
// });
// builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<SecretKeyMiddleware>(); // Secret key middleware
app.UseAuthorization();

app.MapControllers();

using (var connection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=WINDOWS\\SQLEXPRESS;Initial Catalog=apitestweb;Integrated Security=True;Trust Server Certificate=True"))
{
    connection.Open();
    Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", connection);
    int result = (int)command.ExecuteScalar();
    Console.WriteLine(result);
}

// Function to test API call
static async Task TestApiCall()
{
    using (var client = new HttpClient())
    {
        // Add secret key header
        client.DefaultRequestHeaders.Add("secret_key", "123456");

        // API url
        var response = await client.GetAsync("http://localhost:5095/api/v1/orders");

        // Check response status and read content
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine(data);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
}

app.Run();
