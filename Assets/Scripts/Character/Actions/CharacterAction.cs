using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public delegate void OnActionEnded();

public interface IBaseCharacterAction
{
    void ActionStart(); 

    // Persistant implementations: For non persistant, always do nothing / return false
    void ForceCancel();
    bool AttemptCancel(); //Cancel Blocked => false
    bool IsActive();
}

// Expects no cooldown to relay on finished
public interface INonPersistantAction : IBaseCharacterAction
{}

// Expects to relay on finished and to not be called until then
public interface IPersistantAction : IBaseCharacterAction
{
    OnActionEnded GetOnActionEnded();
    
}







// public class EntityActionHandler<TAction> : MonoBehaviour
// where TAction : ICharacterAction
// {
//     public TAction action;
// }

