using System;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private int pointsEarned;

    //What happens on PickUp
    public event Action OnPickUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null) //if the check turns out empty
            return; //Just stop running the code

        //Add Points to a Score
        ScoreSystem.Add(pointsEarned);

        //Turn off the Collider
        GetComponent<Collider2D>().enabled = false;

        //Turn off the Sprite Renderer
        GetComponent<SpriteRenderer>().enabled = false;

        var _audioSource = GetComponent<AudioSource>();

        //If there is an audio soucre attached to the player (preferably a jump sfx) then play it!
        if (_audioSource != null)
            _audioSource.Play();

        //If the Pick Up event isnt null, then invoke it!
        OnPickUp?.Invoke();


    }
}