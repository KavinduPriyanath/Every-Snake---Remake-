using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortenFood3 : MonoBehaviour
{
    private FreeStyleSnake snakeMovementScript;

    [SerializeField] private GameObject shortenFoodPrefab;
    [SerializeField] private BoxCollider2D gridArea;

    public bool foodPresent;

    private void Start()
    {
        foodPresent = false;
        snakeMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<FreeStyleSnake>();
    }

    private void Update()
    {
        if (snakeMovementScript.score != 0 && (snakeMovementScript.score % 500 == 0) && !foodPresent)
        {
            GenerateFood();
            foodPresent = true;
        }

    }

    private void GenerateFood()
    {
        Bounds bounds = gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        Instantiate(shortenFoodPrefab, new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f), Quaternion.identity);

    }
}
