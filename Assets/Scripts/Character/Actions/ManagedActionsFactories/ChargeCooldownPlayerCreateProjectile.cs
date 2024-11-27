using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCreateProjectile))]
public class ChargeCooldownPlayerCreateProjectile : ActionExtractor<BasePlayerBehaviour>
{
    PlayerCreateProjectile instance;

    public int charges = 1;
    public float cooldown = 0f;
    

    void Awake()
    {
        instance = GetComponent<PlayerCreateProjectile>();
    }

    public override IManagedAction GetManagedAction(BasePlayerBehaviour target)
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