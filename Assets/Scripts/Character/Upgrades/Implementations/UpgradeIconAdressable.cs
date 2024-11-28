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

   
    public static void LoadUpgradePaths()
    {
        SpeedBoostAfterHitUpgrade.SetIconAdressablePath(defaultPath);
        BaseDamageBonusAfterHit.SetIconAdressablePath(defaultPath);
        
        SpeedBonusUpgrade.SetIconAdressablePath(defaultPath);
        DoubleSpeedUpgrade.SetIconAdressablePath(defaultPath);
        DamageBonusStatBoost.SetIconAdressablePath(defaultPath);
        DamageMultiplierStatBonus.SetIconAdressablePath(defaultPath);
        BaseHealthBonusUpgrade.SetIconAdressablePath(defaultPath);
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





    // Not best solutions -> may be dropped later
    public static async void LoadIcons()
    {
        Debug.Log("HERE!@!!");
        ImmunityAfterHitUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        SpeedBoostAfterHitUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        BaseDamageBonusAfterHit.LoadIcon(await GetIconAsync(defaultPath));

        SpeedBonusUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        DoubleSpeedUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        DamageBonusStatBoost.LoadIcon(await GetIconAsync(defaultPath));
        DamageMultiplierStatBonus.LoadIcon(await GetIconAsync(defaultPath));
        BaseHealthBonusUpgrade.LoadIcon(await GetIconAsync(defaultPath));
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


 
}


