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
        BeserkUpgrade.AssertIconAdressablePath();

        SpeedBonusUpgrade.AssertIconAdressablePath();
        DoubleSpeedUpgrade.AssertIconAdressablePath();
        DamageBonusStatBoost.AssertIconAdressablePath();
        DamageMultiplierStatBonus.AssertIconAdressablePath();
        BaseHealthBonusUpgrade.AssertIconAdressablePath();
    }


    // Some of those upgrades are not correct or do not exist. This is a 
    // temporary implementation to ensure the adressables are working


    const string FireOnHitRune = "Sprites/Images/Runes/FireOnHitRune.png"; // ORANGE

    const string EscapeRune = "Sprites/Images/Runes/EscapeRune.png"; // YELLOW

    const string LifeRune = "Sprites/Images/Runes/LifeRune.png"; //PINK
    
    const string SpeedRune = "Sprites/Images/Runes/SpeedRune.png"; //BLUE
    
    const string DamageRune = "Sprites/Images/Runes/DamageRune.png"; //RED

    const string BeserkRune = "Sprites/Images/Runes/BeserkRune.png"; //GREEN




    // Is it best to load all of them at once?
    public static async void LoadIcons()
    {
        Debug.Log("Loading Upgrade Icons");

        SpeedBoostAfterHitUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        BeserkUpgrade.LoadIcon(await GetIconAsync(BeserkRune));
        EscapeUpgrade.LoadIcon(await GetIconAsync(EscapeRune));
        FireOnHitUpgrade.LoadIcon(await GetIconAsync(FireOnHitRune));

        SpeedBonusUpgrade.LoadIcon(await GetIconAsync(SpeedRune));
        DoubleSpeedUpgrade.LoadIcon(await GetIconAsync(defaultPath));
        DamageBonusStatBoost.LoadIcon(await GetIconAsync(DamageRune));
        DamageMultiplierStatBonus.LoadIcon(await GetIconAsync(defaultPath));
        BaseHealthBonusUpgrade.LoadIcon(await GetIconAsync(LifeRune));
        
    }

    public static void UnloadIcons()
    {
        SpeedBoostAfterHitUpgrade.UnloadIcon();
        BeserkUpgrade.UnloadIcon();
        EscapeUpgrade.UnloadIcon();
        FireOnHitUpgrade.UnloadIcon();

        SpeedBonusUpgrade.UnloadIcon();
        DoubleSpeedUpgrade.UnloadIcon();
        DamageBonusStatBoost.UnloadIcon();
        DamageMultiplierStatBonus.UnloadIcon();
        BaseHealthBonusUpgrade.UnloadIcon();
        
    }

    

    // If needed later on
    public static void LoadUpgradePaths()
    {
        SpeedBoostAfterHitUpgrade.SetIconAdressablePath(defaultPath);
        BeserkUpgrade.SetIconAdressablePath(defaultPath);
        FireOnHitUpgrade.SetIconAdressablePath(defaultPath);
        EscapeUpgrade.SetIconAdressablePath(defaultPath);
        
        SpeedBonusUpgrade.SetIconAdressablePath(defaultPath);
        DoubleSpeedUpgrade.SetIconAdressablePath(defaultPath);
        DamageBonusStatBoost.SetIconAdressablePath(defaultPath);
        DamageMultiplierStatBonus.SetIconAdressablePath(defaultPath);
        BaseHealthBonusUpgrade.SetIconAdressablePath(defaultPath);
        
    }

 
}


