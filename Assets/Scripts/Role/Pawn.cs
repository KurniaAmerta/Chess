using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : BaseRole
{
    private void Start()
    {
        moveX = new int[] { 0, 0 };
        moveY = new int[] { 1, 2 };
        eatX = new int[] { -1, 1 };
        eatY = new int[] { 1, 1 };
    }

    public override void Choose()
    {
        base.Choose();
    }
}
