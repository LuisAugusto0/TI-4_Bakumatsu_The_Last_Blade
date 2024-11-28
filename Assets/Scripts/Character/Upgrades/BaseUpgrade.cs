using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Upgrade
{
    public abstract Sprite GetIcon();
    public int Quantity { get {return quantity;}}
    protected int quantity = 1;
    public readonly UpgradeManager target;

    public Upgrade(UpgradeManager entity, int quantity)
    {
        this.target = entity;
        this.quantity = quantity;
        // Expand constructor to add permanent changes that does not 
        // change on each update (example: adding listeners)
    }


    public void AddToQuantity(int value)
    {
        int newQuantity = this.quantity + value;
        if (newQuantity <= 0)
        {
            Remove();
        }
        else
        {
            UpdateQuantity(this.quantity + value);
        }
    }

    protected virtual void UpdateQuantity(int quantity)
    {
        this.quantity = quantity;
        // Expand to change effect after quantity is changed
    }

    // Call when the effect is cleared
    public virtual void Remove()
    {
        target.RemoveUpgradeInstance(this.GetType());
        // Expand End() to reset additional changes
    }
}

