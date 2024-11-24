using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#nullable enable

[Serializable]
public class ManagedPersistantCooldownAction<TCooldown> 
: BaseManagedAction<IPersistantAction>, IPersistantManagedAction
    where TCooldown : ICooldown
{
    public readonly TCooldown cooldown;
    public readonly Character target;


    public ManagedPersistantCooldownAction(
        Character target, 
        IPersistantAction action, 
        TCooldown cooldown
    ) : base(action)
    {
        this.cooldown = cooldown;
        this.target = target;
    }   

    public override bool IsReady() => !target.IsActionLocked && cooldown.IsReadyForUse();

    public override bool Attempt()
    {
        if (cooldown.AttemptActivation())
        {   
            if (action.IsActive())
            {
                bool cancelled = action.AttemptCancel();
                if (!cancelled) return false;
            }

            action.ActionStart();
        }
        
        return false;
    }
    

}