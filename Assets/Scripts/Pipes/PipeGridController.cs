using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PipeGridController : MonoBehaviour
{


    [SerializeField]
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    [SerializeField]
    private int height = 10;
    [SerializeField]
    private int width = -10;

    [SerializeField]
    private int startX = 0;
    [SerializeField]
    private int startY = 0;

    [SerializeField]
    private Tilemap tilemap;

    private void Start()
    {
        
    }

    [ContextMenu("—генерировать поле")]
    private void GenerateGrid()
    {
        for (int i = startX; i < height; i++)
        {
            for (int j = startY; j < width; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tiles["All"]);
            }
        }
    }
    [ContextMenu("ќчистить поле")]
    private void ClearGrid()
    {
        tilemap.ClearAllTiles();
    }
}
