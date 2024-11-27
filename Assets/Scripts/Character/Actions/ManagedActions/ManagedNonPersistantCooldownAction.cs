using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#nullable enable

[Serializable]
public class ManagedNonPersistantCooldownAction<TCooldown> 
: BaseManagedAction<NonPersistantAction>, INonPersistantManagedAction
    where TCooldown : ICooldown
{
    [SerializeField]
    public readonly TCooldown cooldown;
    public readonly Character target;


    public ManagedNonPersistantCooldownAction(
        Character target, 
        NonPersistantAction action, 
        TCooldown cooldown
    ) : base(action)
    {
        this.cooldown = cooldown;
        this.target = target;
    }   
    
    public override bool IsReady() => !target.IsActionLocked && cooldown.IsReadyForUse();

    public override bool Attempt()
    {
        Debug.Log("HERE!!");
        bool activated = cooldown.AttemptActivation();
        if (activated)
        {   
            action.ActionStart();
        }
        
        return activated;
    }
}

