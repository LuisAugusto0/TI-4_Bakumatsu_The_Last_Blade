using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : CharacterAction
{
    public float distance;
    protected override void Perform(int context = 0)
    {
        Vector2 teleportVector = character.LastMoveVector.normalized * distance;
        character.Teleport(teleportVector);

        End();
    }
}
