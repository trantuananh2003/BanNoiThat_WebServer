using BanNoiThat.API.Extensions;
using BanNoiThat.API.Mapper;
using Azure.Storage.Blobs;
using BanNoiThat.Application.Service.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using BanNoiThat.Application.Service.PaymentMethod.MomoService;
using BanNoiThat.API.Filter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoApi"));
builder.Services.AddControllers(option => {
    option.Filters.Add<CustomExceptionFilter>();
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateProductsCommand).Assembly);
}); 
builder.Services.AddDbContextSQL(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.RegisterDIRepository();
builder.Services.AddApiVersioning(
    option =>
    {
        option.AssumeDefaultVersionWhenUnspecified = true;
        option.DefaultApiVersion = new ApiVersion(1,0);
    });
builder.Services.AddVersionedApiExplorer(
    option =>
    {
        option.GroupNameFormat = "'v'VVV";
        option.SubstituteApiVersionInUrl = true;
    }
);
builder.Services.AddSingleton(u => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));
builder.Services.RegisterDIService();
builder.Services.SetUpAuthorization(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd",
        builder =>
        {
            builder.WithOrigins("http://161.248.146.74")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithExposedHeaders("X-Pagination");

            builder.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("X-Pagination");

            builder.WithOrigins("http://localhost:3005")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .WithExposedHeaders("X-Pagination");

            builder.WithOrigins("http://localhost:3006")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithExposedHeaders("X-Pagination");

            builder.WithOrigins("https://udify.app/")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .WithExposedHeaders("X-Pagination");
        });
});

var app = builder.Build();
app.UseCors("AllowFrontEnd");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();