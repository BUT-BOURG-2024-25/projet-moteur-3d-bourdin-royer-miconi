using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [Header("Metadata")]
    public new string name;
    public Sprite icon;

    [Header("Values")]
    public float cooldown;
    public int level = 0;
    public int maxLevel = 5;
    public float damageMultiplier;
    public float range = 20f;
    public int targetsNumber = 1;
    public GameObject prefab;
    public GameObject castSource;
    public float lifetime = 10f;
    public int currentXp = 0;
    public int xpToLevelUp = 10;

    public GameObject[] GetClosestEnemy(int numberOfTarget)
    {
        var enemiesInRange = EnemyManager.Instance.allEnemies
            .Where(enemy => enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= range)
            .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
            .Take(numberOfTarget)
            .ToArray();

        return enemiesInRange;
    }

    public virtual IEnumerator Cast()
    {
        yield return this;
    }

    public virtual void LevelUp()
    {
        if(level < maxLevel)
        {
            level++;
        }
    }
}