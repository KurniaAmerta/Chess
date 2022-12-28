using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BaseRole
{
    private void Start()
    {
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

    public override void Choose()
    {
        base.Choose();
    }
}
