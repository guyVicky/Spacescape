using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabManager : MonoBehaviour
{

    public GameObject rowPrefab, nameWindow, mainMenu;

    public int highScore;
    public Transform rowsParent;

    public TextMeshProUGUI nameInput, playername, gameVersion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Login());
        // GetStats();
        // Debug.Log("Application Version : " + Application.version);
        gameVersion.text = "-v" + Application.version;
    }
    #region
    IEnumerator Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);

        yield return null;
    }

    void OnLoginSuccess(LoginResult result)
    {
        // Debug.Log("Account Create/Login Successful!");
        string name = null;
        GetStats();
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            playername.text = name;
            GetLeaderBoard();
        }
        if (name == null)
        {
            nameWindow.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        // Debug.Log("Updated display name!");
        mainMenu.SetActive(true);
        nameWindow.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        // Debug.Log("Login Failed!");
        // Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "GameHighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        // Debug.Log("Successfull leaderboard sent!");
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "GameHighScore",
            StartPosition = 0,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {

        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            // Debug.Log((item.Position+1) + " | " + item.DisplayName + " | " + item.StatValue);
        }
    }
    #endregion

    #region Get Stats
    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStats(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            // Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            highScore = eachStat.Value;
            // Debug.Log(highScore);
        }

    }
    #endregion Get Stats

}
