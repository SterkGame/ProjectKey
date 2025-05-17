using System.Collections.Generic;
using UnityEngine;

public class SkinShopManager : MonoBehaviour
{
    public List<SkinShopItem> Shop;

    public void SelectSkin(string selectedSkinID, SkinType type)
    {
        var data = SaveSystem.LoadPlayer();

        if (type == SkinType.Player)
        {
            data.currentSkin = selectedSkinID;
        }
        else if (type == SkinType.Weapon)
        {
            data.currentWeaponSkin = selectedSkinID;
        }

        SaveSystem.SavePlayer(data);

        foreach (var item in Shop)
        {

            if (item.skinType == type)
            {
                item.UpdateStatus();
            }
        }
    }
}
