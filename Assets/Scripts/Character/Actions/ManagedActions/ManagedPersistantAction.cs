using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
#nullable enable


[Serializable]
public class ManagedPersistantAction 
: BaseManagedAction<IPersistantAction>, IPersistantManagedAction
{
    public readonly Character target;

    public ManagedPersistantAction(
        Character target, 
        IPersistantAction action
    ) : base(action)
    {
        this.target = target;
    }   

    
    public override bool IsReady() => !target.IsActionLocked; 

    public override bool Attempt()
    {
        if (action.IsActive())
        {
            bool cancelled = action.AttemptCancel();
            if (!cancelled) return false;
        }

        action.ActionStart();
      
        
        return true;
    }

}

