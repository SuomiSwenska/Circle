using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameBtn : MonoBehaviour
{
    public void ResetBoard()
    {
        GameBoard.instance.CleaarBoard();
        GameBoard.instance.CreateBoard();
        GameController.instance.ResetCounter();
    }
}
