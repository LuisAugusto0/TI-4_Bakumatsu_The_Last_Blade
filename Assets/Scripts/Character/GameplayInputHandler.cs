using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputHandler : MonoBehaviour
{
    // Contains new "instance" of the Input Action
    // Ensures changes to the input are not hardcoded and local
    // multiplayer acessible. 
    // <If time available, test if c# InputAsset auto generated script 
    // would be a better choise for manual control>
    public PlayerInput playerInput; 


    // Signal player action inputs to the controller
    public PlayerController player;


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
        // -- PLAYER MAP ASSIGNMENT --

        // Dodge Only when released
        _playerDodgeAction.canceled += player.OnDashInput;

        // Slash Only when released
        _playerSlashAction.canceled += player.OnSlashInput;

        // Use skill Only when released
        _playerSkillAction.canceled += player.OnSkillInput;

        // Get when you start (first frame), performed (remaining frames), 
        // and when ended (get Vector2.zero for move vector)
        _playerMoveAction.started += player.OnMoveInput;
        _playerMoveAction.performed += player.OnMoveInput;
        _playerMoveAction.canceled += player.OnMoveInput;

        // Aim (continuous)
        _playerAimAction.performed += player.OnAim;

        // Pause only when released
        _playerPauseAction.canceled += TogglePauseOn;



        // -- UI MAP ASSIGNMENT --

        // Unpause only when released
        _uiCancel.canceled += TogglePauseOff;

    }

    void OnDisable()
    {
        // -- PLAYER MAP ASSIGNMENT --
        _playerDodgeAction.canceled -= player.OnDashInput;

        _playerSlashAction.canceled -= player.OnSlashInput;

        _playerSkillAction.canceled -= player.OnSkillInput;

        _playerMoveAction.started -= player.OnSkillInput;
        _playerMoveAction.performed -= player.OnSkillInput;
        _playerMoveAction.canceled -= player.OnSkillInput;

        _playerAimAction.performed -= player.OnAim;

        _playerPauseAction.canceled -= TogglePauseOn;



        // -- UI MAP ASSIGNMENT
        _uiCancel.canceled -= TogglePauseOff;
    }


    void TogglePauseOn(InputAction.CallbackContext context)
    {
      
        _playerMap.Disable();
        _uiMap.Enable();

        PauseMenu.Instance.BeginPause();
    }

    
    void TogglePauseOff(InputAction.CallbackContext context)
    {
      
        _playerMap.Enable();
        _uiMap.Disable();

        PauseMenu.Instance.Unpause();
    }

}
