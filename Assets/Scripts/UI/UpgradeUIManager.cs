using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private Transform iconContainer; // Container para os ícones
    [SerializeField] private GameObject iconPrefab; // Prefab do ícone

    private Dictionary<Type, GameObject> upgradeIcons = new();

    private void Start()
    {
        // Escutar evento de atualização de upgrades
        if (upgradeManager != null)
        {
            upgradeManager.onUpgradeUpdated.AddListener(UpdateUI);
        }
        
        // Garantir que os ícones iniciais estão carregados
        UpgradeIconAdressable.LoadIcons();
    }

    private async void UpdateUI(Upgrade upgrade)
    {
        if (upgrade == null) return;

        Type upgradeType = upgrade.GetType();

        // Verifica se o ícone já existe
        if (upgradeIcons.ContainsKey(upgradeType))
        {
            // Atualiza a quantidade no texto
            var icon = upgradeIcons[upgradeType];
            var quantityText = icon.GetComponentInChildren<Text>();
            quantityText.text = upgrade.Quantity.ToString();
        }
        else
        {
            // Cria novo ícone
            var newIcon = Instantiate(iconPrefab, iconContainer);
            upgradeIcons[upgradeType] = newIcon;

            var iconImage = newIcon.GetComponentInChildren<Image>();
            var quantityText = newIcon.GetComponentInChildren<Text>();

            // Configura o ícone e quantidade inicial
            Sprite iconSprite = await UpgradeIconAdressable.GetIconAsync(GetIconPath(upgradeType));
            iconImage.sprite = iconSprite ?? await UpgradeIconAdressable.GetDefaultIconAsync();
            quantityText.text = upgrade.Quantity.ToString();
        }
    }

    private string GetIconPath(Type upgradeType)
    {
        // Define os caminhos Addressable baseados no tipo de upgrade
        return upgradeType.Name switch
        {
            _ => "Sprites/Upgrades/Default.png"
        };
    }

    private void OnDestroy()
    {
        // Garantir que os ícones sejam descarregados
        UpgradeIconAdressable.UnloadIcons();
    }
}
