using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeMovement : MonoBehaviour
{
    private Vector2 direction;

    private List<Transform> snakeSegments;
    [SerializeField] private Transform segment;

    [SerializeField] private int initialSize;
    [SerializeField] private int totalLength;

    public int score = 0;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private ScoreBoostFood scoreBoostFoodScript;
    [SerializeField] private ShortenFood shortenFoodScript;
    [SerializeField] private TimeFood timeFoodScript;

    private float initialFixedTimestep;

    [SerializeField] private GameObject gameOverMenu;

    //sounds
    [SerializeField] private AudioSource normalFoodSound;
    [SerializeField] private AudioSource scoreBoostFoodSound;
    [SerializeField] private AudioSource shortenFoodSound;
    [SerializeField] private AudioSource timeFoodSound;
    [SerializeField] private AudioSource hitSound;

    //instructions
    [SerializeField] private GameObject instruction;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        if (PlayerPrefs.GetInt("ClassicIns", 0) == 0)
        {
            StartCoroutine(ShowInstructions());
            PlayerPrefs.SetInt("ClassicIns", 1);
        } 

        gameOverMenu.SetActive(false);
        initialFixedTimestep = Time.fixedDeltaTime;

        direction = Vector2.right;

        snakeSegments = new List<Transform>();
        snakeSegments.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            snakeSegments.Add(Instantiate(segment));
        }

        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highscore.ToString();

        totalLength = initialSize;
    }

    private void Update()
    {
        totalLength = snakeSegments.Count;
        scoreText.text = score.ToString();
        SetHighScore();

        
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
        } else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
    }

    private void FixedUpdate()
    {
        for (int i= snakeSegments.Count - 1; i > 0; i--)
        {
            snakeSegments[i].position = snakeSegments[i - 1].position;
        }
        
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0.0f
            );

        transform.eulerAngles = new Vector3(0, 0, RotateSnake(direction) + 90);
    }

    private void Grow ()
    {
        Transform newSegment = Instantiate(segment);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            normalFoodSound.Play();
            score += 10;
        } else if (other.CompareTag("Obstacle")) {
            //GameOver();
            Freeze();
            hitSound.Play();
        } else if (other.CompareTag("ScoreBoostFood"))
        {
            Grow();
            score += 50;
            scoreBoostFoodScript.foodPresent = false;
            scoreBoostFoodSound.Play();
            Destroy(GameObject.FindGameObjectWithTag("ScoreBoostFood"));
        } else if (other.CompareTag("ShortenFood"))
        {
            ShortenFood();
            score += 10;
            shortenFoodSound.Play();
            shortenFoodScript.foodPresent = false;
            Destroy(GameObject.FindGameObjectWithTag("ShortenFood"));
        } else if (other.CompareTag("TimeFood"))
        {
            StartCoroutine(TimeFood());
            score += 10;
            timeFoodSound.Play();
            timeFoodScript.foodPresent = false;
            Destroy(GameObject.FindGameObjectWithTag("TimeFood"));
        }
    }

    private void Freeze ()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetHighScore ()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }   
    }

    private void ShortenFood ()
    {
        
        int currentLength = snakeSegments.Count;

        GameObject segment1 = snakeSegments[currentLength - 1].gameObject;
        snakeSegments.RemoveAt(currentLength - 1);
        Destroy(segment1);

        GameObject segment2 = snakeSegments[currentLength - 2].gameObject;
        snakeSegments.RemoveAt(currentLength - 2);
        Destroy(segment2);

        GameObject segment3 = snakeSegments[currentLength - 3].gameObject;
        snakeSegments.RemoveAt(currentLength - 3);
        Destroy(segment3);
    }

    IEnumerator TimeFood ()
    {
        Time.fixedDeltaTime = initialFixedTimestep + 0.05f;
        yield return new WaitForSeconds(5);
        Time.fixedDeltaTime = initialFixedTimestep;
    }

    private float RotateSnake (Vector2 dir)
    {
        float n = Mathf.Atan2(Mathf.Round(dir.y), Mathf.Round(dir.x)) * Mathf.Rad2Deg;
        if (n<0)
        {
            n += 360;
        }

        return n;
    }

    IEnumerator ShowInstructions ()
    {
        instruction.SetActive(true);
        yield return new WaitForSeconds(10.2f);
        instruction.SetActive(false);
    }  

}
