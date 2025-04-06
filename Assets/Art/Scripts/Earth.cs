using UnityEngine;

public class Earth : MonoBehaviour
{
    [SerializeField] private float speed = 90f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
