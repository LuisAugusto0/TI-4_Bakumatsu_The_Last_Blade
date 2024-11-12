using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Il2Cpp;
using UnityEngine;


// Separate object that contains 

public delegate void OnActionEnded();

[Serializable]
public abstract class IAction : MonoBehaviour
{
    public OnActionEnded finished = () => {}; //default (do nothing)
    public bool isInstanteneous;


    public abstract void StartAction(OnActionEnded callback);
    public virtual bool AttemptCancelAction() {return false;} 
}

public interface CooldownIAction
{
    public bool Attempt();
}



[Serializable]
public class Cooldown
{
    [SerializeField] 
    private float cooldown; 

    [SerializeField]
    private float lastTimeUsed; 

    public bool IsOnCooldown => Time.time < lastTimeUsed + cooldown;


    public float GetRemaningTime()
    {
        return cooldown - (Time.time - lastTimeUsed);
    }
   
    public void StartCooldown()
    {
        lastTimeUsed = Time.time;
    }
   
    public void ResetCooldown()
    {
        lastTimeUsed = Time.time;
    }
}



[Serializable]
public class CooldownAction : CooldownIAction
{
    [SerializeField]
    public Cooldown cooldown;

    [SerializeField]
    public IAction action;

    bool onAction = false;

    OnActionEnded callback = () => {};

    public void SetEndedCallback(OnActionEnded callback)
    {
        this.callback = callback;
    }

    public bool Attempt()
    {
        bool success = false;
        if (!cooldown.IsOnCooldown && !onAction)
        {
            onAction = true;
            action.StartAction(ActionEnd);
            
        }
        return success;
    }

    void ActionEnd()
    {
        onAction = false;
        cooldown.ResetCooldown();
        callback.Invoke();
    }
}