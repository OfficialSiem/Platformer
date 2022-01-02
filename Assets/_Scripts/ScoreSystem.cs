using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static int Score { get; private set; }

    static int _highScore;

    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
    }

    public static void Add(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
        Debug.Log($"Score = {Score}");

        if(Score > _highScore)
        {
            _highScore = Score;
            Debug.Log("high Score " + _highScore);

            PlayerPrefs.SetInt("HighScore", _highScore);
        }
    }

    internal static void Add(object pointsEarned)
    {
        throw new NotImplementedException();
    }
}
