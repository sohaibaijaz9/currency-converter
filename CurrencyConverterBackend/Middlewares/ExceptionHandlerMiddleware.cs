using CurrencyConverterBackend.Models;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
using VehicleRegistrationTransaction.Application.Common.Exceptions;

namespace CurrencyConverterBackend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (Exception error)
            {

                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>()
                {
                    Success = false,
                    Message = error?.Message
                };
                switch (error)
                {
                    case BadRequestException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;

                        break;

                    case ValidationException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ValidationErrors = ex.Errors;
                        break;

                    case NotFoundException ex:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case AuthenticationException ex:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "Something went wrong, Please try again " + "\n  " + error.Message;
                        break;
                }
                var result = JsonConvert.SerializeObject(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
