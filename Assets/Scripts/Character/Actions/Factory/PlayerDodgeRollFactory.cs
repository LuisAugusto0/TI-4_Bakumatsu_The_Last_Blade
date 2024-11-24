using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionPrefabManager : MonoBehaviour
{
    // Contains same name as asset file:
    // Instead of matching enum with string, c# allows you to turn
    // enum into its exact string match on ToString() method
    public enum PrefabName
    {
        Player,
        Enemy,
        PowerUp
    }

}
