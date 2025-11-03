using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;   // Nơi chứa các dòng
    public GameObject rowPrefab;      // Prefab dòng leaderboard
    public int maxTop = 10;

    private string currentSongName;

    void Start()
    {
        if (LevelManager.selectedLevelData != null)
        {
            currentSongName = LevelManager.selectedLevelData.songName;
            Debug.Log($"📊 Đang tải leaderboard cho bài: {currentSongName}");
            LoadLeaderboard();
        }
        else
        {
            Debug.LogWarning("⚠️ Không có level hiện tại (LevelManager.selectedLevelData = null)!");
        }
    }

    public void LoadLeaderboard()
    {
        if (string.IsNullOrEmpty(currentSongName))
        {
            Debug.LogWarning("⚠️ Không có bài hát để tải leaderboard!");
            return;
        }

        FirebaseInit.DB
            .Child("leaderboard")
            .Child(currentSongName)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (!task.IsCompleted || task.Result == null)
                {
                    Debug.LogWarning("⚠️ Không có dữ liệu leaderboard!");
                    return;
                }

                var snapshot = task.Result;
                var scores = new List<(string name, int score)>();

                foreach (var userNode in snapshot.Children)
                {
                    string playerName = userNode.Key;
                    int playerScore = 0;

                    if (userNode.HasChild("score"))
                        int.TryParse(userNode.Child("score").Value.ToString(), out playerScore);
                    else if (int.TryParse(userNode.Value?.ToString(), out int val))
                        playerScore = val;

                    scores.Add((playerName, playerScore));
                }

                var sorted = scores.OrderByDescending(s => s.score).Take(maxTop).ToList();

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    foreach (Transform child in contentParent)
                        Destroy(child.gameObject);

                    for (int i = 0; i < sorted.Count; i++)
                    {
                        var row = Instantiate(rowPrefab, contentParent);
                        row.transform.localScale = Vector3.one;

                        row.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = $"{i + 1}";
                        row.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = sorted[i].name;
                        row.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = sorted[i].score.ToString();
                    }
                });
            });
    }
}
