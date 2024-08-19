using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Collections.Concurrent;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly string _resourcesPath;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, JObject> _localizationData;

    public JsonStringLocalizer(string resourcesPath, IWebHostEnvironment env, ILogger logger, ConcurrentDictionary<string, JObject> localizationData)
    {
        _resourcesPath = resourcesPath;
        _env = env;
        _logger = logger;
        _localizationData = localizationData;
    }

    private JObject GetLocalizationData(string culture)
    {
        if (!_localizationData.TryGetValue(culture, out var data))
        {
            var filePath = Path.Combine(_env.ContentRootPath, _resourcesPath, $"{culture}.json");
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                data = JObject.Parse(jsonData);
                _localizationData.TryAdd(culture, data);
            }
        }
        return data;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var data = GetLocalizationData(culture);
            var value = data?[name]?.ToString();
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments] => this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var data = GetLocalizationData(culture);

        foreach (var item in data)
        {
            yield return new LocalizedString(item.Key, item.Value.ToString());
        }
    }
}
