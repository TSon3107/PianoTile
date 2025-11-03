using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Normal,
    Hard,
    Easy,
    Custom,
    Endless,
    Random
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public static LevelData selectedLevelData; // Bài đang chơi
    public static GameMode selectedMode;       // Mode hiện tại
    public static bool isEndlessMode = false;  // Cho script cũ dùng lại

    [Header("Cấu hình Level")]
    public LevelData[] availableLevels;

    [Tooltip("Tên scene gameplay")]
    public string gameplaySceneName = "GameScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Load bài theo mode
    /// </summary>
    public void LoadSelectedLevel(LevelData level, GameMode mode)
    {
        if (level == null)
        {
            Debug.LogError("⚠️ LevelData không hợp lệ!");
            return;
        }

        selectedLevelData = level;
        selectedMode = mode;
        isEndlessMode = (mode == GameMode.Endless);

        Time.timeScale = 1f;

        Debug.Log($"🟢 Load bài {level.name} | Mode: {mode} | Endless: {isEndlessMode}");
        SceneManager.LoadScene(gameplaySceneName);
    }

    // --- Các hàm chọn level cũ ---
    public void SelectAndLoadLevel(int index)
    {
        if (availableLevels == null || index < 0 || index >= availableLevels.Length)
        {
            Debug.LogError("⚠️ LevelIndex không hợp lệ!");
            return;
        }
        LoadSelectedLevel(availableLevels[index], GameMode.Normal);
    }

    public void SelectAndLoadEndless(int index)
    {
        if (availableLevels == null || index < 0 || index >= availableLevels.Length)
        {
            Debug.LogError("⚠️ LevelIndex không hợp lệ cho Endless!");
            return;
        }
        LoadSelectedLevel(availableLevels[index], GameMode.Endless);
    }
}
