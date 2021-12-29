using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In the future, I want to have it so doors can decide which collectables they need (and how many of those collectables they need)
public class Door : MonoBehaviour
{
    [Header("Door Sprite")]
    [Tooltip("How the bottom of the middle of the door looks like when open")]
    [SerializeField] Sprite _openMid = null;
    [Tooltip("What does the top of the door look like when open")]
    [SerializeField] Sprite _openTop = null;
    [Tooltip("Mid Renderer")]
    [SerializeField] SpriteRenderer _rendererMid = null;
    [Tooltip("Top Renderer")]
    [SerializeField] SpriteRenderer _rendererTop = null;

    [Header("Door Requirements")]
    [SerializeField] int _requiredCoins = 3;

    [Header("Exit Door")]
    [Tooltip("Where does this door lead to")]
    [SerializeField] Door _exit = null;

    [Header("Canvas")]
    [Tooltip("Which Canvas displays what the door needs")]
    [SerializeField] Canvas _doorCanvas = null;

    //Whether or not the door may open
    bool _mayOpen = false;

    //Helps open the door from the editor 
    [ContextMenu("Open Door")]
    public void Open()
    {
        if(_doorCanvas != null)
            _doorCanvas.enabled = false;

        _mayOpen = true;
        _rendererMid.sprite = _openMid;
        _rendererTop.sprite = _openTop;
        
    }

    private void Update()
    {
        if (_mayOpen == false && Coin.CoinsCollected >= _requiredCoins)
            Open();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   //Check if the door is may be open
            if (_mayOpen == false)
            return; //then stop running code from here

        //Check if who ever is near the door is a player
        var player = collision.GetComponent<Player>();
        if (player == null)
            return; //If not then don't read any code further

        //If an exit was assign to the door

        if(_exit != null)
        {
            //And if the input W key was pressed
            if (Input.GetKeyDown(KeyCode.W))
                player.TeleportTo(_exit.transform.position); //Teleport the player to where the exit is
        }

    }

}
