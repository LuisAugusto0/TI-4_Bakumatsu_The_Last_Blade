using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Upgrades.Implementations.EventUpgrade;
using Upgrades.Implementations.PermanentUpgrade;

public class UpgradeIconAdressable : MonoBehaviour
{
    const string defaultPath = "Sprites/Upgrades/Icon_404.png";
    static Sprite NotExistantIcon;

    static UpgradeIconAdressable()
    {
        LoadDefaultIconAsync();
    }

    private static async void LoadDefaultIconAsync()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(defaultPath);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            NotExistantIcon = await handle.Task; // Set the icon on success.
        }
        else
        {
            Debug.LogError($"Failed to load default icon");
    
        }
    }



    public static Task<Sprite> GetDefaultIconAsync() => GetIconAsync(defaultPath);

    public static async Task<Sprite> GetIconAsync(string path)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);

        // Wait for the async operation to complete
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
           
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load sprite at {path} via Addressables.");
            return null; // Return null to indicate failure
        }
    }

    public static async void AssertIsPathValid(string path)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);

        await handle.Task;
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Adressable {path} does not exist");
        }
    }

   


    public static void AssertUpgradesPaths()
    {
        SpeedBoostAfterHitUpgrade.AssertIconAdressablePath();
        BaseDamageBonusAfterHit.AssertIconAdressablePath();

        SpeedBonusUpgrade.AssertIconAdressablePath();
        DoubleSpeedUpgrade.AssertIconAdressablePath();
        DamageBonusStatBoost.AssertIconAdressablePath();
        DamageMultiplierStatBonus.AssertIconAdressablePath();
        BaseHealthBonusUpgrade.AssertIconAdressablePath();
    }


    // Some of those upgrades are not correct or do not exist. This is a 
    // temporary implementation to ensure the adressables are working
    const string FireSwordRune = "Sprites/Images/Runes/FireSwordRune.png";
    const string ImmunityRune = "Sprites/Images/Runes/ImunityRune.png";
    const string HyperSpeedRune = "Sprites/Images/Runes/HyperSpeedRune.png";
    const string LifeRune = "Sprites/Images/Runes/LifeRune.png";
    const string SpeedRune = "Sprites/Images/Runes/SpeedRune.png";





    // Is it best to load all of them at once?
    public static async void LoadIcons()
    {
        Debug.Log("Loading Upgrade Icons");
        ImmunityAfterHitUpgrade.LoadIcon(await GetIconAsync(ImmunityRune));
        SpeedBoostAfterHitUpgrade.LoadIcon(await GetIconAsync(HyperSpeedRune));
        BaseDamageBonusAfterHit.LoadIcon(await GetIconAsync(defaultPath));

        SpeedBonusUpgrade.LoadIcon(await GetIconAsync(SpeedRune));
        DoubleSpeedUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        DamageBonusStatBoost.LoadIcon(await GetIconAsync(FireSwordRune));
        DamageMultiplierStatBonus.LoadIcon(await GetIconAsync(defaultPath));
        BaseHealthBonusUpgrade.LoadIcon(await GetIconAsync(LifeRune));
    }

    public static void UnloadIcons()
    {
        ImmunityAfterHitUpgrade.UnloadIcon();
        SpeedBoostAfterHitUpgrade.UnloadIcon();
        BaseDamageBonusAfterHit.UnloadIcon();

        SpeedBonusUpgrade.UnloadIcon();
        DoubleSpeedUpgrade.UnloadIcon();
        DamageBonusStatBoost.UnloadIcon();
        DamageMultiplierStatBonus.UnloadIcon();
        BaseHealthBonusUpgrade.UnloadIcon();
    }

    

    // If needed later on
    public static void LoadUpgradePaths()
    {
        SpeedBoostAfterHitUpgrade.SetIconAdressablePath(ImmunityRune);
        BaseDamageBonusAfterHit.SetIconAdressablePath(HyperSpeedRune);
        
        SpeedBonusUpgrade.SetIconAdressablePath(SpeedRune);
        DoubleSpeedUpgrade.SetIconAdressablePath(defaultPath);
        DamageBonusStatBoost.SetIconAdressablePath(FireSwordRune);
        DamageMultiplierStatBonus.SetIconAdressablePath(defaultPath);
        BaseHealthBonusUpgrade.SetIconAdressablePath(LifeRune);
    }

 
}


