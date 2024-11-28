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
    public static Task<Sprite> GetDefaultIconAsync() => GetIconAsync("Sprites/Effects/Icon_404.png");

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



    // static UpgradeIconAdressable()
    // {
    //     LoadIcons();
    // }

    public static async void LoadIcons()
    {
        Debug.Log("HERE!@!!");
        ImmunityAfterHitUpgrade.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        SpeedBoostAfterHitUpgrade.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        BaseDamageBonusAfterHit.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));

        SpeedBonusUpgrade.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        DoubleSpeedUpgrade.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        DamageBonusStatBoost.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        DamageMultiplierStatBonus.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        BaseHealthBonusUpgrade.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
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
