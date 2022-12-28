using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseRole : MonoBehaviour
{
    public bool isDiagonal = false, isStaright = false, isChoosed = true;
    public int[] moveX = new int[] { };
    public int[] moveY = new int[] { };

    public int[] eatX = new int[] { };
    public int[] eatY = new int[] { };

    public virtual void Choose() {
        var o = this.GetComponent<ChessRole>();

        if (!o) return;

        //move
        if (o.role == Role.none) {
            Role _role = o.role;
            bool _isWhite = o.isWhite;
            Sprite spr = o.basicSpr;

            o.Setup(ChessManager.Ins.curChess.role, ChessManager.Ins.curChess.isWhite, o.curPosX, o.curPosY);
            ChessManager.Ins.curChess.Setup(_role, _isWhite, ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY, spr);

            UnChoose(true);
            isChoosed = !isChoosed;
            return;
        }
        //eat
        else if (ChessManager.Ins.curChess && o.role != Role.none && ChessManager.Ins.curChess != o && ChessManager.Ins.curChess.isWhite != o.isWhite) {
            Role _role = Role.none;
            bool _isWhite = o.isWhite;
            Sprite spr = ChessManager.Ins.allChess.Find(x => x.role == Role.none).basicSpr;

            bool isWin = o.role == Role.king;

            o.Setup(ChessManager.Ins.curChess.role, ChessManager.Ins.curChess.isWhite, o.curPosX, o.curPosY);
            ChessManager.Ins.curChess.Setup(_role, _isWhite, ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY, spr);

            if (isWin) GameManager.Ins.SetWin(o.isWhite);

            UnChoose(true);
            isChoosed = !isChoosed;
            return;
        }

        if(ChessManager.Ins.curChess) Debug.LogError((o.role != Role.none) + "*" + (ChessManager.Ins.curChess != o) + "*" + (ChessManager.Ins.curChess.isWhite != o.isWhite) + "*" + (!isChoosed));

        //disable all
        //foreach (var i in ChessManager.Ins.allChess.Where(x => x.role != Role.none))
        //{
        //    i.chessBtn.enabled = (i.curPosX == o.curPosX && i.curPosY == o.curPosY);
        //}

        int multiple = o.isWhite ? 1 : -1;

        if (isChoosed || ChessManager.Ins.curChess != o)
        {
            ChessManager.Ins.curChess = o;

            //clear
            foreach (var i in ChessManager.Ins.allChess.Where(x => x.role == Role.none))
            {
                i.EnableChess(false);
            }

            //basic
            Basic(multiple);

            //move staright
            if(isStaright) MoveStaright(ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY);

            //move diagonal
            if(isDiagonal) MoveDiagonal(ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY);
        }
        else
        {
            UnChoose(false);
        }

        isChoosed = !isChoosed;
    }

    public void UnChoose(bool isDone) {
        if(isDone) ChessManager.Ins.curIsWhite = !ChessManager.Ins.curIsWhite;
        foreach (var i in ChessManager.Ins.allChess)
        {
            i.EnableChess(i.role != Role.none);
        }
        ChessManager.Ins.curChess = null;

        if (GameManager.Ins.isAi && ChessManager.Ins.curIsWhite) AiManager.Ins.GenerateMove();
    }

    public void Basic(int multiple) {
        ChessRole o = ChessManager.Ins.curChess;

        ////move
        for (int i = 0; i < moveX.Length; i++)
        {
            var p = ChessManager.Ins.allChess.Find(x => x.curPosX == o.curPosX + moveX[i] && x.curPosY == o.curPosY + (moveY[i] * multiple) && x.role == Role.none);
            Debug.LogError((o.curPosX + moveX[i]) + "*" + (o.curPosY + (moveY[i] * multiple)));
            if (p)
            {
                p.EnableChess(true);
            }
            else if (o.role == Role.pawn)
            {
                break;
            }
        }

        ////eat
        for (int i = 0; i < eatX.Length; i++)
        {
            var p = ChessManager.Ins.allChess.Find(x => x.curPosX == o.curPosX + eatX[i] && x.curPosY == o.curPosY + (eatY[i] * multiple) && x.role != Role.none);
            Debug.LogError("eat" + (o.curPosX + eatX[i]) + "*" + (o.curPosY + (eatY[i] * multiple)));
            if (p)
            {
                p.EnableChess(true, true);
            }
        }
    }

    public void MoveStaright(int x, int y) {
        int i = x+1;
        ChessRole checkedChess;

        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == y))
        {
            if(checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none) {
                break;
            }
            i++;
        }

        i = x - 1;
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == y))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i--;
        }

        i = y + 1;
        Debug.LogError(i);
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == x && n.curPosY == i))
        {
            Debug.LogError(checkedChess.curPosX +""+ checkedChess.curPosY);
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i++;
        }

        i = y - 1;
        Debug.LogError(i);
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == x && n.curPosY == i))
        {
            Debug.LogError(checkedChess.curPosX + "" + checkedChess.curPosY);
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i--;
        }
    }

    public void MoveDiagonal(int x, int y) {
        int i = x - 1;
        int j = y + 1;
        ChessRole checkedChess;

        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == j))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i--;
            j++;
        }

        i = x + 1;
        j = y + 1;
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == j))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i++;
            j++;
        }

        i = x - 1;
        j = y - 1;
        Debug.LogError(i);
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == j))
        {
            Debug.LogError(checkedChess.curPosX + "" + checkedChess.curPosY);
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i--;
            j--;
        }

        i = x + 1;
        j = y - 1;
        Debug.LogError(i);
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == j))
        {
            Debug.LogError(checkedChess.curPosX + "" + checkedChess.curPosY);
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) checkedChess.EnableChess(true, true);
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i++;
            j--;
        }
    }
}
