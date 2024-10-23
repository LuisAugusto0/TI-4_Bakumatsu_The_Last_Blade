using UnityEngine;
using UnityEngine.Events;
using System;

public class InstantiateProjectile : CharacterCooldownAction
{
    [Serializable]
    public class OnFiredEvent : UnityEvent<InstantiateProjectile>
    { }

    public GameObject prefab;
    public float rotationOffset;
    public Transform spawnPos;

    [Tooltip("Event triggered when the skill is fired.")]
    public OnFiredEvent fired;

    protected override void Perform(int context = 0)
    {
        float angle = Mathf.Atan2(character.lastLookDirection.y, character.lastLookDirection.x) * Mathf.Rad2Deg;

        angle += rotationOffset;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(prefab, spawnPos.position, rotation);

        fired.Invoke(this);

        End();


    }
}


