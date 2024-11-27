using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#nullable enable

public delegate void OnActionEnded();

public abstract class BaseCharacterAction : MonoBehaviour
{
    // Action needs to be initialized to contain entity data
    // Cannot be initialized twice as it can only be tied to a single managed action
    public abstract bool IsInitialized();

    public abstract void ActionStart(); 

    // Persistant implementations: For non persistant, always do nothing / return false
    public abstract void ForceCancel();
    public abstract bool AttemptCancel(); //Cancel Blocked => false
    public abstract bool IsActive();
    
}

// Expects no cooldown to relay on finished
public abstract class NonPersistantAction : BaseCharacterAction
{ }

public abstract class GenericNonPersistantAction<T> : NonPersistantAction
where T : MonoBehaviour
{
    public abstract void Initialize(T target);
}

public abstract class CharacterNonPersistantAction : GenericNonPersistantAction<Character>
{}

public abstract class PlayerNonPersistantAction : GenericNonPersistantAction<BasePlayerBehaviour>
{}

public abstract class RoninPlayerNonPersistantAction : GenericNonPersistantAction<RoninPlayerBehaviour>
{}



// Expects to relay on finished and to not be called until then
public abstract class PersistantAction : BaseCharacterAction
{
    public abstract OnActionEnded GetOnActionEnded();
}

public abstract class GenericPersistantAction<T> : PersistantAction
where T : MonoBehaviour
{
    public abstract void Initialize(T target, OnActionEnded onActionEnded);
}

public abstract class CharacterPersistantAction : GenericPersistantAction<Character>
{ }

public abstract class PlayerPersistantAction : GenericPersistantAction<BasePlayerBehaviour>
{ }

public abstract class RoninPlayerPersistantAction : GenericPersistantAction<RoninPlayerBehaviour>
{ }








// public class EntityActionHandler<TAction> : MonoBehaviour
// where TAction : ICharacterAction
// {
//     public TAction action;
// }

