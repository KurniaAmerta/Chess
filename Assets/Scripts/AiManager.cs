using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public static AiManager Ins { get; private set; }

    List<AiData> datas = new List<AiData> { };

    private void Awake()
    {
        Ins = this;
    }

    public void GenerateMove() {
        var d = ChessManager.Ins.allChess;

        datas.Clear();

        foreach (var i in ChessManager.Ins.allChess.Where(x => x.isWhite)) {
            ChessManager.Ins.curChess = i;


            if (i.role == Role.rook || i.role == Role.queen) MoveStaright(i.curPosX, i.curPosY);

            if (i.role == Role.bishop || i.role == Role.queen) MoveDiagonal(i.curPosX, i.curPosY);
        
            else Basic(1);
        }

        ShuffleList();

        datas = datas.OrderByDescending(x => x.value).ToList();

        var data = datas[0];

        var o = data.posRole;
        ChessManager.Ins.curChess = data.aiRole;

        Debug.LogError("ai:"+o.role+"*"+ ChessManager.Ins.curChess.role+"*"+data.value);

        //move
        if (o.role == Role.none)
        {
            Role _role = o.role;
            bool _isWhite = o.isWhite;
            Sprite spr = o.basicSpr;

            o.Setup(ChessManager.Ins.curChess.role, ChessManager.Ins.curChess.isWhite, o.curPosX, o.curPosY);
            ChessManager.Ins.curChess.Setup(_role, _isWhite, ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY, spr);
        }
        //eat
        else if (ChessManager.Ins.curChess && o.role != Role.none && ChessManager.Ins.curChess != o && ChessManager.Ins.curChess.isWhite != o.isWhite)
        {
            Role _role = Role.none;
            bool _isWhite = o.isWhite;
            Sprite spr = ChessManager.Ins.allChess.Find(x => x.role == Role.none).basicSpr;

            bool isWin = o.role == Role.king;

            o.Setup(ChessManager.Ins.curChess.role, ChessManager.Ins.curChess.isWhite, o.curPosX, o.curPosY);
            ChessManager.Ins.curChess.Setup(_role, _isWhite, ChessManager.Ins.curChess.curPosX, ChessManager.Ins.curChess.curPosY, spr);

            if (isWin) GameManager.Ins.SetWin(o.isWhite);
        }

        ChessManager.Ins.curIsWhite = !ChessManager.Ins.curIsWhite;
        ChessManager.Ins.curChess = null;

        foreach (var i in ChessManager.Ins.allChess)
        {
            i.EnableChess(i.role != Role.none);
        }
    }

    public void ShuffleList() {
        for (int i = 0; i < datas.Count; i++)
        {
            var temp = datas[i];
            int randomIndex = Random.Range(i, datas.Count);
            datas[i] = datas[randomIndex];
            datas[randomIndex] = temp;
        }
    }

    public void Basic(int multiple)
    {
        ChessRole o = ChessManager.Ins.curChess;

        int[] moveX = new int[] { };
        int[] moveY = new int[] { };

        int[] eatX = new int[] { };
        int[] eatY = new int[] { };

        if (o.role == Role.pawn) {
            moveX = new int[] { 0, 0 };
            moveY = new int[] { 1, 2 };
            eatX = new int[] { -1, 1 };
            eatY = new int[] { 1, 1 };
        } else if (o.role == Role.knight) {
            moveX = new int[] {
                -2, -1, 1, 2,
                -2, -1, 1, 2
            };
            moveY = new int[] {
                1, 2, 2, 1,
                -1, -2, -2, -1
            };
            eatX = new int[] {
                -2, -1, 1, 2,
                -2, -1, 1, 2
            };
            eatY = new int[] {
                1, 2, 2, 1,
                -1, -2, -2, -1
            };
        } else if (o.role == Role.king) {
            moveX = new int[] {
                -1, 0, 1,
                -1,1,
                -1, 0, 1
            };
            moveY = new int[] {
                1, 1, 1,
                0, 0,
                -1, -1, -1
            };
            eatX = new int[] {
                -1, 0, 1,
                -1,1,
                -1, 0, 1
            };
            eatY = new int[] {
                1, 1, 1,
                0, 0,
                -1, -1, -1
            };
        }

        ////move
        for (int i = 0; i < moveX.Length; i++)
        {
            var p = ChessManager.Ins.allChess.Find(x => x.curPosX == o.curPosX + moveX[i] && x.curPosY == o.curPosY + (moveY[i] * multiple) && x.role == Role.none && x.isWhite != o.isWhite);

            if (p)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = p,
                    value = 1
                };

                datas.Add(data);
            }
            else if (o.role == Role.pawn)
            {
                break;
            }
        }

        ////eat
        for (int i = 0; i < eatX.Length; i++)
        {
            var p = ChessManager.Ins.allChess.Find(x => x.curPosX == o.curPosX + eatX[i] && x.curPosY == o.curPosY + (eatY[i] * multiple) && x.role != Role.none && x.isWhite != o.isWhite);
            if (p)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = p,
                    value = EatValue(p.role)
                };

                datas.Add(data);
            }
        }
    }

    public void MoveStaright(int x, int y)
    {
        int i = x + 1;
        ChessRole checkedChess;

        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == y))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none) {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i++;
        }

        i = x - 1;
        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == y))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
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
            Debug.LogError(checkedChess.curPosX + "" + checkedChess.curPosY);
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
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
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i--;
        }
    }

    public void MoveDiagonal(int x, int y)
    {
        int i = x - 1;
        int j = y + 1;
        ChessRole checkedChess;

        while (checkedChess = ChessManager.Ins.allChess.Find(n => n.curPosX == i && n.curPosY == j))
        {
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
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
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
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
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
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
            if (checkedChess.isWhite != ChessManager.Ins.curChess.isWhite || checkedChess.role == Role.none)
            {
                AiData data = new AiData
                {
                    aiRole = ChessManager.Ins.curChess,
                    posRole = checkedChess,
                    value = EatValue(checkedChess.role)
                };
            }
            if (checkedChess.role != Role.none)
            {
                break;
            }
            i++;
            j--;
        }
    }

    public int EatValue(Role role) {
        Debug.LogError("ai eat:"+role);

        if (role == Role.pawn) return 2;
        else if (role == Role.knight) return 3;
        else if (role == Role.bishop) return 3;
        else if (role == Role.rook) return 6;
        else if (role == Role.queen) return 7;
        else if (role == Role.king) return 8;
        else return 1;
    }
}

public struct AiData {
    public ChessRole aiRole, posRole;
    public int value;
}
