using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEditor;
using UnityEngine;

public class RelativesManager : Singleton<RelativesManager>
{

    [SerializeField] private GameObject[] RelativesSpawnPoints;

    [SerializeField] private int RelativesPerGroup = 3;
    
    void Start()
    {
        SpawnRelativesGroups();   
    }



    private void SpawnRelativesGroups()
    {
        foreach (var spawnPoint in RelativesSpawnPoints)
        {
            
            SpawnRelativeGroup(spawnPoint.transform);
        }
    }

    private void SpawnRelativeGroup(Transform spawnCenter)
    {
        for (int i = 0; i < RelativesPerGroup; i++)
        {
            var obj = CreateRelative();
            obj.transform.SetParent(spawnCenter);
            obj.transform.localPosition = CalculateCirclePosition(i, RelativesPerGroup) + Vector3.up;
            obj.transform.LookAt(Vector3.up);
        }
        spawnCenter.rotation=Quaternion.Euler(Vector3.Scale(Random.insideUnitSphere,new Vector3(0,360,0)));
    }

    private GameObject CreateRelative()
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        return obj;
    }

    private Vector3 CalculateCirclePosition(int index, int total)
    {
        var unit = 360 / total;
        var angle = unit * index;
        return new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad), 0, Mathf.Sin(angle*Mathf.Deg2Rad));
    }
}
