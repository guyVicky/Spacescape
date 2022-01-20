using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(Toggle))]
public class AudioControl : MonoBehaviour
{
    [SerializeField] Toggle audioToggle;

    void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }

        // audioToggle = GetComponent<Toggle>();
        if (AudioListener.volume == 0)
            audioToggle.isOn = false;
    }

    public void ToggleAudioOnValueChange(bool audioIn)
    {
        if (audioIn)
            AudioListener.volume = 1;
        else
            AudioListener.volume = 0;

        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume);
    }
}
