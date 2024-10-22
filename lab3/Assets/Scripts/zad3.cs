using UnityEngine;

public class SquareMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector3[] points;
    private int currentPoint = 0;

    void Start()
    {
        points = new Vector3[4];
        points[0] = transform.position;
        points[1] = points[0] + new Vector3(10, 0, 0);
        points[2] = points[1] + new Vector3(0, 0, 10);
        points[3] = points[2] + new Vector3(-10, 0, 0);

        RotateTowardsNextPoint();
    }

    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, points[currentPoint], speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, points[currentPoint]) < 0.01f)
        {
            currentPoint = (currentPoint + 1) % points.Length;

            RotateTowardsNextPoint();
        }
    }

    void RotateTowardsNextPoint()
    {
        Vector3 direction = points[currentPoint] - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
