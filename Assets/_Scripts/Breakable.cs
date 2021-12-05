using UnityEngine;

public class Breakable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if a collider with a player component enters the collision area
        if (collision.collider.GetComponent<Player>() == null)
            return; //stop reading code from here

        //If the collision made contact bellow the object
        if (collision.contacts[0].normal.y > 0)
            TakeHit();
    }

    void TakeHit()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
