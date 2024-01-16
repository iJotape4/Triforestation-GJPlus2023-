using Events;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake()
    {
        EventManager.AddListener<int>(ENUM_AnimalEvent.biomePoleOccupied, UpdateScore);
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, SentScore);
    }

    private void SentScore()
    {
        EventManager.Dispatch(ENUM_GameState.win, score);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(ENUM_AnimalEvent.biomePoleOccupied, UpdateScore);
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, SentScore);
    }

    // Update the score text smoothly
    private void UpdateScore(int earnedPoints)
    {
       StartCoroutine(UpdateTexScoreCoroutine(earnedPoints));
    }

    private IEnumerator UpdateTexScoreCoroutine(int earnedPoints)
    {
        int tempScore = score;
        score += earnedPoints;
        while (tempScore != score)
        {
            tempScore++;
            scoreText.text = tempScore.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public int GetScore()
    {
        return score;
    }
}