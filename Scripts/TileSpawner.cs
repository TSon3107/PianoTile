// TileSpawner.cs
using UnityEngine;
using System.Collections;

public class TileSpawner : MonoBehaviour
{
    [Header("Prefabs & Audio")]
    public GameObject tilePrefab;
    public Transform[] spawnPoints;
    private AudioSource audioSource;

    [Header("Cấu hình")]
    public float tileSpeed = 5f;
    public float spawnInterval = 1f;

    // Random mode options
    [Header("Random Mode Settings")]
    public float randomMinInterval = 0.5f;
    public float randomMaxInterval = 1.2f;

    private void Start()
    {
        // đảm bảo có audioSource (từ inspector hoặc tạo mới)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Gán nhạc từ LevelData nếu có
        if (LevelManager.selectedLevelData != null && LevelManager.selectedLevelData.songAudio != null)
        {
            audioSource.clip = LevelManager.selectedLevelData.songAudio;
            audioSource.Play();
        }

        // Nếu spawnPoints chưa gán (ví dụ testing), tạo tạm 4 điểm
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = new Transform[4];
            for (int i = 0; i < 4; i++)
            {
                GameObject sp = new GameObject("SpawnPoint_" + i);
                sp.transform.parent = transform;
                sp.transform.position = new Vector3(-1.5f + i * 1f, 5f, 0f);
                spawnPoints[i] = sp.transform;
            }
        }

        // Start the proper spawn routine
        if (LevelManager.selectedMode == GameMode.Random)
            StartCoroutine(SpawnRandomNotes());
        else
            StartCoroutine(SpawnFromLevelData());
    }

    IEnumerator SpawnRandomNotes()
    {
        Debug.Log("🎲 Random Mode: Spawn ngẫu nhiên note đen/trắng!");

        while (audioSource != null && audioSource.isPlaying)
        {
            int lane = Random.Range(0, spawnPoints.Length);
            bool isCorrect = Random.value > 0.5f; // true = đen (đúng), false = trắng (sai)

            GameObject tile = Instantiate(tilePrefab, spawnPoints[lane].position, Quaternion.identity);
            Tile tileComp = tile.GetComponent<Tile>();

            if (tileComp != null)
            {
                tileComp.SetSpeed(tileSpeed);
                tileComp.isCorrect = isCorrect;

                // Set tile color (SpriteRenderer preferred)
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = isCorrect ? Color.black : Color.white;
                else
                {
                    Renderer r = tile.GetComponent<Renderer>();
                    if (r != null) r.material.color = isCorrect ? Color.black : Color.white;
                }
            }

            float wait = Random.Range(randomMinInterval, randomMaxInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator SpawnFromLevelData()
    {
        LevelData data = LevelManager.selectedLevelData;
        if (data == null || data.notes == null) yield break;

        Debug.Log($"🎵 Load note từ LevelData ({data.songName})");

        // We use audio time as reference if audio exists, otherwise Time.time
        float audioStart = Time.time;
        int noteIndex = 0;

        while (noteIndex < data.notes.Count)
        {
            float elapsed = (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
                ? audioSource.time
                : (Time.time - audioStart);

            var note = data.notes[noteIndex];

            if (elapsed >= note.timeToSpawn)
            {
                if (note.laneIndex >= 0 && note.laneIndex < spawnPoints.Length)
                {
                    Transform spawn = spawnPoints[note.laneIndex];
                    GameObject tile = Instantiate(tilePrefab, spawn.position, Quaternion.identity);

                    Tile comp = tile.GetComponent<Tile>();
                    if (comp != null)
                        comp.SetSpeed(tileSpeed);
                }
                noteIndex++;
            }
            yield return null;
        }
    }
}
