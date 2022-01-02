using UnityEngine;

public class KillOnEnter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if an object with out the Player component has entered the trigger area
        var player = collision.collider.GetComponent<Player>();
        if (player == null) //do nothing
            return; //stop running the code past this point

        //Assuming a player has entered the trigger area
        //immediately send the player back to the restart point
        player.ResetToStart();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if an object with out the Player component has entered the trigger area
        var player = collision.GetComponent<Player>();
        if (player == null) //do nothing
            return; //stop running the code past this point

        //Assuming a player has entered the trigger area
        //immediately send the player back to the restart point
        player.ResetToStart();

        //Quick Note: Actually we can probably set some type of Unity Event to get an audio feedback as well!
    }

    private void OnParticleCollision(GameObject other)
    {
        //if an object with out the Player component has entered the Collision area
        var player = other.GetComponent<Player>();
        if (player == null) //do nothing
            return; //stop running the code past this point

        //Otherwise
        if (player != null)
            //immediately send the player back to the restart point
            player.ResetToStart();
    }
}
