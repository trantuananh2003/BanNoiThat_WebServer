using BanNoiThat.API.Extensions;
using BanNoiThat.API.Mapper;
using Azure.Storage.Blobs;
using BanNoiThat.Application.Service.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using BanNoiThat.API.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoApi"));

builder.Services.AddControllers().AddNewtonsoftJson();
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
builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3006") // 🔹 Chỉ cho phép frontend này
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // 🔹 Bật AllowCredentials()
        });
});


var app = builder.Build();
//app.UseCors("AllowAllOrigins");
app.UseCors("AllowFrontend");

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
