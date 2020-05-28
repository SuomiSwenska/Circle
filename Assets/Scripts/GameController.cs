using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private Text countText;
    private int count = 0;

    [SerializeField] private GameObject bangEffect;
    [SerializeField] private LineRenderer lineRndPref;
    [SerializeField] private Tile startTile;

    private LineRenderer freeLineRnd;
    private LineRenderer lineRnd;

    private List<Tile> tiles = new List<Tile>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private bool haveSelected = false;

    private void Awake()
    {
        instance = this;
        LoadCounter();
    }

    private void Update()
    {
        if(haveSelected && startTile != null)
        {
            ShowFreeLine();
        }
    }

    public void DownFinger(Tile tile)
    {
        if (tile == null || tile == startTile) return;

        startTile = tile;
        tiles.Add(startTile);
        haveSelected = true;

        GetNewFreeLineRenderer();
    }

    List<GameObject> tilesGO = new List<GameObject>();
    public void UpFinger(Tile tile)
    {
        if (lines.Count > 0)
        {
            foreach (var item in lines)
            {
                Destroy(item.gameObject);
            }
        }

        if (tiles.Count > 1)
        {
            foreach (var item in tiles)
            {
                GameObject go = Instantiate(bangEffect, item.transform.position, Quaternion.identity, transform);
                tilesGO.Add(go);
                Destroy(item.gameObject);
                GameBoard.instance.ClearTileIndex(item);
            }

            ChangeCount(tiles.Count);
            SaveCounter();
            Invoke("DestroyTile", 0.7f);
        }

        Reset();
    }

    public void EnterFinger(Tile tile)
    {
        if (startTile == null || tile == startTile) return;

        if (tiles.Count > 1 && tile == tiles[tiles.Count -2])
        {
            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);
            tiles.RemoveAt(tiles.Count - 1);

            startTile = tile;
            return;
        }

        float coordX_1 = startTile.coordX;
        float coordY_1 = startTile.coordY;
        float coordX_2 = tile.coordX;
        float coordY_2 = tile.coordY;

        if (startTile.color == tile.color && (coordX_1 == coordX_2 && Mathf.Abs(coordY_1 - coordY_2) == 1 || coordY_1 == coordY_2 && Mathf.Abs(coordX_1 - coordX_2) == 1))
        {
            AddNewLine(startTile, tile);
            GetNewFreeLineRenderer();
        }
    }

    private void AddNewLine(Tile startTile, Tile endTile)
    {
        lineRnd = Instantiate(lineRndPref, transform);
        lineRnd.gameObject.name = "Line_" + lines.Count;
        Vector3[] positions = { startTile.transform.position, endTile.transform.position };
        lineRnd.SetColors(startTile.color, startTile.color);
        lineRnd.SetPositions(positions);

        lines.Add(lineRnd);
        tiles.Add(endTile);

        this.startTile = endTile;
        Destroy(freeLineRnd.gameObject);
    }

    private void ShowFreeLine()
    {
        Vector3 targetPosition = Input.mousePosition;
        targetPosition.z = 10.0f;
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);

        Vector3[] positions = { startTile.transform.position, targetPosition };

        freeLineRnd.SetColors(startTile.color, startTile.color);
        freeLineRnd.SetPositions(positions);
    }

    private void GetNewFreeLineRenderer()
    {
        if (freeLineRnd != null)
        {
            Destroy(freeLineRnd.gameObject);
        }
        freeLineRnd = Instantiate(lineRndPref, transform);
        freeLineRnd.gameObject.name = "FreeLineRnd";
    }

    private void Reset()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].gameObject);
        }
        lines.Clear();
        tiles.Clear();

        haveSelected = false;
        startTile = null;
        Destroy(freeLineRnd.gameObject);
    }

    private void DestroyTile()
    {
        for (int i = 0; i < tilesGO.Count; i++)
        {
            Destroy(tilesGO[i].gameObject);
        }
        GameBoard.instance.RefreshBoard();
    }

    public void ResetCounter()
    {
        ChangeCount(count * -1);
        SaveCounter();
    }

    private void ChangeCount(int number)
    {
        count += number;
        countText.text = count.ToString();
    }

    private void SaveCounter()
    {
        PlayerPrefs.SetInt("Count", count);
    }

    private void LoadCounter()
    {
        ChangeCount(PlayerPrefs.GetInt("Count"));
    }
}
