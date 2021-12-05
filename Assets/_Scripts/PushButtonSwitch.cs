using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PushButtonSwitch : MonoBehaviour
{
    [SerializeField] Sprite _downSprite;
    [SerializeField] UnityEvent _onPressed;
    [SerializeField] UnityEvent _onReleased;
    SpriteRenderer _spriteRenderer;

    Sprite _releasedSprite;
    [SerializeField] int _playerNumber;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _releasedSprite = _spriteRenderer.sprite;
        BecomeReleased();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if an object with the marker entered the trigger area
        var player = collision.GetComponent<Player>();

        if (player == null || player.PlayerNumber != _playerNumber)
            return;  //stop reading code from here
        BecomePressed();
    }

    void BecomePressed()
    {
        _spriteRenderer.sprite = _downSprite;
        _onPressed?.Invoke();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null || player.PlayerNumber != _playerNumber)
            return;  //stop reading code from here

        BecomeReleased();
    }
    void BecomeReleased()
    {
        //If there is an event count that uses this method 
        if (_onReleased.GetPersistentEventCount() != 0)
        {
            _spriteRenderer.sprite = _releasedSprite;
            _onReleased.Invoke();
        }
    }
}
