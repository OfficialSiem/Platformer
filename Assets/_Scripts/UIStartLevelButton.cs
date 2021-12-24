using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;

    //Way to grab the LevelName without having access to the actual level name!
    public string LevelName => _levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }
}
