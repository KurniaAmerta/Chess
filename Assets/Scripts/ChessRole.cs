using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessRole : MonoBehaviour
{
    public bool isWhite { get; private set; }
    public Role role { get; private set; }
    public int curPosX { get; private set; }
    public int curPosY { get; private set; }

    [SerializeField] Image pawnImg;
    public Button chessBtn;
    public Sprite basicSpr { get; private set; }

    BaseRole bscRole;

    public void Setup(Role _role, bool _isWhite, int posX, int posY, Sprite spr=null) {
        curPosX = posX;
        curPosY = posY;
        
        role = _role;

        isWhite = _isWhite;

        if (bscRole)
        {
            chessBtn.onClick.RemoveAllListeners();
        }

        if (role != Role.none)
        {
            if (isWhite) pawnImg.sprite = ChessManager.Ins.cheesWhiteSpr[(int)role];
            else pawnImg.sprite = ChessManager.Ins.cheesBlackSpr[(int)role];

            
            if(role == Role.pawn && !this.gameObject.GetComponent<Pawn>()) bscRole = this.gameObject.AddComponent<Pawn>();
            else if(role == Role.knight && !this.gameObject.GetComponent<Knight>()) bscRole = this.gameObject.AddComponent<Knight>();
            else if (role == Role.king && !this.gameObject.GetComponent<King>()) bscRole = this.gameObject.AddComponent<King>();
            else if (role == Role.rook && !this.gameObject.GetComponent<Rook>()) bscRole = this.gameObject.AddComponent<Rook>();
            else if (role == Role.bishop && !this.gameObject.GetComponent<Bishop>()) bscRole = this.gameObject.AddComponent<Bishop>();
            else if (role == Role.queen && !this.gameObject.GetComponent<Queen>()) bscRole = this.gameObject.AddComponent<Queen>();

            if (bscRole) chessBtn.onClick.AddListener(bscRole.Choose);
        }
        else {
            if(spr) pawnImg.sprite = spr;

            var o = this.gameObject.GetComponent<None>();
            if (!o) o = this.gameObject.AddComponent<None>();

            bscRole = o;
            chessBtn.onClick.AddListener(o.Choose);
        }

        basicSpr = pawnImg.sprite;
    }

    public void EnableChess(bool isEnable, bool isEat = false) {
        chessBtn.enabled = isEnable && ((isWhite == ChessManager.Ins.curIsWhite) || (role == Role.none && isEnable) || isEat);
        pawnImg.color = new Color(1,1,1, isEnable ? 1 : 0);
    }
}
