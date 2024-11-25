using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Collider zone;
    public float minimumSpawnTime; 
    public float maximumSpawnTime;
    public float distanceFromPlayer;
    public GameObject enemyPrefab;
    public bool spawnEnemies;

    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        zone = GetComponent<Collider>();
        player = GameObject.FindWithTag("Player");
        StartCoroutine(spawnEnemy());
    }


    private IEnumerator spawnEnemy()
    {
        while (true)
        {
                Vector3 spawnPosition = GetValidSpawnPosition();

                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }

                float cooldown = UnityEngine.Random.Range(minimumSpawnTime, maximumSpawnTime);
                yield return new WaitForSeconds(cooldown);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        int attempts = 0;
        while (!validPositionFound && attempts < 100)
        {
            float randomX = UnityEngine.Random.Range(zone.bounds.min.x, zone.bounds.max.x);
            float randomZ = UnityEngine.Random.Range(zone.bounds.min.z, zone.bounds.max.z);
            spawnPosition = new Vector3(randomX, zone.transform.position.y, randomZ);

            if (Vector3.Distance(spawnPosition, player.transform.position) >= distanceFromPlayer)
            {
                validPositionFound = true;
            }

            attempts++;
        }
 
        return validPositionFound ? spawnPosition : Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        DrawCircle(distanceFromPlayer, player.transform.position);
    }

    private void DrawCircle(float radius, Vector3 offset)
    {
        int segments = 50;
        float angleStep = 360f / segments;

        Vector3 center = offset;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);

            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
