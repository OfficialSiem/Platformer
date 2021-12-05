using System;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //List of which collectors has access to this collectable
    public event Action OnPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null) //if the check turns out empty
            return; //Just stop running the code

        //Otherwise turn off the object
        gameObject.SetActive(false);

        //If the Pick Up event isnt null, then invoke it!
        OnPickedUp?.Invoke();


    }
}