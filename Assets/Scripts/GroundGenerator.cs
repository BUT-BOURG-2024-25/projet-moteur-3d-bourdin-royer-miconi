using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public ObjectPool objectPool;
    private GameObject player;
    private int gridRadius = 75;
    public float cellSize = 1.0f;

    private Dictionary<Vector2Int, GameObject> activeCells = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int lastPlayerGridPosition;

    public bool loadingScene = false;

    void Start()
    {
        gridRadius = GameManager.Instance.groundGridSize;
        player = PlayerManager.Instance.player;
        UpdateGrid();
        GameManager.Instance.sceneLoaded += () => loadingScene = false;
        GameManager.Instance.reloadScene += () => loadingScene = true;
    }

    void Update()
    {
        if (loadingScene)
        {

        }
        else
        {
            player = PlayerManager.Instance.player;
            Vector2Int currentPlayerGridPosition = GetGridPosition(player.transform.position);

            if (currentPlayerGridPosition != lastPlayerGridPosition)
            {
                lastPlayerGridPosition = currentPlayerGridPosition;
                UpdateGrid();
            }
        }        
    }

    void UpdateGrid()
    {
        Vector2Int playerGridPosition = GetGridPosition(player.transform.position);

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int z = -gridRadius; z <= gridRadius; z++)
            {
                Vector2Int gridPosition = new Vector2Int(playerGridPosition.x + x, playerGridPosition.y + z);

                if (!activeCells.ContainsKey(gridPosition))
                {
                    Vector3 worldPosition = GetWorldPosition(gridPosition);
                    GameObject cell = objectPool.GetObject(cellSize);
                    cell.transform.position = worldPosition;
                    activeCells.Add(gridPosition, cell);
                }
            }
        }

        List<Vector2Int> cellsToRemove = new List<Vector2Int>();
        foreach (var cell in activeCells)
        {
            if (Vector2Int.Distance(playerGridPosition, cell.Key) > gridRadius)
            {
                cellsToRemove.Add(cell.Key);
            }
        }

        foreach (var gridPosition in cellsToRemove)
        {
            objectPool.ReturnObject(activeCells[gridPosition]);
            activeCells.Remove(gridPosition);
        }
    }

    Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize);
        return new Vector2Int(x, z);
    }

    Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, 0, gridPosition.y * cellSize);
    }
}
