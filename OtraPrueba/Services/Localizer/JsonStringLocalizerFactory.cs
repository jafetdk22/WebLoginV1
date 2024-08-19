using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly string _resourcesPath;
    private readonly ConcurrentDictionary<string, JObject> _localizationData;

    public JsonStringLocalizerFactory(IWebHostEnvironment env, ILogger<JsonStringLocalizerFactory> logger, IOptions<LocalizationOptions> localizationOptions)
    {
        _env = env;
        _logger = logger;
        _resourcesPath = localizationOptions.Value.ResourcesPath;
        _localizationData = new ConcurrentDictionary<string, JObject>();
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(_resourcesPath, _env, _logger, _localizationData);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(_resourcesPath, _env, _logger, _localizationData);
    }
}
