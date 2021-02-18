using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildConfigsManager : MonoBehaviour
{
    [MenuItem(itemName: "BuildManager/Build Current Platform")]
    public static void BuildCurrentPlatform()
    {
        Debug.Log("///////////////////////////////////////////////////////////////////");
        var currentTarget = EditorUserBuildSettings.activeBuildTarget;
        Debug.Log("building:" + currentTarget.ToString());
        var result = BuildByTarget(currentTarget);
        Debug.Log("result:" + result);
    }

    private static bool BuildByTarget(BuildTarget target)
    {
        Debug.Log("before GetSettings");
        var config = BuildSettings.GetOrCreateSettings();
        Debug.Log("after GetSettings");
        if (!config.configsByTarget.ContainsKey(target))
            return false;
        Debug.Log("CONTAINS!");
        var buildConfigs = config.configsByTarget[target];
        Debug.Log("building:" + buildConfigs);
        if (buildConfigs.Length == 0)
            return false;
        foreach (var buildConfig in buildConfigs)
        {
            Debug.Log("FORRRRRRRRRRRRRRRRRR");
            var result = buildConfig.Build();
            Debug.Log("FOR RESULT"+result);
        }

        return true;
    }


    [InitializeOnLoadMethod]
    private static void OnCompileEnd()
    {
        if (Application.isBatchMode && !EditorApplication.isCompiling)
        {
            //BuildCurrentPlatform();
        }
        else
        {
            Debug.Log("NO BATCH MODE");
        }
    }
}