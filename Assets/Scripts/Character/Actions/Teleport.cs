using UnityEngine;
using UnityEngine.Events;
using System;
using System.Runtime.CompilerServices;


public class Teleport : CharacterCooldownAction
{
    [Serializable]
    public class OnFiredEvent : UnityEvent<Teleport>
    { }

    [Tooltip("Event triggered when the skill is fired.")]
    public OnFiredEvent fired;

    public float distance;
    protected override void Perform(int context = 0)
    {
        Vector2 teleportVector = character.LastMoveVector.normalized * distance;
        character.Teleport(teleportVector);

        fired.Invoke(this);
        End();
    }
}
