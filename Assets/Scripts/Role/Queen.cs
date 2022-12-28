using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : BaseRole
{
    private void Start()
    {
        isDiagonal = isStaright = true;
    }

    public override void Choose()
    {
        base.Choose();
    }
}
