using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RoninBaseAttack))]
public class UnrestrictedRoninBaseAttack : CancellableActionExtractor<RoninPlayerBehaviour>
{

    RoninBaseAttack instance;

    void Awake()
    {
        instance = GetComponent<RoninBaseAttack>();
    }


    public override IManagedAction InitializeManagedAction(
        RoninPlayerBehaviour target, OnActionEnded finishedCallback
    )
    {
        instance.Initialize(target, finishedCallback);
        return new ManagedPersistantAction(target.character, instance);
    }
}