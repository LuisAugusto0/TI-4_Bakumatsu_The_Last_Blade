using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class CharacterActionManager 
{
    public readonly Character character;
    public readonly Transform parentTransform;
    protected readonly OnActionEnded onActionEnded; //when it is cancellable
    
    protected IManagedAction managedAction;
    protected GameObject managedActionObject;
    

    public Character CharacterInstance {get {return character;}}
    public IManagedAction ManagedAction {get {return managedAction;}}
    public GameObject ManagedActionObject {get {return managedActionObject;}}

    public CharacterActionManager(
        Character character, 
        Transform parentTransform, 
        OnActionEnded onActionEnded
    )
    {
        this.parentTransform = parentTransform;
        this.character = character;
        this.onActionEnded = onActionEnded;
    }

    

    private TFactory SetActionInternal<T, TFactory>(
        T target,
        GameObject actionPrefab
    ) 
    where TFactory : IBaseActionPrefabFactory<T>
    where T : MonoBehaviour
    {
        // Destroy the current action object if it exists
        if (managedActionObject != null)
        {
            UnityEngine.Object.Destroy(managedActionObject);
        }

        // Instantiate the new prefab
        managedActionObject = UnityEngine.Object.Instantiate(actionPrefab, parentTransform, false);
        managedActionObject.transform.SetParent(parentTransform, false); // Use local space
        managedActionObject.transform.localPosition = Vector3.zero; // Adjust position if needed
        managedActionObject.transform.localRotation = Quaternion.identity;
        // Fetch the factory component
        TFactory dataComponent = managedActionObject.GetComponent(typeof(TFactory)) as TFactory;
        if (dataComponent == null)
        {
            Debug.LogError(
                $"The prefab {actionPrefab.name} does not implement the required interface: {typeof(TFactory).Name}"
            );
            managedActionObject = null;
            return null; // Explicitly return null for consistency
        }

        return dataComponent;
    }

    protected void SetAction<T>(T target, GameObject actionPrefab)
    where T : MonoBehaviour
    {
        var dataComponent = SetActionInternal<T, IActionPrefabFactory<T>>(
            target,
            actionPrefab
        );

        // Previous func did not fail
        if (dataComponent != null)
        {
            managedAction = dataComponent.GetManagedAction(target);
        }
    }

    protected void SetCancellableAction<T>(T target, GameObject actionPrefab)
    where T : MonoBehaviour
    {
        var dataComponent = SetActionInternal<T, ICancellableActionPrefabFactory<T>>(
            target,
            actionPrefab
        );

        // Previous func did not fail
        if (dataComponent != null)
        {
            managedAction = dataComponent.GetManagedAction(target, onActionEnded);
        }
       
    }

    
    

    
    // warning: no asserted safety for correct prefabs here
    public void SetCharacterAction(GameObject actionPrefab)
    {
        SetAction<Character>(character, actionPrefab);
    }

    public void SetCharacterCancellableAction(GameObject actionPrefab)
    {
        SetCancellableAction<Character>(character, actionPrefab);
    }




}



public class RoninPlayerActionManager : CharacterActionManager
{
    RoninPlayerBehaviourHandler player;

    public RoninPlayerActionManager(
        RoninPlayerBehaviourHandler player,
        Character character, 
        Transform parentTransform, 
        OnActionEnded onActionEnded
    ) : base(character, parentTransform, onActionEnded)
    {
        this.player = player;
    }



    public void SetPlayerAction(GameObject actionPrefab)
    {
        SetAction<AbstractPlayerBehaviourHandler>(player, actionPrefab);
    }

    public void SetPlayerCancellableAction(GameObject actionPrefab)
    {
        SetCancellableAction<AbstractPlayerBehaviourHandler>(player, actionPrefab);
    }



    public void SetRoninPlayerAction(GameObject actionPrefab)
    {
        SetAction<RoninPlayerBehaviourHandler>(player, actionPrefab);
    }

    public void SetRoninPlayerCancellableAction(GameObject actionPrefab)
    {
        SetCancellableAction<RoninPlayerBehaviourHandler>(player, actionPrefab);
    }


    private GameObject HandlePrefab(GameObject actionPrefab)
    {
        // Destroy the current action object if it exists
        if (managedActionObject != null)
        {
            UnityEngine.Object.Destroy(managedActionObject);
        }

        // Instantiate the new prefab
        return UnityEngine.Object.Instantiate(actionPrefab, parentTransform);
    }

    
    // // warning: no asserted safety for correct prefabs here
    // public void SetCharacterAction(GameObject actionPrefab)
    // {
    //     GameObject prefab = HandlePrefab(actionPrefab);
    //     var component = prefab.GetComponent<IActionPrefabFactory<AbstractPlayerBehaviourHandler>>();
        
        
    //     if (component == null) Debug.Log("THIS IS NULL --");
    //     else {
    //         var action = component.GetManagedAction(player);
    //         Debug.Log(action.ToString());
    //     }
    // }
}

