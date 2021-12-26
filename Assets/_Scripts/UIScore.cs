using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        ScoreSystem.OnScoreChanged += UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        _text.SetText(score.ToString());
    }
}