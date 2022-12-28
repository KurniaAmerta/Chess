using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    const string DATA = "DATA";
    string data;

    [SerializeField] Button resumeBtn;

    private void Awake()
    {
        data = PlayerPrefs.GetString(DATA);
    }

    private void Start()
    {
        resumeBtn.gameObject.SetActive(!string.IsNullOrEmpty(data));
    }

    public void Resume() { 
        
    }

    public void Player() {
        GameManager.Ins.SetChess();
    }

    public void Ai() {
        GameManager.Ins.SetChess();
        GameManager.Ins.isAi = true;
        AiManager.Ins.GenerateMove();
    }

    public void Exit() {
        Application.Quit();
    }
}
