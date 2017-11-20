
namespace Procons.Durrah.Api.Handlers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class SessionIdHandler : DelegatingHandler
    {
        public static string SessionIdToken = "ASP.NET_SessionId";

        //async protected override Task<HttpResponseMessage> SendAsync(
        //    HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    string sessionId = string.Empty;

        //    // Try to get the session ID from the request; otherwise create a new ID.
        //    var cookie = request.Headers.GetCookies(SessionIdToken).FirstOrDefault();

        //    if (cookie != null)
        //    {
        //        sessionId = cookie[SessionIdToken].Value;
        //        request.Properties[SessionIdToken] = sessionId;
        //    }




        //    HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        //    if (cookie != null)
        //    {
        //        response.Headers.AddCookies(new CookieHeaderValue[] {
        //    new CookieHeaderValue(SessionIdToken, sessionId)

        //});
        //    }
        //    return response;
        //}
    }


    //public class SessionIdHandler : DelegatingHandler
    //{
    //    public static string SessionIdToken = "ASP.NET_SessionId";

    //    async protected override Task<HttpResponseMessage> SendAsync(
    //        HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        string sessionId;

    //        // Try to get the session ID from the request; otherwise create a new ID.
    //        var cookie = request.Headers.GetCookies(SessionIdToken).FirstOrDefault();

    //        if (cookie == null)
    //        {
    //            sessionId = Guid.NewGuid().ToString();
    //        }
    //        else
    //        {
    //            sessionId = cookie[SessionIdToken].Value;
    //            try
    //            {
    //                Guid guid = Guid.Parse(sessionId);
    //            }
    //            catch (FormatException)
    //            {
    //                // Bad session ID. Create a new one.
    //                sessionId = Guid.NewGuid().ToString();
    //            }
    //        }

    //        request.Properties[SessionIdToken] = sessionId;

    //        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

    //        response.Headers.AddCookies(new CookieHeaderValue[] {
    //        new CookieHeaderValue(SessionIdToken, sessionId)
    //    });

    //        return response;
    //    }
    //}
}