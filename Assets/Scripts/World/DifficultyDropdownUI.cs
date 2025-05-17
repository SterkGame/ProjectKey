using TMPro;
using UnityEngine;

public class DifficultyDropdownUI : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private void Start()
    {
        LoadDifficulty();
        dropdown.onValueChanged.AddListener(SetDifficultyFromDropdown);
    }

    void LoadDifficulty()
    {
        var data = SaveSystem.LoadPlayer();
        dropdown.value = (int)data.difficulty;
    }

    void SetDifficultyFromDropdown(int value)
    {
        var data = SaveSystem.LoadPlayer();
        data.difficulty = (GameDifficulty)value;
        SaveSystem.SavePlayer(data);
        Debug.Log("Difficulty set to: " + data.difficulty);
    }
}
