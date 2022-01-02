using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, ITakeDamage
{
    Rigidbody2D _rigidbody2D;

    [Tooltip("How far to check for objects and or if there is a floor")]
    [SerializeField] float _sensorDepth = 0.4f;

    [Tooltip("Which direction on the x axis do we travel in")]
    int _direction = -1;

    [Tooltip("What checks for things on the left")]
    [SerializeField] Transform _leftSensor = null;

    [Tooltip("Right things are checked with this")]
    [SerializeField] Transform _rightSensor = null;

    [Tooltip("What it looks like when its defeated")]
    [SerializeField] Sprite _deadSprite;

    
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(_direction, _rigidbody2D.velocity.y);
        if(_direction < 0)
            ScanSensor(_leftSensor);
        else
            ScanSensor(_rightSensor);
    }

    public void TakeDamage()
    {
        StartCoroutine(Die());
    }

    private void ScanSensor(Transform sensor)
    {
        ScanBellow(sensor);
        ScanSides(sensor);

    }


    private void ScanSides(Transform sensor)
    {
        //Draw a ray that extends from the sensor's position to bellow the ground
        Debug.DrawRay(sensor.position, new Vector2(_direction, 0) * _sensorDepth, Color.red);

        //Check if there is an object in the way
        var sideresult = Physics2D.Raycast(sensor.position, new Vector2(_direction, 0), _sensorDepth);
        if (sideresult.collider != null)
            TurnAround(); //If so, then turn around
    }

    private void ScanBellow(Transform sensor)
    {
        //Draw a ray that extends from the sensor's position to bellow the ground by the amout
        Debug.DrawRay(sensor.position, Vector2.down * _sensorDepth, Color.red);
        //check if the ground is bellow
        var result = Physics2D.Raycast(sensor.position, Vector2.down, _sensorDepth);
        if (result.collider == null)
            TurnAround(); //If not then turn around
    }

    private void TurnAround()
    {
        //Get the Sprite Rendered
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        //Change the direction we're moving
        _direction *= -1;
        //And then flip the sprite on the x axis if need be!
        _spriteRenderer.flipX = _direction > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        var contact = collision.contacts[0];
        Vector2 normal = contact.normal;
        Debug.Log($"Normal = {normal}");

        if (normal.y <= -0.5)
            TakeDamage();
        else
            player.ResetToStart();
    }

    IEnumerator Die()
    {
        PlayAudio();
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _deadSprite;
        GetComponent<Animator>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        float alpha = 1;
        while(alpha > 0)
        {
            yield return null;
            alpha -= Time.deltaTime;
            _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
        }

        
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
