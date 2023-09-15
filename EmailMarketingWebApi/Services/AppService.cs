using System.Net.Sockets;
using System.Net;

namespace EmailMarketingWebApi.Services
{
    public class AppService
    {

        public IPAddress? GetRemoteHostIpAddress(HttpContext httpContext)
        {
            IPAddress? remoteIpAddress = null;
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim());

                foreach (var ip in ips)
                {
                    if (IPAddress.TryParse(ip, out var address) &&
                        (address.AddressFamily is AddressFamily.InterNetwork
                         or AddressFamily.InterNetworkV6))
                    {
                        remoteIpAddress = address;
                        break;
                    }
                }
            }

            // Check if remoteIpAddress is still null
            if (remoteIpAddress == null)
            {
                remoteIpAddress = httpContext.Connection.RemoteIpAddress;
            }

            return remoteIpAddress;
        }

        public string? GetRemoteHostUserAgent(HttpContext httpContext)
        {
            string? userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault();

            return userAgent;
        }
    }
}
