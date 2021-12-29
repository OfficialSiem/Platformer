using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float _bounceVelocity = 10f;
    [SerializeField] AudioSource _audioSource;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check what object collided with the player
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return; //if the object isn't a player, end the code here
        var rigidbody2D = player.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log("PLAYER IS MISSING A 2D RIGIDBODY COMPONENT!");
            return; //likewise, if the player doesn't have a rigidbody then end the code here
        }

        if(_audioSource != null)
        {
            _audioSource.Play();
        }
        //send the player flying upwards
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _bounceVelocity);
    }

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
