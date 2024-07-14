using CurrencyConverterBackend.Commands;
using CurrencyConverterBackend.Commands.CurrencyConversion;
using CurrencyConverterBackend.Extensions;
using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Queries;
using CurrencyConverterBackend.Queries.HistoricalRates;
using CurrencyConverterBackend.Queries.LatestExchangeRates;
using CurrencyConverterBackend.Utilities;
using FluentValidation;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

//Add validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//API service client
builder.Services.AddScoped<ApiServiceClient>(provider => new ApiServiceClient(builder.Configuration.GetSection("ExternalApi").GetValue<string>("BaseUrl")));

//Commands
builder.Services.AddScoped<ICommandHandler<CurrencyConversionCommand, Response<ConversionResponse>>, CurrencyConversionCommandHandler>();

//Queries
builder.Services.AddScoped<IQueryHandler<LatestExchangeRatesQuery, Response<ExchangeRateResponse>>, LatestExchangeRatesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<HistoricalRatesQuery, Response<HistoricalRatesResponse>>, HistoricalRatesQueryHandler>();

//Validators
builder.Services.AddTransient<IValidator<CurrencyConversionCommand>, CurrencyConversionCommandValidator>();
builder.Services.AddTransient<IValidator<LatestExchangeRatesQuery>, LatestExchangeRatesQueryValidator>();
builder.Services.AddTransient<IValidator<HistoricalRatesQuery>, HistoricalRatesQueryValidator>();

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency Converter", Version = "v1" });
});



builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseSwaggerExtension();

app.UseAuthorization();

app.MapControllers();

app.Run();
