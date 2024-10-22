using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class RandomCubeGenerator : MonoBehaviour
{
    public GameObject block;
    public int numberOfCubes = 10;
    public float planeSize = 10f;

    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        GenerateCubes();
    }

    void GenerateCubes()
    {
        int gridSize = (int)planeSize;
        List<Vector3> possiblePositions = new List<Vector3>();

        
        for (int x = -gridSize / 2; x < gridSize / 2; x++)
        {
            for (int z = -gridSize / 2; z < gridSize / 2; z++)
            {
                Vector3 position = new Vector3(x + 0.5f, 0.5f, z + 0.5f);
                possiblePositions.Add(position);
            }
        }

        
        Shuffle(possiblePositions);

        
        for (int i = 0; i < numberOfCubes; i++)
        {
            if (i >= possiblePositions.Count)
            {
                Debug.LogWarning("Nie ma wystarczaj¹cej liczby unikalnych pozycji dla wszystkich bloków.");
                break;
            }

            Vector3 cubePosition = possiblePositions[i];
            Instantiate(block, cubePosition, Quaternion.identity);
        }
    }

    
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
