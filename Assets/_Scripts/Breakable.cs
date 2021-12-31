using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Tooltip("How many points earned from hitting the box?")]
    [SerializeField] int pointsEarned = 100;

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
        PlayParticleEffects();
        PlayAudio();
        ScoreSystem.Add(pointsEarned);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void PlayParticleEffects()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
            particleSystem.Play();
        else
            Debug.Log("MISSING PARTICLE SYSTEM");
    }

    private void PlayAudio()
    {
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();
        else
            Debug.Log("MISSING AUDIO SOURCE");
    }
}
