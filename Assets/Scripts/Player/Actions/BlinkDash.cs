using System;
using UnityEngine;

public class BlinkDash : PlayerDashBase
{
    public float distance = 100f;


    protected override void Perform(int context = 0)
    {
        player.InstantMove(GetMoveVector(), distance);
    }
}