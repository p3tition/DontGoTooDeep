using UnityEngine;

public class AutoDestroyBelowPlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
    }

    private void Update()
    {
        if (transform.position.y < player.position.y - 100f)
        {
            Destroy(gameObject);
        }
    }
}