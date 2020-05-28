using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private class MovedTile
    {
        public GameObject go;
        public Vector3 target;
        public float fallRate;

        public MovedTile(GameObject go, Vector3 target, float fallRate)
        {
            this.go = go;
            this.target = target;
            this.fallRate = fallRate;
        }
    }

    public static GameBoard instance;

    private int sizeX;
    private int sizeY;
    private float minFallRate;
    private float maxFallRate;
    private Tile tile;
    private List<Color> tileColor = new List<Color>();
    private List<MovedTile> movedTiles = new List<MovedTile>();
    private Tile[,] tileArray;
    private Vector3[,] positionMatrix;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(movedTiles.Count > 0)
        {
            for (int i = 0; i < movedTiles.Count; i++)
            {
                if (movedTiles[i].go == null) continue;

                Transform t = movedTiles[i].go.transform;
                t.position = Vector3.Lerp(t.position, movedTiles[i].target, movedTiles[i].fallRate * Time.deltaTime);

                if (Vector3.Distance(t.position, movedTiles[i].target) <= 0.1f)
                {
                    t.position = movedTiles[i].target;
                    movedTiles.Remove(movedTiles[i]);
                }
            }
        }
    }

    public void SetValue(int sizeX, int sizeY, float minFallRate, float maxFallRate, Tile tile, List<Color> tileColor)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.minFallRate = minFallRate;
        this.maxFallRate = maxFallRate;
        this.tile = tile;
        this.tileColor = tileColor;

        tileArray = new Tile[sizeX, sizeY];
        positionMatrix = new Vector3[sizeX, sizeY];

        CreateBoard();
    }

    public void CreateBoard()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 boundsSize = tile.spriteRenderer.bounds.size;
        Vector2 tileSize = new Vector2(boundsSize.x + (boundsSize.x * 2), boundsSize.y + (boundsSize.y * 2));

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Tile newTile = Instantiate(tile, transform.position, Quaternion.identity);
                Vector3 startPos = new Vector3(xPos + (tileSize.x * x), yPos + (tileSize.y * y * 10), 0);
                newTile.transform.position = startPos;
                newTile.transform.SetParent(transform);

                Color rndColor = tileColor[Random.Range(1, tileColor.Count)];
                newTile.color = rndColor;
                newTile.spriteRenderer.color = rndColor;

                newTile.coordY = y;
                newTile.coordX = x;

                Vector3 target = new Vector3(xPos + (tileSize.x * x), yPos + (tileSize.y * y), 0);
                tileArray[x, y] = newTile;
                positionMatrix[x, y] = target;

                float fallRate = Random.Range(minFallRate, maxFallRate);
                MovedTile mt = new MovedTile(newTile.gameObject, target, fallRate);
                movedTiles.Add(mt);
            }
        }
    }

    public void ClearTileIndex(Tile tile)
    {
        tileArray[tile.coordX, tile.coordY] = null;
    }

    List<int> emptyTile_Y_Coordinate = new List<int>();
    public void RefreshBoard()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 boundsSize = tile.spriteRenderer.bounds.size;
        Vector2 tileSize = new Vector2(boundsSize.x + (boundsSize.x * 2), boundsSize.y + (boundsSize.y * 2));

        for (int x = 0; x < sizeX; x++)
        {
            emptyTile_Y_Coordinate.Clear();

            for (int y = 0; y < sizeY; y++)
            {
                if (tileArray[x, y] == null)
                {
                    emptyTile_Y_Coordinate.Add(y);
                }
                else
                {
                    if(emptyTile_Y_Coordinate.Count > 0)
                    {
                        int moveToY = emptyTile_Y_Coordinate[0];
                        Tile movedTile = tileArray[x, y];
                        Vector3 target = positionMatrix[x, moveToY];

                        float fallRate = Random.Range(minFallRate, maxFallRate);
                        MovedTile mt = new MovedTile(movedTile.gameObject, target, fallRate);
                        movedTiles.Add(mt);

                        tileArray[x, moveToY] = movedTile;
                        movedTile.coordX = x;
                        movedTile.coordY = moveToY;

                        tileArray[x, y] = null;
                        emptyTile_Y_Coordinate.Add(y);
                        emptyTile_Y_Coordinate.Remove(moveToY);
                    }
                }
            }

            if (emptyTile_Y_Coordinate.Count > 0)
            {
                for (int i = 0; i < emptyTile_Y_Coordinate.Count; i++)
                {
                    int _y = emptyTile_Y_Coordinate[i];
                    Tile newTile = Instantiate(tile, transform.position, Quaternion.identity);
                    Vector3 startPos = new Vector3(xPos + (tileSize.x * x), yPos + (tileSize.y * _y * 4), 0);
                    newTile.transform.position = startPos;
                    newTile.transform.SetParent(transform);

                    Color rndColor = tileColor[Random.Range(1, tileColor.Count)];
                    newTile.color = rndColor;
                    newTile.spriteRenderer.color = rndColor;

                    newTile.coordY = _y;
                    newTile.coordX = x;
                    tileArray[x, _y] = newTile;

                    Vector3 target = positionMatrix[x, _y];
                    float speed = Random.Range(minFallRate, maxFallRate);
                    MovedTile mt = new MovedTile(newTile.gameObject, target, speed);
                    movedTiles.Add(mt);
                }
            }
        }
    }

    public void CleaarBoard()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Destroy(tileArray[x, y].gameObject);
            }
        }
    }
}
