using System.Collections;
using UnityEngine;

public class Border : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine colorChangeCoroutine;

    [SerializeField] private Color targetColor = new Color(1f, 0f, 0f, 1f);
    [SerializeField] private float colorChangeDuration = 0.5f;

    private bool isChanging = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (spriteRenderer != null && !isChanging)
        {
            colorChangeCoroutine = StartCoroutine(SmoothColorChange(targetColor));
        }
    }

    private IEnumerator SmoothColorChange(Color newColor)
    {
        isChanging = true;
        Color startColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < colorChangeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, newColor, elapsed / colorChangeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        isChanging = false;
        spriteRenderer.color = newColor;
    }
}
