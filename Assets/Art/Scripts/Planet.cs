using UnityEngine;

public class Planet : MonoBehaviour
{
    private GameObject powerUpUI;
    private Player player;
    public bool isActive = true;
    void Start()
    {
        player = FindInactiveObjectByTag("Player").GetComponent<Player>();
        powerUpUI = FindInactiveObjectByTag("PowerUpUI");
        if (powerUpUI != null)
        {
            powerUpUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PowerUpUI not found in scene.");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            SoundManager.Instance.PlayPowerUpSound();
            isActive = false;
            player.Pause();
            powerUpUI.SetActive(true);
        }
    }

    GameObject FindInactiveObjectByTag(string tag)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.hideFlags == HideFlags.None && t.CompareTag(tag))
            {
                return t.gameObject;
            }
        }
        return null;
    }
}