using UnityEngine;
using TMPro;

public class PopUpFX : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 3f;
    public float lifeTime = 0.5f;

    private TMP_Text tmpText;
    private Color originalColor;
    private float timer = 0f;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        originalColor = tmpText.color;
    }

    void OnEnable()
    {
        // Reset lại mỗi lần bật
        timer = 0f;
        tmpText.color = originalColor;
    }

    void Update()
    {
        if (tmpText == null) return;

        timer += Time.deltaTime;

        // Bay lên
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Giảm alpha
        Color c = tmpText.color;
        c.a = Mathf.Lerp(originalColor.a, 0, timer / lifeTime);
        tmpText.color = c;

        // Khi hết thời gian → ẩn
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
