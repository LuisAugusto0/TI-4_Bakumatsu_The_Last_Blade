using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInstantiateSkill : PlayerAction
{
    public GameObject projectile;

    protected override void Perform(int context = 0)
    {
        // Get rotation through input
        GameObject.Instantiate(projectile, player.transform);
    }
}
