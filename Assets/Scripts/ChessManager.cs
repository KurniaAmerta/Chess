using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    public static ChessManager Ins { get; private set; }

    [SerializeField] ChessRole chessObj;

    public Sprite[] cheesWhiteSpr, cheesBlackSpr;

    [SerializeField] Transform boardTrf;

    private Queue<ChessRole> chessPool = new Queue<ChessRole>();

    [SerializeField] int[] startRole;

    public List<ChessRole> allChess { get; private set; } = new List<ChessRole>();

    public ChessRole curChess;
    public bool curIsWhite = true;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        Create();
    }

    public void Create() {
        for (int i = 0; i < 64; i++)
        {
            int x = i % 8;
            int y = i / 8;

            if (i < 16) InstantiateObject(i >= 8 ? (Role)5 : (Role)startRole[i], true, x, y);
            else if (i >= 48) InstantiateObject(i < 56 ? (Role)5 : (Role)startRole[i - 56], false, x, y);
            else InstantiateObject(Role.none, false, x, y);
        }
    }

    public void InstantiateObject(Role role, bool isWhite, int x, int y) {
        var obj = Instantiate(chessObj, boardTrf);
        obj.Setup(role, isWhite, x, y);
        chessPool.Enqueue(obj);
        obj.EnableChess(false);
    }

    public void GetObject() {
        while (chessPool.Count>0) { 
            var obj = chessPool.Dequeue();
            allChess.Add(obj);
            obj.EnableChess(obj.role != Role.none);
        }
    }

    public void Restart() {
        for (int i =0; i<boardTrf.childCount; i++) {
            Destroy(boardTrf.GetChild(i).gameObject);
        }
        Create();
        GetObject();
    }
}

public enum Role { 
    king, queen, bishop, knight, rook, pawn, none
}
