using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Generic Player Behaviour Handler class to handle which actions to take
// Contains abstract and fixed implementations for how to handle inputs
[RequireComponent(typeof(BasePlayerBehaviour))]

// Contains new "instance" of the Input Action
// Ensures changes to the input are not hardcoded and local
// multiplayer acessible. 
// <If time available, test if c# InputAsset auto generated script 
// would be a better choise for manual control>
[RequireComponent(typeof(PlayerInput))]

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput; 
    private BasePlayerBehaviour player;


    // Input definitions
    InputActionMap _playerMap; // when moving around as character
    InputAction _playerDodgeAction; //button
    InputAction _playerSlashAction; //button
    InputAction _playerSkillAction; //button
    InputAction _playerMoveAction;  //vector2
    InputAction _playerAimAction;   //vector2 
    InputAction _playerPauseAction; //button 
    

    InputActionMap _uiMap; // when on ui menus
    InputAction _uiNavigate;
    InputAction _uiSubmit;
    InputAction _uiCancel;
    InputAction _uiPoint;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<BasePlayerBehaviour>();


        _playerMap = playerInput.actions.FindActionMap("Player");
        _uiMap = playerInput.actions.FindActionMap("UI");


        _playerDodgeAction = _playerMap.FindAction("Dodge");
        _playerSlashAction = _playerMap.FindAction("Slash");
        _playerSkillAction = _playerMap.FindAction("Skill"); 
        _playerMoveAction = _playerMap.FindAction("Move");
        _playerAimAction = _playerMap.FindAction("Aim");
        _playerPauseAction = _playerMap.FindAction("Pause");

        _uiNavigate = _uiMap.FindAction("Navigate");
        _uiSubmit = _uiMap.FindAction("Submit");
        _uiCancel = _uiMap.FindAction("Cancel");
        _uiPoint = _uiMap.FindAction("Point");
    }

    void Start()
    {
        _uiMap.Disable();
    }


    // Manually enable / disable PlayerController events
    // Since PlayerInput is only limited to .performed (hold), this script was added
    // for greater controls of inputs and map switches
    void OnEnable()
    {
        /// -- PLAYER MAP ASSIGNMENT --

        // -- Abstract mappings --
        _playerDodgeAction.started += player.OnDodgeInputStarted;
        _playerDodgeAction.performed += player.OnDodgeInputPerformed;
        _playerDodgeAction.canceled += player.OnDodgeInputCancelled;

        _playerSlashAction.started += player.OnAttackInputStarted;
        _playerSlashAction.performed += player.OnAttackInputPerformed;
        _playerSlashAction.canceled += player.OnAttackInputCancelled;

        _playerSkillAction.started += player.OnSkillInputStarted;
        _playerSkillAction.performed += player.OnSkillInputPerformed;
        _playerSkillAction.canceled += player.OnSkillInputCancelled;

        // -- Defined behaviour mappings --

        // Get when you start (first frame), performed (remaining frames), 
        // and when ended (get Vector2.zero for move vector)
        _playerMoveAction.started += player.OnMoveInput;
        _playerMoveAction.performed += player.OnMoveInput;
        _playerMoveAction.canceled += player.OnMoveInput;

        // Aim (continuous)
        _playerAimAction.performed += player.OnAim;

        // Pause only when released
        _playerPauseAction.canceled += TogglePauseOnInput;



        // -- UI MAP ASSIGNMENT --

        // Unpause only when released
        _uiCancel.canceled += TogglePauseOffInput;

    }

    void OnDisable()
    {
        // -- PLAYER MAP ASSIGNMENT --
        _playerDodgeAction.started -= player.OnDodgeInputStarted;
        _playerDodgeAction.performed -= player.OnDodgeInputPerformed;
        _playerDodgeAction.canceled -= player.OnDodgeInputCancelled;

        _playerSlashAction.started -= player.OnAttackInputStarted;
        _playerSlashAction.performed -= player.OnAttackInputPerformed;
        _playerSlashAction.canceled -= player.OnAttackInputCancelled;

        _playerSkillAction.started -= player.OnSkillInputStarted;
        _playerSkillAction.performed -= player.OnSkillInputPerformed;
        _playerSkillAction.canceled -= player.OnSkillInputCancelled;

        _playerMoveAction.started -= player.OnMoveInput;
        _playerMoveAction.performed -= player.OnMoveInput;
        _playerMoveAction.canceled -= player.OnMoveInput;

        _playerAimAction.performed -= player.OnAim;

        _playerPauseAction.canceled -= TogglePauseOnInput;



        // -- UI MAP ASSIGNMENT --
        _uiCancel.canceled -= TogglePauseOffInput;
    }






  
    void TogglePauseOnInput(InputAction.CallbackContext context)
    {  
        TogglePauseOn();
    }

    
    void TogglePauseOffInput(InputAction.CallbackContext context)
    {
      TogglePauseOff();
    }

    public void TogglePauseOn()
    {
      
        _playerMap.Disable();
        _uiMap.Enable();

        PauseMenu.Instance.BeginPause();
    }

    
    public void TogglePauseOff()
    {
      
        _playerMap.Enable();
        _uiMap.Disable();

        PauseMenu.Instance.Unpause();
    }

}
