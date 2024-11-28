using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Effects.Implementations.ChargeRepeatingEffects;
using Effects.Implementations.TimedEffect;
using Effects.Implementations.TimedRepeatingEffect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectIconAdressable : MonoBehaviour
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



    // static EffectIconAdressable()
    // {
    //     LoadIcons();
    // }

    public static async void LoadIcons()
    {
        ChargeRepeatingBurningEffect.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));

        SlidingShoes.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));

        TimedImmunityEffect.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        TimedPositiveFixedSpeedBonusEffect.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        TimedPositiveFixedDamageBonusEffect.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        TimedFixedHealthBonusEffect.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
        TimedPositiveSpeedMultiplier.LoadIcon(await GetIconAsync("Sprites/Effects/Icon_404.png"));
    }

    public static void UnloadIcons()
    {
        ChargeRepeatingBurningEffect.UnloadIcon();

        SlidingShoes.UnloadIcon();

        TimedImmunityEffect.UnloadIcon();
        TimedPositiveFixedSpeedBonusEffect.UnloadIcon();
        TimedPositiveFixedDamageBonusEffect.UnloadIcon();
        TimedFixedHealthBonusEffect.UnloadIcon();
        TimedPositiveSpeedMultiplier.UnloadIcon();
    }


}
