using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedNonPersistantAction: BaseManagedAction<INonPersistantAction>
{
    [SerializeField]
    public readonly Character target;

    public IBaseCharacterAction GetAction() => action;

    public ManagedNonPersistantAction(Character target, INonPersistantAction action)
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
