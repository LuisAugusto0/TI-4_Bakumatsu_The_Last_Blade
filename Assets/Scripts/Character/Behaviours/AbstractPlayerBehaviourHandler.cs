using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))] 
public abstract class AbstractPlayerBehaviourHandler : MonoBehaviour
{
    // Static management Player
    // Player must always exist on the scene otherwise other components will break
    // Not a singleton, as more than one player may exist
    public static AbstractPlayerBehaviourHandler ActivePlayer { get { return s_ActivePlayer; } }
    protected static AbstractPlayerBehaviourHandler s_ActivePlayer;


    protected Vector2 _moveInputVector;
    public Vector2 LastMoveInputVector {get {return _moveInputVector;}}
 
    protected Vector2 _lastFacingDirection;
    public Vector2 LastPointDirection {get {return _lastFacingDirection;}}

    [NonSerialized]
    public Camera mainCamera;

    [NonSerialized]
    public Character character;


    protected virtual void Awake()
    {
        s_ActivePlayer = this;
        character = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        mainCamera = CameraController.Instance.MainCamera;
    }

    public virtual void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInputVector = context.ReadValue<Vector2>().normalized;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        // Uses screen position (for now only Mouse)
        if (context.control.device is Mouse)
        {
            Vector2 mousePosition  = context.ReadValue<Vector2>();
            Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(
                new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane)
            );
            Vector2 dir = worldMousePosition - transform.position;
            _lastFacingDirection = dir.normalized;
        }
        // Uses normalized vector2
        else 
        {
            _lastFacingDirection = context.ReadValue<Vector2>();
        }
    }

    public virtual void OnDodgeInputStarted(InputAction.CallbackContext context) {}
    public virtual void OnDodgeInputPerformed(InputAction.CallbackContext context) {}
    public virtual void OnDodgeInputCancelled(InputAction.CallbackContext context) {}
        
    public virtual void OnAttackInputStarted(InputAction.CallbackContext context) {}
    public virtual void OnAttackInputPerformed(InputAction.CallbackContext context) {}
    public virtual void OnAttackInputCancelled(InputAction.CallbackContext context) {}

    public virtual void OnSkillInputStarted(InputAction.CallbackContext context) {}
    public virtual void OnSkillInputPerformed(InputAction.CallbackContext context) {}
    public virtual void OnSkillInputCancelled(InputAction.CallbackContext context) {}


}
