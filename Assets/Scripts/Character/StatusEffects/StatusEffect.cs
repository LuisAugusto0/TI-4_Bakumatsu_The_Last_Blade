using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


/* Contains information on what to change / add, and when to end
 *
 * Ending usually means making sure changes are reset and removing self from
 * a data structure that holds reference to self
 *
 * Behaviour is self managed; when to call Remove() and what to do until then
 * can greatly vary. Some implementations include:
 *   - TimedStatusEffect: call Remove() after given time
 *   - ChargedStatusEffect: call Remove() after charges reach 0
 *   - LooseStatusEffect: has its own specific way to handle Remove() 
 *                        (mostly through specific listeners)
 * 
 * For now only TimedStatusEffect is being considered 
 */
public abstract class StatusEffect {
    protected StatusEffectManager target;


    // Clear effect and remove self from some data structure
    public virtual void Remove()
    {
        RemoveSelfFromStructure();
    }

    protected abstract void RemoveSelfFromStructure();


}


