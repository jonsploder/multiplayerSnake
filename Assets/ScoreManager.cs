using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    private TextMeshProUGUI scoreCounterText;

    private void Start()
    {
        scoreCounterText = GetComponent<TextMeshProUGUI>();
        scoreCounterText.text = "Score: " + score;
    }

    public void incrementScore()
    {
        score += 1;
        scoreCounterText.text = "Score: " + score;
    }
}
