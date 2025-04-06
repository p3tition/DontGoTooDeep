using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPrefabs;
    [SerializeField] private float backgroundHeight = 32;
    [SerializeField] private float spawnBuffer = 10f;
    [SerializeField] private GameObject border;
    private Transform playerTransform;
    private Player player;
    private float spawnHeight = 0f;
    private int backgroundIndex = 0;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        InitBackground();
    }

    public void InitBackground()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        playerTransform = player.transform;
        for (int i = 0; i < backgroundPrefabs.Length; i++)
        {
            SpawnBackground(i * backgroundHeight);
        }
    }

    void Update()
    {
        CheckBackgroundSpawn();
    }

    private void CheckBackgroundSpawn()
    {
        if (playerTransform.position.y > spawnHeight - spawnBuffer)
        {
            SpawnBackground(spawnHeight);
            spawnHeight += backgroundHeight;
        }
    }

    private void SpawnBackground(float positionY)
    {
        GameObject background = Instantiate(backgroundPrefabs[backgroundIndex], new Vector3(0f, positionY, 0f), Quaternion.identity);
        background.transform.SetParent(transform);
        GameObject borderObject = Instantiate(border, new Vector3(0f, positionY, 0f), Quaternion.identity);
        borderObject.transform.SetParent(transform);
        backgroundIndex = (backgroundIndex + 1) % backgroundPrefabs.Length;
        if (backgroundIndex == backgroundPrefabs.Length - 1 && !player.isDead)
        {
            player.AddBackgroundBonus();
        }
    }
}