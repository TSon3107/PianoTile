using UnityEngine;

public class SongSelectUI : MonoBehaviour
{
    [Header("Tham chiếu")]
    public GameObject modePanel;      // Panel chọn mode
    public LevelManager levelManager; // Gắn prefab LevelManager
    private int selectedLevelIndex = -1;

    void Start()
    {
        if (modePanel != null)
            modePanel.SetActive(false);
    }

    /// <summary>
    /// Khi bấm chọn bài
    /// </summary>
    public void OnSelectSong(int index)
    {
        selectedLevelIndex = index;

        if (modePanel != null)
            modePanel.SetActive(true); // hiện bảng chọn mode
    }

    /// <summary>
    /// Khi bấm nút chọn mode
    /// </summary>
    public void OnChooseMode(string modeName)
    {
        if (levelManager == null)
        {
            Debug.LogError("⚠️ Chưa gán LevelManager!");
            return;
        }

        if (selectedLevelIndex < 0 || selectedLevelIndex >= levelManager.availableLevels.Length)
        {
            Debug.LogError("⚠️ Chưa chọn bài hoặc index không hợp lệ!");
            return;
        }

        // Chuyển string sang enum GameMode, không phân biệt hoa/thường
        if (!System.Enum.TryParse(modeName, true, out GameMode mode))
        {
            Debug.LogError("⚠️ Mode không hợp lệ: " + modeName);
            return;
        }

        LevelData level = levelManager.availableLevels[selectedLevelIndex];

        // Tắt panel trước khi load
        if (modePanel != null)
            modePanel.SetActive(false);

        levelManager.LoadSelectedLevel(level, mode);
    }

    /// <summary>
    /// Đóng bảng chọn mode
    /// </summary>
    public void CloseModePanel()
    {
        if (modePanel != null)
            modePanel.SetActive(false);
    }
}
