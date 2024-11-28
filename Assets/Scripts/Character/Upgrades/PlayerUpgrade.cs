using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#nullable enable

public abstract class RoninPlayerUpgrade : BaseUpgrade
{ 
    RoninPlayerBehaviour player;
    public RoninPlayerUpgrade(UpgradeManager target, int quantity, RoninPlayerBehaviour player)
    : base(target, quantity) 
    { 
        this.player = player;
        FirstStart();
    }
    
    protected abstract void FirstStart();


}

public class RoninPlayerFireSwordUpgrade : RoninPlayerUpgrade
{
    static Sprite? icon;
    public static Sprite? GetStaticIcon() => icon;
    public override Sprite? GetIcon() => icon;

    public static void LoadIcon(Sprite sprite) => icon = sprite;
    public static void UnloadIcon() => icon = null;

    static GameObject? action;
    public static void LoadActionPrefab(GameObject prefab)
    {
        action = prefab;
    }

    public static void UnloadActionPrefab() => action = null;

    const int BaseCharges = 2;
    int charges = 2;


    public RoninPlayerFireSwordUpgrade(UpgradeManager target, int quantity, RoninPlayerBehaviour player)
    : base(target, quantity, player) 
    {}

    protected override void FirstStart()
    {
        charges = BaseCharges + quantity;
        
            
    }

    protected override void UpdateQuantity(int quantity)
    {
        this.quantity = quantity;
        charges = BaseCharges + quantity;
    }
}

