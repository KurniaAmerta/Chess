using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Ins { get; private set; }

    [SerializeField] Text winTxt;
    [SerializeField] GameObject winObj;

    public bool isWin = false, isAi = false;

    private void Awake()
    {
        Ins = this;
    }

    public void SetChess(string data = "") {
        ChessManager.Ins.GetObject();
    }

    public void SetWin(bool isWhite) {
        winObj.SetActive(true);
        winTxt.text = isWhite ? "White Win" : "Black Win";
    }
}
