namespace Procons.Durrah.Api.Attributes
{
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Web.Http.ModelBinding;

    public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //TODO: ADD EXCEPTION LOGGER.
        }
    }
}