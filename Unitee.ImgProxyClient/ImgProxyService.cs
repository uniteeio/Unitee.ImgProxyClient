using System;
using System.IO;
using System.Text;

namespace Unitee.ImgProxyClient;

public class ImgProxyService
{
    private readonly string _imgProxyBaseUrl;
    private readonly string? _commonOptions;
    private readonly string? _key;
    private readonly string? _salt;

    public ImgProxyService(string imgProxyBaseUrl, string? key, string? salt, string? commonOptions)
    {
        _imgProxyBaseUrl = imgProxyBaseUrl;
        _commonOptions = commonOptions;
        _key = key;
        _salt = salt;
    }

    public string GetUrl(string originalUrl, ImgProcessingOptions? options = null)
    {
        if (string.IsNullOrEmpty(originalUrl))
        {
            return originalUrl;
        }

        if (options == null)
        {
            options = new ImgProcessingOptions();
        }

        var baseUrl = _imgProxyBaseUrl;
        var b64 = originalUrl.EncodeBase64URLSafeString();

        var processingOptions = new StringBuilder();

        if (!string.IsNullOrEmpty(_commonOptions))
        {
            processingOptions.Append($"{_commonOptions}/");
        }

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

        if (options.AutoRotate.HasValue)
        {
            processingOptions.Append($"auto_rotate:{(options.AutoRotate.Value ? 1 : 0)}/");
        }

        var path = $"{processingOptions}{b64}";

        if (_key is null || _salt is null)
        {
            return $"{baseUrl}/insecure/{path}";
        }

        var signature = SignerHelper.SignPath(_key, _salt, path);

        return $"{baseUrl}/{signature}/{processingOptions}{b64}";
    }
}
