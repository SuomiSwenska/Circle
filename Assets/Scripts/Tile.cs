using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public int coordY;
    public int coordX;
    public Color color;
    public SpriteRenderer spriteRenderer;


    public void OnPointerDown(PointerEventData eventData)
    {
        GameController.instance.DownFinger(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameController.instance.UpFinger(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.instance.EnterFinger(this);
    }
}
