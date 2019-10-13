using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private string curScore;

    public int PlayerOneScore = 0;
    public int PlayerTwoScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        curScore = PlayerOneScore + " - " + PlayerTwoScore;

        scoreText.text = curScore;
    }

    public void updateScoreboard(string score)
    {
        scoreText.text = score;
    }
}
