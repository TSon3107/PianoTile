using UnityEngine;
using System.Collections.Generic;

public class Perfectline : MonoBehaviour
{
    // Danh sách các Tile hiện đang nằm trong vùng Perfect
    private List<GameObject> tilesInPerfectArea = new List<GameObject>();
    
    // Tạo tham chiếu tĩnh (static) để script xử lý input có thể truy cập dễ dàng
    public static Perfectline Instance; 

    void Awake()
    {
        // Khởi tạo Singleton pattern (giúp gọi Perfectline.Instance.HandleHit() dễ dàng)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Xử lý khi Tile đi vào vùng Perfect
    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đó có phải là Tile (dùng Tag "Tile" hoặc Component Tile Script)
        if (other.CompareTag("Tile")) 
        {
            // Thêm Tile vào danh sách
            tilesInPerfectArea.Add(other.gameObject);
        }
    }

    // Xử lý khi Tile đi ra khỏi vùng Perfect (Miss)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            // Nếu Tile đi ra mà chưa được bấm
            if (tilesInPerfectArea.Contains(other.gameObject))
            {
                // Loại bỏ khỏi danh sách
                tilesInPerfectArea.Remove(other.gameObject);
                
                // Xử lý Miss (gọi hàm trừ điểm hoặc báo Miss)
                Debug.Log("Miss! Tile went past the line.");
                // GameManager.Instance.ScoreMiss(); 
            }
        }
    }

    // Hàm này được gọi từ script xử lý Input khi người chơi CHẠM/CLICK
    public void HandleInput()
    {
        if (tilesInPerfectArea.Count > 0)
        {
            // Lấy Tile đang ở vị trí Perfect (luôn là tile đầu tiên)
            GameObject perfectTile = tilesInPerfectArea[0];
            
            // --- LOGIC PERFECT HIT ---
            
            // 1. Kích hoạt hiệu ứng Perfect
            TriggerPerfectEffect(perfectTile.transform.position); 
            
            // 2. Tăng điểm (nếu có GameManager)
            // GameManager.Instance.ScorePerfect();
            
            // 3. Xử lý Tile: loại bỏ khỏi list và hủy
            tilesInPerfectArea.RemoveAt(0);
            Destroy(perfectTile);
        }
        else
        {
            // Chạm trượt (không có Tile nào trong vùng Perfect)
            Debug.Log("Bad Hit! No tile to hit.");
            // GameManager.Instance.ScoreMiss();
        }
    }

    // Kích hoạt hiệu ứng (Text Pop-up/Particle)
    private void TriggerPerfectEffect(Vector3 hitPosition)
    {
        // TODO: Viết code để Instantiate Prefab chữ "PERFECT!" hoặc Particle System tại hitPosition
        Debug.Log("PERFECT!"); 
    }
}