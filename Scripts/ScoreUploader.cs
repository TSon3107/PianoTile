using Firebase.Database;
using Firebase.Auth;
using UnityEngine;

public class ScoreUploader : MonoBehaviour
{
    public static void UploadScore(int score)
    {
        if (FirebaseInit.DB == null || FirebaseInit.User == null)
        {
            Debug.LogError("❌ Firebase chưa sẵn sàng hoặc user null!");
            return;
        }

        if (LevelManager.selectedLevelData == null)
        {
            Debug.LogError("⚠️ Không có level đang chơi để lưu điểm!");
            return;
        }

        string songName = LevelManager.selectedLevelData.songName;
        string playerName = FirebaseInit.User.DisplayName ?? FirebaseInit.User.Email.Split('@')[0];

        FirebaseInit.DB
            .Child("leaderboard")
            .Child(songName)
            .Child(playerName)
            .SetValueAsync(score)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log($"✅ Ghi điểm thành công: {playerName} - {score} ({songName})");
                else
                    Debug.LogError("❌ Lỗi khi ghi điểm: " + task.Exception);
            });
    }
}
