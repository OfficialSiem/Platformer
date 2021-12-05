using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    [SerializeField] float _sensorDepth = 0.4f;
    int _direction = -1;
    [SerializeField] Transform _leftSensor = null;
    [SerializeField] Transform _rightSensor = null;
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

    private void ScanSensor(Transform sensor)
    {
        Debug.DrawRay(sensor.position, Vector2.down * _sensorDepth, Color.red);
        var result = Physics2D.Raycast(sensor.position, Vector2.down, _sensorDepth);
        if (result.collider == null)
            TurnAround();

        Debug.DrawRay(sensor.position, new Vector2(_direction, 0) * _sensorDepth, Color.red);
        var sideresult = Physics2D.Raycast(sensor.position, new Vector2(_direction, 0), _sensorDepth);
        if (sideresult.collider != null)
            TurnAround();

    }

    private void TurnAround()
    {
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        _direction *= -1;
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
            StartCoroutine(Die());
        else
            player.ResetToStart();
    }

    IEnumerator Die()
    {
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
}
