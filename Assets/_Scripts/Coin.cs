using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //How many coins have been collected through out the level
    public static int CoinsCollected;

    //How many points are earned for picking up the ciub
    [SerializeField] int pointsEarned = 100;

    //List of sounds to play when picking up a coin
    [SerializeField] List<AudioClip> _clips;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null) //if the check turns out empty
            return; //Just stop running the code
        
        //Add one to the coin collected total
        CoinsCollected++;

        //Add Points to a Score
        ScoreSystem.Add(pointsEarned);

        //Turn off the Collider
        GetComponent<Collider2D>().enabled = false;
        
        //Turn off the Sprite Renderer
        GetComponent<SpriteRenderer>().enabled = false;

        //If we have more than one sound file we want to play when grabbing a coin
        if(_clips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _clips.Count);
            AudioClip clip = _clips[randomIndex];
            //Play coin music (to sound as if a coin was picked up)
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        else
        {
            GetComponent<AudioSource>().Play();
        }
        
    }
}
