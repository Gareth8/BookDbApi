using BookDbApi.Shared;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookDbApi.Pipeline
{
    public class ActionErrorHandlingFilter : IAlwaysRunResultFilter, IExceptionFilter
    {
        private SharedError Error { get; set; }

        public ActionErrorHandlingFilter(SharedError error)
        {
            Error = error;
        }

        /// <summary>
        /// This filter is used to catch any errors that occur in actions
        /// It runs automatically after every synchronous action and checks if an error has occurred
        /// </summary>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result != null)
            {
                // If there is an error, output it
                if (Error.Message != null)
                {
                    context.HttpContext.Response.ContentType = "Text/Plain";
                    if (Error.StatusCode != null && context.HttpContext.Response.StatusCode != Error.StatusCode)
                    {
                        context.HttpContext.Response.StatusCode = (int)Error.StatusCode;
                    }
                    context.HttpContext.Response.WriteAsync(Error.Message);
                }
                else if ((context.HttpContext.Response.StatusCode >= 400 && context.HttpContext.Response.StatusCode < 600) || (Error.StatusCode >= 400 && Error.StatusCode < 600))
                {
                    if (Error.StatusCode != null && context.HttpContext.Response.StatusCode != Error.StatusCode)
                    {
                        context.HttpContext.Response.StatusCode = (int)Error.StatusCode;
                    }
                    context.HttpContext.Response.ContentType = "Text/Plain";
                    context.HttpContext.Response.WriteAsync("An unexpected error occurred");
                }

            }
        }

        /// <summary>
        /// This filter is used to catch any errors that occur in the pipeline
        /// It runs after every asynchronous action and checks if an error has occurred
        /// </summary>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            OnResultExecuting(context);
            await next();
        }

        /// <summary>
        /// This method is called after the result has been executed
        /// </summary>
        public void OnResultExecuted(ResultExecutedContext context) { }

        /// <summary>
        /// This method is called when an exception occurs in an action
        /// Don't do this in production code, it's a security risk!
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "Text/Plain";
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync("An exception occurred in " + context.ActionDescriptor.DisplayName + ": (" + context.Exception.Message + ")");
        }
    }
}
