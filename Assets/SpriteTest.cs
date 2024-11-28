using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrades.Implementations.PermanentUpgrade;


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTest : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LoadImage());
    }

    IEnumerator LoadImage()
    {
        UpgradeIconAdressable.LoadIcons();
        while (BaseHealthBonusUpgrade.GetStaticIcon() == null)
        {
            yield return null;
        }
        spriteRenderer.sprite = BaseHealthBonusUpgrade.GetStaticIcon();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
