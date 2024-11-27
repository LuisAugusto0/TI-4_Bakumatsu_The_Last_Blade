using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RoninDodgeRoll))]
public class CooldownRoninDodgeRoll : CancellableActionExtractor<RoninPlayerBehaviour>
{
    public float cooldown = 1f;
    public int charges = 1;
    RoninDodgeRoll instance;

    void Awake()
    {
        instance = GetComponent<RoninDodgeRoll>();
    }
    public override IManagedAction InitializeManagedAction(
        RoninPlayerBehaviour target, OnActionEnded finishedCallback)
    {
        instance.Initialize(target, finishedCallback);
        return new ManagedPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, instance, GetCooldown(target.character)
        );
    }


    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }
}
