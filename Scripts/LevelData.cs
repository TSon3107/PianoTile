using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Rhythm/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("THÔNG TIN BÀI HÁT")]
    [Tooltip("Tên bài hát hiển thị cho người chơi.")]
    public string songName;

    [Tooltip("Tên nghệ sĩ / tác giả bài hát.")]
    public string artistName;

    [Tooltip("Độ khó (ví dụ: Easy / Normal / Hard).")]
    public string difficulty;

    [Tooltip("Ảnh thumbnail của bài hát (hiển thị trong menu).")]
    public Sprite thumbnail;

    [Tooltip("File nhạc (.mp3 hoặc .wav) của bài hát.")]
    public AudioClip songAudio;

    [System.Serializable]
    public class ManualNoteData
    {
        [Tooltip("Thời điểm (giây) tile cần được spawn, tính từ đầu bài hát.")]
        public float timeToSpawn;

        [Tooltip("Cột (Lane) mà tile sẽ xuất hiện: 0 (Trái) đến 3 (Phải).")]
        [Range(0, 3)]
        public int laneIndex;

        [Tooltip("Độ dài (giây) của nốt. 0 cho nốt thường, > 0 cho nốt giữ (hold note).")]
        public float duration;
    }

    [Header("Danh sách nốt nhạc của bài hát")]
    public List<ManualNoteData> notes = new List<ManualNoteData>();
}
