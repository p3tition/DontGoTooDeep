using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Player player;
    private GameObject powerUpUI;
    public static PowerUpManager Instance { get; private set; }
    
    public float accuracyMultiplier = 1f;
    public float handlingMultiplier = 1f;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        player = FindFirstObjectByType<Player>();
    }

    public void Start()
    {
        powerUpUI = FindInactiveObjectByTag("PowerUpUI");
        powerUpUI.SetActive(false);
    }
    
    public void IncreaseSignalAccuracy()
    {
        accuracyMultiplier *= 0.5f;
        powerUpUI.SetActive(false);
        player.UnPause();
    }
    
    public void ActivateShield()
    {
        player.ActivateShield();
        powerUpUI.SetActive(false);
        player.UnPause();
    }
    
    public void PointsBonus()
    {
        player.AddBonus(250);
        powerUpUI.SetActive(false);
        player.UnPause();
    }
    
    public void IncreaseSignalHandling()
    {
        handlingMultiplier *= 1.25f;
        powerUpUI.SetActive(false);
        player.UnPause();
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