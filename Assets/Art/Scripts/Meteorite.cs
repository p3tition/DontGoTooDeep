using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField]private float minSpeed = 3f;
    [SerializeField]private float maxSpeed = 5f;
    private Vector2 direction;
    private float speed;
    private Vector2 velocity;
    void Start()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        speed = Random.Range(minSpeed, maxSpeed);
        velocity = direction * speed;
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
