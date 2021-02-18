using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Build;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class BuildConfigSettingProvider : SettingsProvider
{
    private BuildSettings _settings;

    public BuildConfigSettingProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(
        path, scopes, keywords)
    {
    }

    const string k_MyCustomSettingsPath = "Assets/Editor/BuildSettings.asset";

    public static bool IsSettingsAvailable()
    {
        if (File.Exists(k_MyCustomSettingsPath))
            return true;
        else
        {
            BuildSettings.GetSerializedSettings();
            return true;
        }
    }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        _settings = BuildSettings.GetOrCreateSettings();
        foreach (var keyValuePair in _settings.configsByTarget)
        {
            var target = new VisualElement();
            target.Add(new Label(keyValuePair.Key.ToString()));
            var config = new ObjectField("default config")
            {
                objectType = typeof(BuildConfiguration), allowSceneObjects = false,
                value = keyValuePair.Value.FirstOrDefault()
            };

            config.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is BuildConfiguration buildConfiguration)
                {
                    _settings.configsByTarget[keyValuePair.Key] = new[] {buildConfiguration};
                }
            });
            target.Add(config);
            rootElement.Add(target);
        }
    }


    public override void OnDeactivate()
    {
        if (_settings != null)
            _settings.Save();
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
        if (IsSettingsAvailable())
        {
            var provider = new BuildConfigSettingProvider("Project/BuildConfigManager", SettingsScope.Project);

            // Automatically extract all keywords from the Styles.
            //provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
            return provider;
        }

        // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
        return null;
    }
}