using UnityEngine;
using TMPro;

public class SkinShopItem : MonoBehaviour
{
    public string skinID;
    public int skinCost = 500;
    public SkinType skinType = SkinType.Player;

    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI coinsText;

    public SkinShopManager shopManager;

    void Start()
    {
        UpdateStatus();
    }

    public void OnBuyOrSelect()
    {
        var data = SaveSystem.LoadPlayer();

        if (!data.purchasedItems.Contains(skinID))
        {

            if (data.coins >= skinCost)
            {
                data.coins -= skinCost;
                data.purchasedItems.Add(skinID);
                SetSkin(data); 
                SaveSystem.SavePlayer(data);

                shopManager.SelectSkin(skinID, skinType);
            }
        }
        else
        {

            SetSkin(data);
            SaveSystem.SavePlayer(data);

            shopManager.SelectSkin(skinID, skinType);
        }

        UpdateStatus();
    }

    private void SetSkin(PlayerData data)
    {
        if (skinType == SkinType.Player)
            data.currentSkin = skinID;
        else if (skinType == SkinType.Weapon)
            data.currentWeaponSkin = skinID;
    }

    public void UpdateStatus()
    {
        var data = SaveSystem.LoadPlayer();
        bool isSelected = (skinType == SkinType.Player && data.currentSkin == skinID) ||
                          (skinType == SkinType.Weapon && data.currentWeaponSkin == skinID);

        if (isSelected)
        {
            buttonText.text = "Вибрано";
        }
        else if (data.purchasedItems.Contains(skinID))
        {
            buttonText.text = "Вибрати";
        }
        else
        {
            buttonText.text = "Купити (" + skinCost + ")";
        }

        statusText.text = data.purchasedItems.Contains(skinID) ? " " : "Новий!";
        coinsText.text = "Монети: " + data.coins;
    }
}


public enum SkinType
{
    Player,
    Weapon
}