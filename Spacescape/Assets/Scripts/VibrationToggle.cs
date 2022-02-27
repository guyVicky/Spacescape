using UnityEngine;
using UnityEngine.UI;

public class VibrationToggle : MonoBehaviour
{
    [SerializeField] Toggle vibrationToggle;
    private int value;

    void Awake()
    {
        if (PlayerPrefs.HasKey("vibration"))
        {
            PlayerPrefs.GetInt("vibration");
        }

        if (PlayerPrefs.GetInt("vibration") == 0)
            vibrationToggle.isOn = false;
    }

    public void ToggleVibrationOnValueChange(bool vibrate)
    {
        if (vibrate)
            value = 1;
        else
            value = 0;

        PlayerPrefs.SetInt("vibration", value);
    }
}
