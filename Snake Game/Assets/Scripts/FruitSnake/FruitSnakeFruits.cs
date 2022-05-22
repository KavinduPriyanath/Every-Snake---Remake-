using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSnakeFruits : MonoBehaviour
{
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private FruitSnakeMoves fruitSnakeScript;


    private void Start()
    {
        RandomizePosition();
    }

    private void Update()
    {
        if (fruitSnakeScript.gotFood)
        {
            RandomizePosition();
            fruitSnakeScript.gotFood = false;
        }
    }

    private void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        int fruitNo = Random.Range(0, fruits.Length);

        GameObject fruit = Instantiate(fruits[fruitNo], new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f), Quaternion.identity);
        //Destroy(fruit);
    }
}
