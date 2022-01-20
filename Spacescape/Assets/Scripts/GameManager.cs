using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public PlayFabManager playFabManager;

    public GameObject loadingCanvas;
    public GameObject mainCanvas;

    public GameObject removeAdsBtn;


    public Slider slider;
    public TextMeshProUGUI progressText;

    private void Awake()
    {
        var currResolution = Screen.currentResolution.ToString();
        var refreshRate = (currResolution.Substring(currResolution.Length - 4)).Substring(0, 2);
        Application.targetFrameRate = int.Parse(refreshRate);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Quit()
    {
        // Debug.Log("Quited !");
        Application.Quit();
    }

    public void PlayGame()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        // SceneManager.LoadScene(1);
        StartCoroutine(LoadAsynchronously(2));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        mainCanvas.SetActive(false);
        loadingCanvas.SetActive(true);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }

    public void MainMenu()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        // SceneManager.LoadScene(0);
        StartCoroutine(LoadAsynchronously(1));
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        audioSource.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        audioSource.Play();
    }

    // public void SoundToggle(bool chk)
    // {
    //     if (chk)
    //     {
    //         AudioListener.volume = 0;
    //     }
    //     else
    //     {
    //         AudioListener.volume = 1;
    //     }
    //     PlayerPrefs.Save();
    // }

    // public void ResetScore()
    // {
    //     PlayerControl.ResetScore();
    //     playFabManager.SendLeaderBoard(0);
    //     SSTools.ShowMessage("Score Reset successfully!", SSTools.Position.bottom, SSTools.Time.oneSecond);
    // }

}
