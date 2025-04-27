using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject gridPrefab;
    [SerializeField] Vector2 gridSize;
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            GameObject rowParent = new("Row " + (i + 1));
            for (int j = 0; j < gridSize.y; j++)
            {
                GameObject grid = Instantiate(gridPrefab);
                grid.transform.SetParent(rowParent.transform, false);
                grid.transform.position = new Vector3(1-i, 2-j, 0f);
            }
        }
    }

    private void OnBecameVisible()
    {
        SerializedDictionary<string, string> dick = 
    }
}
