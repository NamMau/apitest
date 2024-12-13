using Microsoft.EntityFrameworkCore;
using Quartz;
using webshop.Data;
using webshop.MiddleWare;
using webshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqldbstring")));

// Add services and repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); 

// Add Quartz for job scheduling
//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();
//    var jobKey = new JobKey("VipStatusJob");
//    q.AddJob<VipStatusJob>(opts => opts.WithIdentity(jobKey));
//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("VipStatusJob-trigger")
//        .WithCronSchedule("0 0 * * * ?")); // Every hour
//});
//builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<SecretKeyMiddleware>(); // Secret key middleware

using (var connection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=WINDOWS\\SQLEXPRESS;Initial Catalog=apitestweb;Integrated Security=True;Trust Server Certificate=True"))
{
    connection.Open();
    Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", connection);
    int result = (int)command.ExecuteScalar();
    Console.WriteLine(result);
}
app.MapControllers();

//Function to test API call
static async Task TestApiCall()
{
    using (var client = new HttpClient())
    {
        //add secret key header
        client.DefaultRequestHeaders.Add("secret_key", "123456");

        //replace with your actual API url
        var response = await client.GetAsync("http://localhost:5095/api/v1/orders");

        //check response status and read content
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
