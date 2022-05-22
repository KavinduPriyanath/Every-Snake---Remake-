using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    [SerializeField] private FruitSnakeMoves fruitSnakeMoveScript;

    public int secondsLeft = 30;
    private bool takingAway = false;
    private bool isPlaying;

    private void Start()
    {
        isPlaying = false;
        timer.text = "00:" + secondsLeft;
    }

    private void Update()
    {
        if (!takingAway && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        } else if (secondsLeft <= 0)
        {
            fruitSnakeMoveScript.Freeze();
            if (!isPlaying)
            {
                fruitSnakeMoveScript.timeOverSound.Play();
                isPlaying = true;
            }
            
            fruitSnakeMoveScript.gameOverText.text = "Time Over";
        }
    }

    IEnumerator TimerTake ()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft < 10)
        {
            timer.text = "00:0" + secondsLeft;
        } else
        {
            timer.text = "00:" + secondsLeft;
        }
        takingAway = false;
    }
}
