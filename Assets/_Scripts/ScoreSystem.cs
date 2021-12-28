using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    static int _score;
    static int _highScore;

    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
    }

    public static void Add(int points)
    {
        _score += points;
        OnScoreChanged?.Invoke(_score);
        Debug.Log($"Score = {_score}");

        if(_score > _highScore)
        {
            _highScore = _score;
            Debug.Log("high Score " + _highScore);

            PlayerPrefs.SetInt("HighScore", _highScore);
        }
    }
}
