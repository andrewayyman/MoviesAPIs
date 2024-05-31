
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Db Connection 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddTransient<IGenresService, GenresService>();
builder.Services.AddTransient<IMoviesService, MoviesService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movies Api",
        Description = "Crud Project",
        TermsOfService = new Uri("https://www.google.com"),
        Contact = new OpenApiContact
        {
            Name = "Andrew",
            Email = "Andrewayman1000@gmail.com",
            Url = new Uri("https://github.com/andrewayyman"),
        }
    });


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Auth",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "45ll 3lshan t3de y habeb a5ok"
    }) ;

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
               Name = "Bearer",
               In = ParameterLocation.Header

            },
            new List<string>()
        }
    });
     
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); // to control how can access my api from frontend pov
app.UseAuthorization();
app.MapControllers();

app.Run();
