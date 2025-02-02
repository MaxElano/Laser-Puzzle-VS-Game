using System.Drawing;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;

    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private LaserTile laserTilePrefab;
    [SerializeField] private Transform cameraTransform;
    private Tile[,] tiles;
    private Tile holdingTile;

    static public GridManager GetInstance()
    {
        if (instance)
            return instance;
         
        instance = new GridManager();
        return instance;
    }

    private void Start()
    {
        GenerateEmptyGrid();
    }

    void GenerateEmptyGrid()
    {
        tiles = new Tile[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(i,j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[i,j] = spawnedTile;
            }

        cameraTransform.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, cameraTransform.transform.position.z);
    }
    void GenerateLevelGrid()
    {
        tiles = new Tile[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[i, j] = spawnedTile;
            }

        var spawnedTile2 = Instantiate(laserTilePrefab, new Vector3(0, 0), Quaternion.identity);
        spawnedTile2.name = $"LaserTile {0} {0}";

        var isOffset2 = (0 % 2 == 0 && 0 % 2 != 0) || (0 % 2 != 0 && 0 % 2 == 0);
        spawnedTile2.Init(isOffset2);

        tiles[0, 0] = spawnedTile2;

        cameraTransform.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, cameraTransform.transform.position.z);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        Point actualPosition = new Point(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        
        if (actualPosition.X >= 0 && actualPosition.X < width && actualPosition.Y >= 0 && actualPosition.Y < height)
            return tiles[actualPosition.X, actualPosition.Y];
        else
            return null;
    }

    public void ShootLaser()
    {

    }
}
