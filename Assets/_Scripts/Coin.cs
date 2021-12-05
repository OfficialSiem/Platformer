using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //How many coins have been collected through out the level
    public static int CoinsCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null) //if the check turns out empty
            return; //Just stop running the code
        
        //Otherwise turn off the coin
        gameObject.SetActive(false);

        //Add one to the coin collected total
        CoinsCollected++;
    }
}
