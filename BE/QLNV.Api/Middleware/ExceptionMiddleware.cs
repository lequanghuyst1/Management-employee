
using Microsoft.IdentityModel.Tokens;
using MISA.AMIS.Core.DTOs;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Resources;
using System.Net;

namespace MISA.AMIS.Api.Middleware
{
    public class ExceptionMiddleware
    {
        #region Field
        private readonly RequestDelegate _next;
        #endregion

        #region Constructor
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Xử lý các yêu cầu HTTP 
        /// </summary>
        /// <param name="context">thông tin của yêu cầu và phản hồi</param>
        /// <returns></returns>
        /// Created by: LQHUY(25/12/2023)
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        /// <summary>
        /// Xử lý các Exception xảy ra
        /// </summary>
        /// <param name="context">thông tin của yêu cầu và phản hồi.</param>
        /// <param name="ex">đối tượng ngoại lệ</param>
        /// <returns>
        /// Status code: 404 và thông tin chi tiết về lỗi xảy ra
        /// Status code: 409 và thông tin chi tiết về lỗi xảy ra
        /// Status code: 500 và thông tin chi tiết về lỗi xảy ra
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Console.WriteLine(ex);
            context.Response.ContentType = "apptication/json";
            if(ex is ConnectDbException connectDbException)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = connectDbException.status,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    errors = connectDbException.errors,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink
                }.ToString() ?? ""
                );
            }
            else if (ex is NotFoundException notFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = notFoundException.status,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    errors = notFoundException.errors,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink
                }.ToString() ?? ""
                ); 
            }
            else if (ex is ValidateException validateException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = validateException.status,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    errors = validateException.errors,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink,
                }.ToString() ?? ""
                ); 
            }
            else if(ex is AuthenticationException authenticationException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = authenticationException.status,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    errors = authenticationException.errors,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink,
                }.ToString() ?? ""
                );
            }
            else if(ex is SecurityTokenException securityException)
            {
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = HttpStatusCode.GatewayTimeout,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink,
                }.ToString() ?? ""
                );
            }
            else if (ex is ImportException importException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = importException.status,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    errors = importException.errors,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink,
                }.ToString() ?? ""
                );
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(text: new MISAServiceResult()
                {
                    status = HttpStatusCode.InternalServerError,
                    userMsg = ResourceVN.UserMessage,
                    devMsg = ex.Message,
                    traceId = context.TraceIdentifier,
                    moreInfo = ex.HelpLink
                }.ToString() ?? ""
                );
            } 
            #endregion
        }
    }
}
