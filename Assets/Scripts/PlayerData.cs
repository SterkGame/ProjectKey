using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public List<string> purchasedItems = new List<string>();
    public string currentSkin = "default";
    public string currentWeaponSkin = "m4";

    public int weaponDamage = 2;
    public int maxHealth = 10;
    public int maxAmmo = 15;

    public int totalDeaths = 0;
    public int totalKills = 0;
    public int totalWins = 0;

    public GameDifficulty difficulty = GameDifficulty.Середній;
}

public enum GameDifficulty
{
    Легкий,
    Середній,
    Важкий
}

