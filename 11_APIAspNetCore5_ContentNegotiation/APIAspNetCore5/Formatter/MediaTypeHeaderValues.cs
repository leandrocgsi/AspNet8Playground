using Microsoft.Net.Http.Headers;

namespace APIAspNetCore5.Formatter
{
    public class MediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationYaml = MediaTypeHeaderValue.Parse("application/x-yaml").CopyAsReadOnly();
    }
}