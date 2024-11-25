using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> prefabs;
    private int poolSize = 100;
    public Transform poolParent;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        poolSize = Mathf.CeilToInt(Mathf.Pow((2 * GameManager.Instance.groundGridSize + 1), 2.0f));
        if (poolParent == null)
        {
            GameObject parentObject = new GameObject("ObjectPool");
            poolParent = parentObject.transform;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = InstantiateRandomPrefab();
            obj.SetActive(false);
            obj.transform.SetParent(poolParent);
            pool.Enqueue(obj);
        }
    }

    private GameObject InstantiateRandomPrefab()
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        return Instantiate(prefabs[randomIndex]);
    }

    public GameObject GetObject(float cellSize)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            
            
        }
        else
        {
            obj = InstantiateRandomPrefab();
            obj.transform.SetParent(poolParent);
        }

        obj.SetActive(true);
        obj.transform.localScale = new Vector3(cellSize, cellSize, obj.transform.localScale.y);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        pool.Enqueue(obj);
    }
}

