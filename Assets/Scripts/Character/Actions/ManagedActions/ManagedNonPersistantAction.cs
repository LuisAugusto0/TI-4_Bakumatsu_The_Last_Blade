using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedNonPersistantAction: BaseManagedAction<NonPersistantAction>
{
    [SerializeField]
    public readonly Character target;

    public BaseCharacterAction GetAction() => action;

    public ManagedNonPersistantAction(Character target, NonPersistantAction action)
    : base(action)
    {
        this.target = target;
    }   
    
    public override bool IsReady() => !target.IsActionLocked;

    public override bool Attempt()
    {
        action.ActionStart();
        
        return true;
    }

}
