using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI songNameText;

    [Header("Hit Effects")]
    public GameObject perfectTab;
    public GameObject greatTab;

    [Header("Settings")]
    public bool isPlacementMode = false; // Chỉ dùng cho chế độ editor

    private int score = 0;
    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (scoreText != null) scoreText.text = "Score: 0";
        if (perfectTab != null) perfectTab.SetActive(false);
        if (greatTab != null) greatTab.SetActive(false);
    }

    private void Start()
    {
        // ✅ Reset Time.timeScale để gameplay chạy
        Time.timeScale = 1f;

        if (LevelManager.selectedLevelData != null && songNameText != null)
            songNameText.text = "🎵 " + LevelManager.selectedLevelData.songName;
        else if (songNameText != null)
            songNameText.text = "🎵 Unknown Song";
    }

    // =========================
    // SCORE SYSTEM
    // =========================
    public void AddScore(int val)
    {
        // Không tính điểm nếu game over hoặc đang trong chế độ editor
        if (isGameOver || isPlacementMode) return;

        score += val;
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        string key = "HighScore_" + (LevelManager.selectedLevelData?.name ?? "UnknownSong");
        if (score > PlayerPrefs.GetInt(key, 0))
            PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save();
    }

    // =========================
    // HIT EFFECTS
    // =========================
    public void TriggerHitEffect(string quality, Vector3 pos)
    {
        if (isPlacementMode) return;

        GameObject prefab = quality switch
        {
            "PERFECT!" => perfectTab,
            "GREAT" => greatTab,
            _ => null
        };

        if (prefab != null)
            Instantiate(prefab, pos, Quaternion.identity).SetActive(true);
    }

    // =========================
    // GAME OVER
    // =========================
public void GameOver()
{
    // Nếu đang ở chế độ chỉnh sửa hoặc đã game over thì bỏ qua
    if (isPlacementMode || isGameOver) return;

    // 🟢 Nếu là Endless hoặc Random mode → không game over thật sự
    if (LevelManager.isEndlessMode || LevelManager.selectedMode == GameMode.Random)
    {
        Debug.Log("⚠️ Endless/Random Mode: Không Game Over, chỉ trừ điểm!");
        AddScore(-10); // trừ 10 điểm hoặc tùy chỉnh
        return;
    }

    // ⚠️ Các mode còn lại (Easy/Normal/Hard) thì game over thật
    isGameOver = true;

    PlayerPrefs.SetInt("LastScore", score);
    PlayerPrefs.SetString("LastSong", LevelManager.selectedLevelData?.name ?? "UnknownSong");
    PlayerPrefs.Save();

    // ✅ Gửi điểm lên Firebase
    ScoreUploader.UploadScore(score);

    // ✅ Đợi 1.5 giây rồi load scene "Ending"
    Invoke(nameof(LoadEndingScene), 1.5f);
}

private void LoadEndingScene()
{
    // Reset Time.timeScale trước khi load Ending
    Time.timeScale = 1f;
    SceneManager.LoadScene("Ending");
}
}
