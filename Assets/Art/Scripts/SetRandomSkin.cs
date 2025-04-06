using UnityEngine;

public class SetRandomSkin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            int randomIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randomIndex];
        }
    }
}