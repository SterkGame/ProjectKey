using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public List<SkinOption> skins;

    void Start()
    {
        var data = SaveSystem.LoadPlayer();
        ApplySkin(data.currentSkin, SkinType.Player);
        ApplySkin(data.currentWeaponSkin, SkinType.Weapon);
    }

    public void ApplySkin(string skinID, SkinType type)
    {
        foreach (var skin in skins)
        {
            if (skin.skinType != type) continue;

            skin.model.SetActive(skin.skinID == skinID);
        }
    }
}


[System.Serializable]
public class SkinOption
{
    public string skinID;
    public GameObject model;
    public SkinType skinType;
}
