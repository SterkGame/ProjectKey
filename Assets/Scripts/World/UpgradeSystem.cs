using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    public int damageUpgradeCost = 100;
    public int healthUpgradeCost = 150;
    public int ammoUpgradeCost = 200;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI damageButton;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthButton;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI ammoButton;

    public TextMeshProUGUI coinsText;

    public void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            data = new PlayerData();
            SaveSystem.SavePlayer(data);
        }
        coinsText.text = "������: " + data.coins;
        UpdateUI();
    }

    public void UpgradeDamage()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data.weaponDamage >= 10)
        {
            damageButton.text = "��������";
            return;
        }
        if (data.weaponDamage >= 6)
        {
            damageUpgradeCost = 500;
        }
        else if (data.weaponDamage >= 4)
        {
            damageUpgradeCost = 200;
        }
        else
        {
            damageUpgradeCost = 100;
        }

        if (data.coins >= damageUpgradeCost)
        {
            data.coins -= damageUpgradeCost;
            data.weaponDamage++;
            SaveSystem.SavePlayer(data);
            UpdateUI();
        }

        if (data.weaponDamage >= 10)
        {
            damageButton.text = "��������";
        }
        else
        {
            damageButton.text = "ֳ��: " + damageUpgradeCost + " (+1)";
        }
    }

    public void UpgradeHealth()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data.maxHealth >= 30)
        {
            healthButton.text = "��������";
            return;
        }
        else if (data.maxHealth >= 23)
        {
            healthUpgradeCost = 500;
        }
        else if (data.maxHealth >= 15)
        {
            healthUpgradeCost = 300;
        }
        else
        {
            healthUpgradeCost = 150;
        }

        if (data.coins >= healthUpgradeCost)
        {
            data.coins -= healthUpgradeCost;
            data.maxHealth += 2;
            SaveSystem.SavePlayer(data);
            UpdateUI();

            if (data.maxHealth >= 30)
                healthButton.text = "��������";
            else
                healthButton.text = "ֳ��: " + healthUpgradeCost + " (+2)";
        }
    }

    public void UpgradeAmmo()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data.maxAmmo >= 40)
        {
            ammoButton.text = "��������";
            return;
        }
        else if (data.maxAmmo >= 30)
        {
            ammoUpgradeCost = 600;
        }
        else if (data.maxAmmo >= 25)
        {
            ammoUpgradeCost = 400;
        }
        else
        {
            ammoUpgradeCost = 200;
        }

        if (data.coins >= ammoUpgradeCost)
        {
            data.coins -= ammoUpgradeCost;
            data.maxAmmo += 5;
            SaveSystem.SavePlayer(data);
            UpdateUI();

            if (data.maxAmmo >= 40)
                ammoButton.text = "��������";
            else
                ammoButton.text = "ֳ��: " + ammoUpgradeCost + " (+5)";
        }
    }


    void UpdateUI()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        damageText.text = "�����: " + data.weaponDamage;
        healthText.text = "������'�: " + data.maxHealth;
        ammoText.text = "����: " + data.maxAmmo;
        coinsText.text = "������: " + data.coins;

        if (data.weaponDamage >= 10)
            damageButton.text = "��������";
        else
            damageButton.text = "ֳ��: " + damageUpgradeCost + " (+1)";
        
        if (data.maxHealth >= 30)
            healthButton.text = "��������";
        else
            healthButton.text = "ֳ��: " + healthUpgradeCost + " (+2)";

        if (data.maxAmmo >= 40)
            ammoButton.text = "��������";
        else
            ammoButton.text = "ֳ��: " + ammoUpgradeCost + " (+5)";
    }
}
