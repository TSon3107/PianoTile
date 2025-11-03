// Tile.cs
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float moveSpeed;
    public float tileDuration = 0f;
    public bool isCorrect = true; // true = đen (đúng), false = trắng (sai)

    public const float TARGET_Y = -2.5f;
    private const float GAMEOVER_Y_BOUNDARY = -4f;

    private const float PERFECT_OFFSET = 0.1f;
    private const float GREAT_OFFSET = 0.3f;

    public float TargetY => TARGET_Y;

    public void SetSpeed(float s) => moveSpeed = s;
    public void SetDuration(float duration) { tileDuration = duration; }

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < GAMEOVER_Y_BOUNDARY)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
                GameManager.Instance.GameOver();

            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        float offset = Mathf.Abs(transform.position.y - TARGET_Y);
        if (offset > GREAT_OFFSET) return;

        string hitQuality = "";
        int scoreValue = 0;

        if (isCorrect)
        {
            if (offset <= PERFECT_OFFSET)
            {
                hitQuality = "PERFECT!";
                scoreValue = 3;
            }
            else
            {
                hitQuality = "GREAT!";
                scoreValue = 1;
            }
        }
        else
        {
            hitQuality = "WRONG!";
            scoreValue = -2; // trừ điểm nếu tile trắng (có thể chỉnh)
        }

        GameManager.Instance?.AddScore(scoreValue);
        GameManager.Instance?.TriggerHitEffect(hitQuality, transform.position);

        Destroy(gameObject);
    }
}
