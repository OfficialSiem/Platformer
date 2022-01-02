using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D _rigidbody = null;

    [Tooltip("How fast the Fireball will move")]
    [Range(3f, 8f)] [SerializeField] float _launchForce = 5f;

    [Tooltip("Rate the fireball can bounce")]
    [Range(3f, 8f)] [SerializeField] float _bounceForce = 5f;

    public float Direction { get; set; }
    [Tooltip("How many times can the fireball bounce")]
    [SerializeField] int _bouncesRemaining = 3;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector2(_launchForce * Direction, _bounceForce);

    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        ITakeDamage danageable = collision.collider.GetComponent<ITakeDamage>();
        Fly fly = collision.collider.GetComponent<Fly>();
        if (danageable != null)
        {
            danageable.TakeDamage();
            DestroyFireBall();
            return;
        }

        _bouncesRemaining--;
        if (_bouncesRemaining < 0)
            DestroyFireBall();
        else
            _rigidbody.velocity = new Vector2(_launchForce * Direction, _bounceForce);

    }

    //In The Future, I'd like to make the Fireball emit some particles when it dies (smoke, smaller fireballs, whatever)
    void DestroyFireBall()
    {
        Destroy(gameObject);
    }
}
