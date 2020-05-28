using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardSettings
{
    public int sizeX;
    public int sizeY;
    public float minFallRate;
    public float maxFallRate;
    public Tile tile;
    public List<Color> tileColor;
}

public class GameManager : MonoBehaviour
{
    [Header ("Параметры игровой доски")]
    public BoardSettings settings;

    void Start()
    {
        GameBoard.instance.SetValue(settings.sizeX, settings.sizeY, settings.minFallRate, settings.maxFallRate, settings.tile, settings.tileColor);
    }
}
