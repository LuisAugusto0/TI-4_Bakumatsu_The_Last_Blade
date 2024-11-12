using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* ABANDONED FOR NOW 
 * NOT MANY USE CASES, NEEDS REVIEW TO SEE IF IT FITS THE GAME
 */

/* Status effect based on charges 
 * COntains a number of inner charges that may be externally updated
 * Each effect execution consumes one charge
 * Once the charges are over, Remove() is called
 */

[Obsolete]
public abstract class ChargeStatusEffect : StatusEffect
{
    // extending / decreasing duration will not reset activated time
    protected int charges;
    public int Charges{get {return charges;}}

    public ChargeStatusEffect(StatusEffectManager target, int charges)
    {
        this.target = target;
        this.charges = charges;
       
        // Expand contructor to add effects
    }


    public void SetCharges(int charges)
    {
        if (charges <= 0)
        {
            Debug.LogError($"Unexpected charge value received {charges}");
            Remove();
        }
        this.charges = charges;
    }

    public void AddToCharges(int charges)
    {
        this.charges += charges;
        if (this.charges <= 0)
        {
            Remove();
        }
    }

    // Refreshes duration if its higher than current duration
    public void RefreshCharges(int charges)
    {
        this.charges = Mathf.Max(charges, this.charges);
    } 

}