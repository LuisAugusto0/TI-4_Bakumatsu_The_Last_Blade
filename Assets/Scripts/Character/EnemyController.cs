using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Character))]
public class EnemyController : MonoBehaviour
{

    // Calls the specific Enemy script to indicate the 
    // start / interruption of their attacks
    // public delegate void NotifyInRange();
    // public delegate void InterruptAttackAction();

    // // Bundles each attack with their respective start radius.
    // [Serializable]
    // public readonly struct AttackDistance {
    //     public NotifyInRange Attack { get; }
    //     public float Distance { get; }

    //     public AttackDistance(NotifyInRange action, float distance)
    //     {
    //         this.Attack = action;
    //         this.Distance = distance;
    //     }

    //     public void ExecuteAttack()
    //     {
    //         Attack?.Invoke();
    //     }
    // }

    // List of available attacks
    // public List<AttackDistance> attacks;






    // // This only allows for one attack - extensibility with child will be added later
    // // (Multiple sonar children. One of them is the main which also stops the movement)
    // // For now there is only the main sonar and thus only one delegate
    // public delegate void InAttackRange();
    // public delegate void CancelAttack();

    // [NonSerialized]
    // public InAttackRange inPrimaryMovementRange;


    // // Delay for enemy to get destroyed after death animation ended
    // public float destroyDelay = 5f;
    
    // // Radius towards player in which the enemy stops moving towards


    // // List of different attack radius
    // public List<float> attackRadius;


    // public Character character;
    // public Animator animator;
    // public Collider2D attackCollider;
    // private GameObject _player;


    // void Awake()
    // {
    //     keepAwayRadius = 1;
    //     attackCollider.enabled = false;
        
    //     _player = GameObject.FindGameObjectWithTag("Player");
    // }

    // void FixedUpdate()
    // {
    //     if (!character.isActionLocked)
    //     {
    //         FollowPlayer();
    //     }
    // }

    // float distance = 0f;
    // void FollowPlayer()
    // {
    //     distance = Vector2.Distance(
    //         new Vector2(transform.position.x, transform.position.y), 
    //         new Vector2(_player.transform.position.x, _player.transform.position.y)
    //     );

    //     if (distance <= keepAwayRadius)
    //     {
    //         inPrimaryMovementRange.Invoke();
    //     }
    //     else
    //     {
    //         Vector2 direction = (_player.transform.position - transform.position).normalized;
    //         character.Move(direction * character.moveSpeed);
    //     }
    // }


}
