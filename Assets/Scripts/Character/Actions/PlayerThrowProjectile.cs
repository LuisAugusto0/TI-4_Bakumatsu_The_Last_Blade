using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerThrowProjectile : IAction
{
    [Serializable]
    public class OnFiredEvent : UnityEvent<PlayerThrowProjectile>
    { }

    public AbstractPlayerBehaviourHandler player;
    public GameObject prefab;
    public float rotationOffset;
    public Transform spawnPos;

    [Tooltip("Event triggered when the skill is fired.")]
    public OnFiredEvent fired;

    public override void StartAction(OnActionEnded callback)
    {
        finished = callback;

        float angle = Mathf.Atan2(player.LastPointDirection.y, player.LastPointDirection.x) * Mathf.Rad2Deg;

        angle += rotationOffset;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(prefab, spawnPos.position, rotation);
        

        fired.Invoke(this);
        finished.Invoke();
    }
}
