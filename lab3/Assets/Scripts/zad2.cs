using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public float speed = 2f; 

    private Vector3 startPosition; 
    private int direction = 1; 

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        
        float distance = Mathf.Abs(transform.position.x - startPosition.x);

        
        if (distance >= 10f)
        {
            direction *= -1;
        }
    }
}
