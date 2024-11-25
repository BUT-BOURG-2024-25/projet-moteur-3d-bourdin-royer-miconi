using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<GameObject> allEnemies = new List<GameObject>();
    public List<Wave> waves = new List<Wave>();
    public GameObject enemyDefaultPrefab;
    public float spawnRange;
    public List<OrbsDropChances> xpOrbs;
    public int killedEnemies = 0;

    private void Start()
    {

    }

    public void AddEnemyToList(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    public void RemoveEnemyToList(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    public void DropXpOrb(Transform enemy)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);

        for (int i = xpOrbs.Count - 1; i >= 0; i--)
        {
            var orb = xpOrbs[i];

            if (randomValue <= orb.orbChance)
            {
                Instantiate(orb.orbPrefab, enemy.transform.position, Quaternion.identity);
                return;
            }
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        if (enemy == null)
            enemy = enemyDefaultPrefab;

        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * spawnRange;
        float z = Mathf.Sin(angle) * spawnRange;
        Vector3 spawnPosition = new Vector3(x, 0, z) + PlayerManager.Instance.player.transform.position;

        GameObject newEnemy =  Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

    public IEnumerator StartWaves()
    {
        foreach (Wave wave in waves)
        {
            foreach (WavePart part in wave.parts)
            {
                for (int i = 0; i < part.numberOfEnemiesToSpawn; i++)
                {
                    int index = Random.Range(0, part.enemiesToSpawn.Length);

                    SpawnEnemy(part.enemiesToSpawn[index]);

                    yield return new WaitForSeconds(part.cooldownBetweenEachEnemy);
                }
            }
        }
    }

    public override void Reload()
    {
    }
}
