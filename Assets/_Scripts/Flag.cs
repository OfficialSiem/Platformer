using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] string _sceneName = null;
    [SerializeField] float _loadLevelAfterHowManySeconds = 1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if(player == null)
        {
            return;
        }

        var animator = GetComponent<Animator>();
        animator.SetTrigger("RaiseFlag");
        StartCoroutine(LoadAfterDelay());
    }

    private IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(_loadLevelAfterHowManySeconds);
        Debug.Log($"Waited for {_loadLevelAfterHowManySeconds} seconds!");
        SceneManager.LoadScene(_sceneName);

    }
}
