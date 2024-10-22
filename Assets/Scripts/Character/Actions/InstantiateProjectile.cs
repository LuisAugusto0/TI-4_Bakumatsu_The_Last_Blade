using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateProjectile : CharacterAction
{
    public GameObject prefab;
    public float rotationOffset;
    public Transform spawnPos;

    protected override void Perform(int context = 0)
    {
        float angle = Mathf.Atan2(character.lastLookDirection.y, character.lastLookDirection.x) * Mathf.Rad2Deg;

        angle += rotationOffset;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(prefab, spawnPos.position, rotation);

        End();
    }
}
