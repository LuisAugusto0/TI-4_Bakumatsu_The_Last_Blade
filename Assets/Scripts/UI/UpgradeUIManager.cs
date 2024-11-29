using System;
using System.Collections.Generic;
using TMPro;
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

    private async void UpdateUI(BaseUpgrade upgrade)
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
            var quantityText = newIcon.GetComponentInChildren<TMP_Text>();
            

            // Configura o ícone e quantidade inicial
            Sprite iconSprite = upgrade.GetIcon();
            if (iconSprite == null) iconSprite = await UpgradeIconAdressable.GetDefaultIconAsync();
            iconImage.sprite = iconSprite;
            quantityText.text = "x"+upgrade.Quantity.ToString();
        }
    }

    private void OnDestroy()
    {
        // Garantir que os ícones sejam descarregados
        UpgradeIconAdressable.UnloadIcons();
    }
}
