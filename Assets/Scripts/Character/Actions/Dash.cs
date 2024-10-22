using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dash : CharacterAction
{
    public float speed;
    public float duration;

    private float _lastStartTime;
    private Vector2 moveVector = Vector2.zero; 


    protected override void Perform(int context = 0)
    {
        _lastStartTime = Time.time;
        if (character.LastMoveVector == Vector2.zero)
        {
            Vector2 dir = character.spriteRenderer.flipX ? Vector2.left : Vector2.left;
            moveVector = dir * speed;
        }
        else
        {
            moveVector = character.LastMoveVector.normalized * speed;
        }

        character.StartActionLock(Cancel, this);
        character.damageable.AddImmunity(this);

        StartCoroutine(DashRoutine());
    }

    public bool IsActionDone()
    {
        return Time.time > _lastStartTime + duration;
    }

    IEnumerator DashRoutine() 
    {
        while (!IsActionDone()) 
        {
            character.Move(moveVector);
            yield return new WaitForFixedUpdate();
        }
     
        End();
    }

    protected override void End()
    {
        base.End();
        character.EndActionLock(this);
        character.damageable.RemoveImmunity(this);
    }

    void Cancel()
    {
        End();
    }
}
