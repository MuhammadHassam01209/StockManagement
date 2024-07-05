namespace OcelotApiGateway
{
    public class AccessTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _accessToken;

        public AccessTokenMiddleware(RequestDelegate next, string accessToken)
        {
            _next = next;
            _accessToken = accessToken;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.Add("Authorization", $"Bearer {_accessToken}");
            await _next(context);
        }
    }
}
