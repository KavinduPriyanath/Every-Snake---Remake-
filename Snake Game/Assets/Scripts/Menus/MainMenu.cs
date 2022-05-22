using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameModes;

    [SerializeField] private GameObject options;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject highScores;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject home;

    private bool showHome;

    //Highscore References
    [SerializeField] private TMP_Text classicHigh;
    [SerializeField] private TMP_Text freeHigh;
    [SerializeField] private TMP_Text fruitHigh;

    private void Start()
    {
        showHome = false;

        options.SetActive(false);
        credits.SetActive(false);
        exit.SetActive(false);
        highScores.SetActive(false);
    }

    private void Update()
    {
        if (showHome)
        {
            home.SetActive(true);
        } else
        {
            home.SetActive(false);
        }

        classicHigh.text = (PlayerPrefs.GetInt("HighScore", 0)).ToString();
        freeHigh.text = (PlayerPrefs.GetInt("FreeHighScore", 0).ToString());
        fruitHigh.text = (PlayerPrefs.GetInt("FruitSnakeHighScore", 0).ToString());
    }


    public void ClassicGame ()
    {
        SceneManager.LoadScene("Game1");
    }

    public void FreeStyle ()
    {
        SceneManager.LoadScene("Game3");
    }

    public void FruitSnake ()
    {
        SceneManager.LoadScene("FruitSnake");
    }

    public void OptionModes ()
    {
        showHome = true;

        options.SetActive(true);

        gameModes.SetActive(false);
        credits.SetActive(false);
        exit.SetActive(false);
        highScores.SetActive(false);

    }

    public void Credits ()
    {
        showHome = true;

        credits.SetActive(true);

        gameModes.SetActive(false);
        options.SetActive(false);
        exit.SetActive(false);
        highScores.SetActive(false);
    }

    public void HighScore ()
    {
        showHome = true;

        highScores.SetActive(true);

        gameModes.SetActive(false);
        credits.SetActive(false);
        exit.SetActive(false);
        options.SetActive(false);
    }

    public void Power ()
    {
        showHome = true;

        exit.SetActive(true);

        gameModes.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        highScores.SetActive(false);
    }

    public void Home ()
    {
        showHome = false;

        gameModes.SetActive(true);

        exit.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        highScores.SetActive(false);
    }

    public void Slug ()
    {
        Time.fixedDeltaTime = 0.08f;
    }

    public void Worm ()
    {
        Time.fixedDeltaTime = 0.06f;
    }

    public void Python ()
    {
        Time.fixedDeltaTime = 0.04f;
    }

    public void Leave ()
    {
        Application.Quit();
    }

    public void ResetButton()
    {
        PlayerPrefs.DeleteAll();
    }
}
