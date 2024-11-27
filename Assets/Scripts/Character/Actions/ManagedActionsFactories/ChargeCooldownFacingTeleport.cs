using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FacingTeleport))]
public class ChargeCooldownFacingTeleport : ActionExtractor<RoninPlayerBehaviour>
{
    public int charges = 1;
    public float cooldown = 0f;
    FacingTeleport instance;
    
    void Awake()
    {
        instance = GetComponent<FacingTeleport>();
    }

    public override IManagedAction GetManagedAction(RoninPlayerBehaviour target)
    {
        instance.Initialize(target);
        return new ManagedNonPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, instance, GetCooldown(target.character)
        );
    }

    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }
}