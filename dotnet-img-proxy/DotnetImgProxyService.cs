using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DotnetImgProxy
{
    public class ImageProxyService
    {
        private readonly string _imgProxyBaseUrl;

        public ImageProxyService(string imgProxyBaseUrl)
        {
            _imgProxyBaseUrl = imgProxyBaseUrl;
        }

        public string GetUrl(string originalUrl, ImageProxyOptions options = null)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return originalUrl;
            }

            if (options == null)
            {
                options = new ImageProxyOptions();
            }

            var baseUrl = _imgProxyBaseUrl;
            var b64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(originalUrl));

            var processingOptions = new StringBuilder();

            if (options.Width.HasValue && !options.Height.HasValue)
            {
                options.Height = 0; // 0 means: keep the aspect ratio
            }

            if (options.Height.HasValue && !options.Width.HasValue)
            {
                options.Width = 0;
            }

            if (options.Width.HasValue && options.Height.HasValue)
            {
                processingOptions.Append($"resize:{options.ResizeType}:{options.Width}:{options.Height}:{(options.Enlarge ? 1 : 0)}:{(options.Extend ? 1 : 0)}/");

                // default dpr is 2 for retina screen
                processingOptions.Append($"dpr:{options.Dpr}/");
            }

            if (options.Format != null)
            {
                processingOptions.Append($"format:{options.Format}/");
            }

            if (!string.IsNullOrEmpty(options.Extra))
            {
                processingOptions.Append($"{options.Extra}/");
            }

            return $"{baseUrl}/insecure/{processingOptions}{b64}";
        }
    }
}
