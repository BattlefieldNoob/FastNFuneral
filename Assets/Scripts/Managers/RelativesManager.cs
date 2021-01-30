using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RelativesManager : Singleton<RelativesManager>
{

    [SerializeField] private GameObject[] RelativesSpawnPoints;

    [SerializeField] private int spawnPointsNumber = 4;

    [SerializeField] private GameObject[] relatives;
    
    [SerializeField] private int maxRelativesPerGroup = 3;

    [SerializeField] private float distance = 1;

    private int _minRelativesPerGroup = 2;
    void Start()
    {
        if (maxRelativesPerGroup < _minRelativesPerGroup)
            maxRelativesPerGroup = _minRelativesPerGroup;
        SpawnRelativesGroups();   
        if(spawnPointsNumber>= RelativesSpawnPoints.Length)
            Application.Quit();
    }



    private void SpawnRelativesGroups()
    {
        var randomSort = RelativesSpawnPoints.OrderBy(a => Guid.NewGuid()).ToList();
        for (var i = 0; i < spawnPointsNumber; i++)
        {
            var spawnPoint = randomSort[i];
            SpawnRelativeGroup(spawnPoint.transform);
        }
    }

    private void SpawnRelativeGroup(Transform spawnCenter)
    {
        spawnCenter.rotation=Quaternion.Euler(Vector3.Scale(Random.insideUnitSphere,new Vector3(0,360,0)));
        var center = spawnCenter.position;
        var relativesNumber = Random.Range(_minRelativesPerGroup, maxRelativesPerGroup + 1);
        for (var i = 0; i < relativesNumber; i++)
        {
            var obj = CreateRelative();
            obj.transform.SetParent(spawnCenter);
            obj.transform.localPosition = CalculateCirclePosition(i, relativesNumber);
            obj.transform.LookAt(center);
        }
    }

    private GameObject CreateRelative()
    {
        var index = Random.Range(0, relatives.Length);
        var obj = Instantiate(relatives[index]);
        return obj;
    }

    private Vector3 CalculateCirclePosition(int index, int total)
    {
        var unit = 360 / total;
        var angle = unit * index;
        return new Vector3(distance * Mathf.Cos(angle*Mathf.Deg2Rad), 0, distance * Mathf.Sin(angle*Mathf.Deg2Rad));
    }
}
