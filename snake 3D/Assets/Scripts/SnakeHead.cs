using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeHead : MonoBehaviour
{
    public float moveInterval = 0.2f;
    public GameObject segmentPrefab;
    public List<Transform> segments = new List<Transform>();
    public Transform segmentsParent;

    private Vector3 direction = Vector3.forward;
    private Grid grid;
    private float nodeDiameter;
    private int growPending = 0;
    public int fruitsEaten = 0;

    void Start()
    {
        Time.timeScale = 1f;
        grid = FindObjectOfType<Grid>();
        nodeDiameter = grid.nodeRadius * 2f;

        if (PlayerPrefs.HasKey("MoveInterval"))
        {
            moveInterval = PlayerPrefs.GetFloat("MoveInterval");
        }

        direction = Vector3.forward;
        direction.y = 0f;
        direction.Normalize();

        segments.Add(transform);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        StartCoroutine(MoveRoutine());
    }

    void Update()
    {
        HandleInput();

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Quaternion.Euler(0, -90, 0) * direction;
            direction.y = 0f;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Quaternion.Euler(0, 90, 0) * direction;
            direction.y = 0f;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + direction * nodeDiameter;
            targetPosition.y = 0.5f;

            Node newNode = grid.NodeFromWorldPoint(targetPosition);
            if (!newNode.walkable)
            {
                GameOver();
                yield break;
            }

            List<Vector3> previousPositions = new List<Vector3>();
            foreach (Transform segment in segments)
            {
                previousPositions.Add(segment.position);
            }

            float elapsedTime = 0f;
            float moveDuration = moveInterval;

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / moveDuration);

                transform.position = Vector3.Lerp(startPosition, newNode.worldPosition, t);
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

                for (int i = 1; i < segments.Count; i++)
                {
                    segments[i].position = Vector3.Lerp(previousPositions[i], previousPositions[i - 1], t);
                    segments[i].position = new Vector3(segments[i].position.x, 0.5f, segments[i].position.z);
                }

                yield return null;
            }

            transform.position = newNode.worldPosition;
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

            for (int i = 1; i < segments.Count; i++)
            {
                segments[i].position = previousPositions[i - 1];
                segments[i].position = new Vector3(segments[i].position.x, 0.5f, segments[i].position.z);
            }

            direction.y = 0f;
            direction.Normalize();

            float finalAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, finalAngle, 0f);

            CheckSelfCollision();

            if (growPending > 0)
            {
                Grow();
                growPending--;
            }

            yield return null;
        }
    }

    void CheckSelfCollision()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, segments[i].position);

            if (distance < nodeDiameter / 2f)
            {
                GameOver();
                break;
            }
        }
    }

    void Grow()
    {
        Transform lastSegment = segments[segments.Count - 1];
        Vector3 newSegmentPosition = lastSegment.position;
        Vector3 segmentDirection = Vector3.zero;

        if (segments.Count >= 2)
        {
            segmentDirection = (lastSegment.position - segments[segments.Count - 2].position).normalized;
        }
        else
        {
            segmentDirection = -direction;
        }

        segmentDirection.y = 0f;
        segmentDirection.Normalize();

        newSegmentPosition += segmentDirection * nodeDiameter;
        newSegmentPosition.y = 0.5f;

        GameObject newSegment = Instantiate(segmentPrefab, newSegmentPosition, Quaternion.identity);

        if (segmentsParent != null)
        {
            newSegment.transform.SetParent(segmentsParent, true);
        }

        segments.Add(newSegment.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit") && CompareTag("eyes"))
        {
            Destroy(other.gameObject);
            growPending++;
            fruitsEaten++;

            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateFruitCounter(fruitsEaten);
            }

            FruitSpawner spawner = FindObjectOfType<FruitSpawner>();
            if (spawner != null)
            {
                spawner.SpawnFruit();
            }
            else
            {
                Debug.LogError("Nie znaleziono obiektu FruitSpawner w scenie!");
            }
        }
    }

    void GameOver()
    {
        StopAllCoroutines();

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowGameOverScreen();
        }
    }
}
