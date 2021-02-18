using System;
using System.Collections.Generic;
using OdinSerializer;
using Unity.Build;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BuildSettings : SerializedScriptableObject
{
    public const string k_BuildSettingsPath = "Assets/Editor/BuildSettings.asset";
    
    public Dictionary<BuildTarget, BuildConfiguration[]> configsByTarget =
        new Dictionary<BuildTarget, BuildConfiguration[]>
        {
            {BuildTarget.WebGL, new BuildConfiguration[0]},
            {BuildTarget.StandaloneWindows64, new BuildConfiguration[0]},
            {BuildTarget.StandaloneOSX, new BuildConfiguration[0]}
        };

    public static BuildSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<BuildSettings>(k_BuildSettingsPath);
        if (settings == null)
        {
            settings = CreateInstance<BuildSettings>();
            AssetDatabase.CreateAsset(settings, k_BuildSettingsPath);
            AssetDatabase.SaveAssets();
        }

        return settings;
    }

    public static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }

    public void Save()
    {
        Debug.Log("SAVING!");
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}