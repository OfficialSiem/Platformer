using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [Tooltip("Every single thing that happens when a door is unlocked!")]
    public UnityEvent _onUnlocked;

    public void Unlock()
    {
        //Unlockes Door
        _onUnlocked.Invoke();
    }
}
