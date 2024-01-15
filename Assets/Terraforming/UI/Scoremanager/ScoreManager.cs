using Events;
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake()
    {
        EventManager.AddListener<int>(ENUM_AnimalEvent.biomePoleOccupied, UpdateScore);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(ENUM_AnimalEvent.biomePoleOccupied, UpdateScore);
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
}