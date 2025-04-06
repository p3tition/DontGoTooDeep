using UnityEngine;

public class Planet : MonoBehaviour
{
    private GameObject powerUpUI;

    void Start()
    {
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
        if (powerUpUI != null)
        {
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