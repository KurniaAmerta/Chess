using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : BaseRole
{
    private void Start()
    {
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
    }

    public override void Choose()
    {
        base.Choose();
    }
}
