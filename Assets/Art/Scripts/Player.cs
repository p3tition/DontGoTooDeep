using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;  // For TextMeshPro

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float upwardSpeed = 5f;
    [SerializeField] public float horizontalSpeed = 0f;
    [SerializeField] private float tiltAngle = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float cameraOffset = 0.2f;
    [SerializeField] private float minXBound = -8f;
    [SerializeField] private float maxXBound = 8f;
    
    private int score = 0;
    private Vector3 lastPosition;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private Camera mainCamera;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bonusScoreText;
    public GameObject gameOverUI;
    
    private float bonusScoreValue = 0;
    private float bonusScoreDuration = 1f;
    private float bonusScoreTimer = 0f;
    public bool isDead = false;
    public bool isPaused = false;
    private Vector3 spawnPosition;
    private BackgroundSpawner background;
    private float startOccuracy = 1f;
    void Start()
    {
        background = FindFirstObjectByType<BackgroundSpawner>();
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        mainCamera = Camera.main;
        
        lastPosition = transform.position;

        bonusScoreText.gameObject.SetActive(false);
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true;
        }

        if (isMoving && !isDead && !isPaused)
        {
            StartRun();
            HandleRotation();
            UpdateScore();
            LockCamera();
        }

        if (bonusScoreValue > 0  && !isDead && !isPaused)
        {
            bonusScoreTimer += Time.deltaTime;
            if (bonusScoreTimer >= bonusScoreDuration)
            {
                bonusScoreValue = 0;
                bonusScoreText.gameObject.SetActive(false);
            }
            else
            {
                bonusScoreText.text = "+" + Mathf.RoundToInt(bonusScoreValue).ToString();
                UpdateBonusTextColor();
            }
        }
    }

    private void StartRun()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 newVelocity = new Vector2(horizontalInput * horizontalSpeed * PowerUpManager.Instance.handlingMultiplier, upwardSpeed);
        rb.linearVelocity = newVelocity;

        Vector3 scale = gameObject.transform.localScale;

        float smoothedMultiplier = Mathf.Lerp(1f, PowerUpManager.Instance.accuracyMultiplier, 0.5f);
    
        float newScaleX = startOccuracy * Mathf.Sqrt(smoothedMultiplier);

        float maxScale = 10f;
        newScaleX = Mathf.Clamp(newScaleX, 0.1f, maxScale);

        scale.x = newScaleX;
        gameObject.transform.localScale = scale;

    
    }


    private void HandleRotation()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float targetZRotation = -horizontalInput * tiltAngle;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }

    private void UpdateScore()
    {
        float distanceMoved = Vector3.Distance(lastPosition, transform.position);

        if (distanceMoved >= 2f)
        {
            score += 1;
            lastPosition = transform.position;
        }

        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }

    private void LockCamera()
    {
        if (mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            cameraPosition.y = transform.position.y - cameraOffset;

            float cameraX = Mathf.Clamp(transform.position.x, minXBound, maxXBound);

            cameraPosition.x = cameraX;

            mainCamera.transform.position = cameraPosition;
        }
    }
    
    public void AddBackgroundBonus()
    {
        float distanceMoved = Vector3.Distance(lastPosition, transform.position);
        float bonus = Mathf.Round(distanceMoved * 10/10) * 100;
        if (bonus > 0)
        {
            bonusScoreValue = bonus;
            score += Mathf.RoundToInt(bonusScoreValue);
            bonusScoreTimer = 0f;
            bonusScoreText.gameObject.SetActive(true);
        }
    }
    public void AddBonus(int bonus)
    {
        bonusScoreValue = bonus;
        score += Mathf.RoundToInt(bonusScoreValue);
        bonusScoreTimer = 0f;
        bonusScoreText.gameObject.SetActive(true);
    }
    private void UpdateBonusTextColor()
    {
        float colorFactor = Mathf.Clamp01(bonusScoreValue / 1000f);
        Color bonusColor = new Color(1f, 1f - colorFactor * 0.5f, 0.886f);

        bonusScoreText.color = bonusColor;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<FollowPath>() && !isPaused)
        {
            PlayerDeath();
        }
    }
    public void PlayerDeath()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isDead = true;
        scoreText.gameObject.SetActive(false);
        bonusScoreText.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void Respawn()
    {
        scoreText.gameObject.SetActive(true);
        gameOverUI.SetActive(false);
        gameObject.transform.position = spawnPosition;
        score = 0;
        background.InitBackground();
        lastPosition = transform.position;
        bonusScoreValue = 0;
        bonusScoreTimer = 0f;
        isDead = false;
    }
    public void Pause()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
    public void UnPause()
    {
        isPaused = false;
    }

    public void ActivateShield()
    {
        
    }
}
