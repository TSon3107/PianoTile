using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Danh sách tất cả Canvas/Panel")]
    public GameObject[] allPanels;

    void Awake()
    {
        if (allPanels != null)
        {
            foreach (var panel in allPanels)
                if (panel != null)
                    panel.SetActive(false); // ẩn tất cả panel lúc đầu
        }
    }

    // =========================
    // Toggle Canvas/Panel
    // =========================
    public void ToggleCanvas(GameObject canvas)
    {
        if (canvas == null) return;

        bool isActive = canvas.activeSelf;

        foreach (var panel in allPanels)
            if (panel != null)
                panel.SetActive(false);

        if (!isActive)
        {
            canvas.SetActive(true);

            RectTransform rect = canvas.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.localPosition = Vector3.zero;
                rect.localScale = Vector3.one;
            }
        }
    }

    // =========================
    // Scene Management
    // =========================
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();
    }

    // =========================
    // Retry / Continue
    // =========================
    public void RetryCurrentLevel()
    {
        if (LevelManager.selectedLevelData != null)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(
                FindObjectOfType<LevelManager>()?.gameplaySceneName ?? "GameScene"
            );
        }
        else
        {
            Debug.LogWarning("⚠️ Không có LevelData đã chọn!");
        }
    }

    public void ContinueLevel() => RetryCurrentLevel();
}
