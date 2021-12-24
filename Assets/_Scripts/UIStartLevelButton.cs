using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }

    private void OnValidate()
    {
        //If there isn't a TMP object, then the name won't be set, otherwise, name the object the Level Name!
        GetComponentInChildren<TMP_Text>()?.SetText(_levelName);
    }
}
