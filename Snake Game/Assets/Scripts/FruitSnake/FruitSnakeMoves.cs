using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitSnakeMoves : MonoBehaviour
{
    private Vector2 direction;

    private List<Transform> snakeSegments;
    [SerializeField] private Transform segment;

    [SerializeField] private int initialSize;
    [SerializeField] private int totalLength;
    [SerializeField] private Timer timerScript;

    private float initialFixedTimestep;

    public bool gotFood;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text scoreText;
    private int score;

    [SerializeField] private GameObject gameOverMenu;

    public TMP_Text gameOverText;

    //sounds
    [SerializeField] private AudioSource foodSound;
    [SerializeField] private AudioSource hitSound;
    public AudioSource timeOverSound;

    [SerializeField] private GameObject instruction;

    private void Start()
    {
        gameOverMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        if (PlayerPrefs.GetInt("FruitSnakeIns", 0) == 0)
        {
            StartCoroutine(ShowInstructions());
            PlayerPrefs.SetInt("FruitSnakeIns", 1);
        }

        initialFixedTimestep = Time.fixedDeltaTime;

        direction = Vector2.right;

        snakeSegments = new List<Transform>();
        snakeSegments.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            snakeSegments.Add(Instantiate(segment));
        }

        totalLength = initialSize;
        score = 0;

        int highscore = PlayerPrefs.GetInt("FruitSnakeHighScore", 0);
        highScoreText.text = highscore.ToString();
    }

    private void Update()
    {
        totalLength = snakeSegments.Count;
        scoreText.text = score.ToString();
        SetHighScore();


        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
    }

    private void FixedUpdate()
    {
        for (int i = snakeSegments.Count - 1; i > 0; i--)
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

    private void Grow()
    {
        Transform newSegment = Instantiate(segment);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(GameObject.FindGameObjectWithTag("Food"));
            Grow();
            foodSound.Play();
            gotFood = true;
            score += 10;
            timerScript.secondsLeft += 1;
        }
        else if (other.CompareTag("Obstacle"))
        {
            Freeze();
            hitSound.Play();
            gameOverText.text = "Too Dumb";
        }
        
    }

    public void Freeze()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private float RotateSnake(Vector2 dir)
    {
        float n = Mathf.Atan2(Mathf.Round(dir.y), Mathf.Round(dir.x)) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    private void SetHighScore()
    {
        if (score > PlayerPrefs.GetInt("FruitSnakeHighScore", 0))
        {
            PlayerPrefs.SetInt("FruitSnakeHighScore", score);
            highScoreText.text = PlayerPrefs.GetInt("FruitSnakeHighScore", 0).ToString();
        }
    }

    IEnumerator ShowInstructions()
    {
        instruction.SetActive(true);
        yield return new WaitForSeconds(10.2f);
        instruction.SetActive(false);
    }



}
