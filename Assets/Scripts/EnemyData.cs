using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public float moveSpeed = 3;
    public float maxHealt = 20;
    public float maxDist = 1;
    public float damage = 1;
    public float currentDamage = 1;
    public float currentMoveSpeed = 3;
    public float currentHealt = 20;

    public void takeDamage(float dmg)
    {
        currentHealt -= dmg;

        if (currentHealt <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
