using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;
    private Grid grid;
    private float nodeDiameter;

    void Start()
    {
        grid = FindObjectOfType<Grid>();

        if (grid == null)
        {
            Debug.LogError("Nie znaleziono obiektu Grid!");
            return;
        }
        else
        {
            Debug.Log("Grid znaleziony.");
        }

        nodeDiameter = grid.nodeRadius * 2;
        SpawnFruit();
    }

    public void SpawnFruit()
    {
        if (grid.grid == null)
        {
            Debug.LogError("Grid nie zosta³ zainicjalizowany!");
            return;
        }

        List<Node> availableNodes = new List<Node>();

        foreach (Node node in grid.grid)
        {
            if (node.walkable)
            {
                availableNodes.Add(node);
            }
        }

        foreach (Node node in grid.grid)
        {
            if (node.walkable)
            {
                availableNodes.Add(node);
            }
        }

        SnakeHead snake = FindObjectOfType<SnakeHead>();
        foreach (Transform segment in snake.segments)
        {
            Node node = grid.NodeFromWorldPoint(segment.position);
            if (availableNodes.Contains(node))
            {
                availableNodes.Remove(node);
            }
        }

        if (availableNodes.Count == 0)
        {
            Debug.Log("Brak dostêpnych miejsc na owoc.");
            return;
        }

        Node randomNode = availableNodes[Random.Range(0, availableNodes.Count)];

        Vector3 fruitPosition = randomNode.worldPosition;
        fruitPosition.y = 0.5f;

        Instantiate(fruitPrefab, fruitPosition, Quaternion.identity);
    }
}
