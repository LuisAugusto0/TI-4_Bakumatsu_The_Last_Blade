using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IManagedAction 
{
    //IBaseCharacterAction GetCharacterAction();
    bool IsReady();
    bool Attempt();

    void ForceCancel();
    bool AttemptCancel();
    bool IsActive();
}

[Serializable]
public abstract class BaseManagedAction<TAction> : IManagedAction
where TAction : BaseCharacterAction
{
    public readonly TAction action;
    public BaseManagedAction(TAction action)
    {
        this.action = action;
    }

    public BaseCharacterAction GetCharacterAction() => action;
    public void ForceCancel() {action.ForceCancel();}
    public bool AttemptCancel() {return action.AttemptCancel();}
    public bool IsActive() {return action.IsActive();}

    public abstract bool IsReady();
    public abstract bool Attempt();



}


public interface INonPersistantManagedAction
{}

public interface IPersistantManagedAction
{}








//public interface IManagedCharacterAction
// {
//     bool Attempt();
//     bool IsReady();
//     IBaseCharacterAction GetAction();
// }
//
// public abstract class BaseManagedAction<TAction>
// where TAction : IBaseCharacterAction
// {
//     public readonly TAction action;
//     public BaseManagedAction(TAction action)
//     {
//         this.action = action;
//     }

//     public abstract bool IsReady();
//     public abstract bool Attempt();

// }
