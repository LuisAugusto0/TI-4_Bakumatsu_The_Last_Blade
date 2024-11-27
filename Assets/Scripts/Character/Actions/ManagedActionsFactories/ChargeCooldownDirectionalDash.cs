using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionalDash))]
public class ChargeCooldownDirectionalDash : CancellableActionExtractor<RoninPlayerBehaviour>
{
    DirectionalDash instance;
    public int charges = 1;
    public float cooldown = 0f;


    void Awake()
    {
        instance = GetComponent<DirectionalDash>();
    }

    public override IManagedAction InitializeManagedAction(
        RoninPlayerBehaviour target, OnActionEnded finishedCallback
    )
    {
        instance.Initialize(target, finishedCallback);

        var res = new ManagedPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, instance, GetCooldown(target.character)
        );

        return res;

    }


    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }

}
