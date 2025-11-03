using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        scoreText.text = "Score" + lastScore.ToString();
    }
}
