using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private GameObject[] points;
    [SerializeField] private float moveSpeed;

    private int pointIndex;
    public void Start()
    {
        transform.position = points[pointIndex].transform.position;
    }

    public void Update()
    {
        if (pointIndex <= points.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == points[pointIndex].transform.position)
            {
                pointIndex++;
            }

            if (pointIndex == points.Length - 1)
            {
                pointIndex = 0;
            }
        }
    }
}
