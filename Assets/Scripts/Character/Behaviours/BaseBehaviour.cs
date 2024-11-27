using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))] 
public abstract class BaseBehaviour : MonoBehaviour
{
    public abstract Character GetCharacter();
}
