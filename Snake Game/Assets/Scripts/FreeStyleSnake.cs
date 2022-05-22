using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeStyleSnake : MonoBehaviour
{
    public float speed = 3f;
    public float rotationalSpeed = 200f;

    float velX = 0f;

    private List<Transform> snakeSegments;
    [SerializeField] private Transform segment;
    [SerializeField] private int initialSize;

    public int score = 0;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    [SerializeField]
    private int currentLength;

    [SerializeField] private GameObject gameOverMenu;

    //sounds
    [SerializeField] private AudioSource foodSound;
    [SerializeField] private AudioSource hitSound;

    [SerializeField] private GameObject instruction;

    private void Start()
    {
        gameOverMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        if (PlayerPrefs.GetInt("FreeIns", 0) == 0)
        {
            StartCoroutine(ShowInstructions());
            PlayerPrefs.SetInt("FreeIns", 1);
        }

        snakeSegments = new List<Transform>();
        snakeSegments.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            snakeSegments.Add(Instantiate(segment));
        }

        int highscore = PlayerPrefs.GetInt("FreeHighScore", 0);
        highScoreText.text = highscore.ToString();
    }

    private void Update()
    {
        velX = Input.GetAxisRaw("Horizontal");
        scoreText.text = score.ToString();
        SetHighScore();
        currentLength = snakeSegments.Count;
    }

    private void FixedUpdate()
    {
        for (int i = snakeSegments.Count - 1; i > 0; i--)
        {
            snakeSegments[i].position = snakeSegments[i - 1].position;
            snakeSegments[i].rotation = snakeSegments[i - 1].rotation;
        }

        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime, Space.Self);
        transform.Rotate(Vector3.forward * -velX * rotationalSpeed * Time.fixedDeltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            foodSound.Play();
            score += 10;
        } else if (other.CompareTag("Obstacle"))
        {
            Freeze();
            hitSound.Play();
        }

    }

    private void Freeze()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetHighScore()
    {
        if (score > PlayerPrefs.GetInt("FreeHighScore", 0))
        {
            PlayerPrefs.SetInt("FreeHighScore", score);
            highScoreText.text = PlayerPrefs.GetInt("FreeHighScore", 0).ToString();
        }
    }

    private void Grow()
    {
        Transform newSegment = Instantiate(segment);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    IEnumerator ShowInstructions()
    {
        instruction.SetActive(true);
        yield return new WaitForSeconds(10.2f);
        instruction.SetActive(false);
    }

}
