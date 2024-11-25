using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


[System.Serializable]
public class Wave
{
    public List<WavePart> parts;
}

[System.Serializable]
public class WavePart
{
    [SerializeField]
    public GameObject[] enemiesToSpawn;
    public int numberOfEnemiesToSpawn;
    public float cooldownBetweenEachEnemy;
}

[System.Serializable]
public class PowerUpgradeButton
{
    public Action callback;
    public string text;
}

[System.Serializable]
public class OrbsDropChances
{
    public GameObject orbPrefab;

    [Range(0, 100)]
    public float orbChance;
}