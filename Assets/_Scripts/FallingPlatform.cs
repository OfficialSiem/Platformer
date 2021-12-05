using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    #region Wiggling


    //A HashSet will contain the first instance of an element (no dupplicates)
    //Perfect data structure to check how many players are on the platform.
    [Header("Platform Player Checks")]
    HashSet<Player> _playersInTrigger = new HashSet<Player>();

    [Tooltip("Is there a collider with the player script on the platform?")]
    [SerializeField] bool _playerInside = false;

    [Tooltip("Reset the wiggle timer when no players are on the platform")]
    [SerializeField] bool _resetOnEmpty = false;

    [Header("Platform Wiggling Conditions")]
    [Tooltip("How long the platform should wiggle for")]
    float _wiggleTimer;

    [Tooltip("Platform can be offset on the Y axis by this much while shaking")]
    [Range(0.005f, 0.10f)] [SerializeField] float _shakeY;

    [Tooltip("How far the platform's x position can be offset from")]
    [Range(0.005f, 0.10f)] [SerializeField] float _shakeX;
#endregion

    #region Falling
    [Header("Platform Falling Conditions")]
    Vector3 _initialPosition = Vector3.zero;

    [Tooltip("The number of seconds it takes for the platform to fall")]
    [Range(0.1f, 5f)] [SerializeField] float _fallAfterSeconds = 3f;

    [Tooltip("Speed in which the platform falls")]
    [SerializeField] float _fallspeed = 9f;

    [Tooltip("Is the platform falling?")]
    bool _falling;
    #endregion

    private void Start()
    {
        //Initialize the platform's position
        _initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check to see if a player has entered the Trigger Area
        var player = collision.GetComponent<Player>();
        if (player == null)
            //if not, then return null, easy way to get out of the loop
            return; 

        //Add one player element to the HashSet
        _playersInTrigger.Add(player);

        //Mark that a player is inside the platform
        _playerInside = true;

        //If we have one player then start to wiggle and fall!
        if (_playersInTrigger.Count == 1)
            StartCoroutine(WiggleAndFall());
    }

    IEnumerator WiggleAndFall()
    {
        //Waiting for platform to wiggle
        yield return new WaitForSeconds(0.25f);
        
        //Start Wiggling
        //_wiggleTimer = 0;
        while (_wiggleTimer < 1f)
        {
            //Explicitly use Unity Engine's Random generator to give a value to offset the X and Y value;
            float randomX = UnityEngine.Random.Range(-_shakeX, _shakeX);
            float randomY = UnityEngine.Random.Range(-_shakeY, _shakeY);

            //Adding the random offsets will create a wiggle effect
            transform.position = _initialPosition + new Vector3(randomX, randomY);

            //Wiggle by the offset for a random amount of time
            float randomDelay = UnityEngine.Random.Range(0.005f, 0.01f);

            //Wait for wiggle to end
            yield return new WaitForSeconds(randomDelay);

            //increment timer
            _wiggleTimer += randomDelay;
        }

        //Start Falling
        _falling = true;
        //First grab all of the colliders 
        Collider2D[] colliders = GetComponents<Collider2D>();

        //Then cycle through the colliders
        foreach(var collider in colliders)
        {
            //And turn off each collider
            collider.enabled = false;
        }

        float fallTimer = 0;
        while (fallTimer < _fallAfterSeconds)
        {

            //Platform will move downward per frame at a falling speed
            transform.position += Vector3.down * Time.deltaTime * _fallspeed;
            //Increment timer as we fall
            fallTimer += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the platform is falling  
        if (_falling)
            return;//Then no need to read the rest of the code

        //Check to see if a player has entered the Trigger Area
        var player = collision.GetComponent<Player>();
        if (player == null)
             return; //if not, then return null

        //Remove one player element from the HashSet
        _playersInTrigger.Remove(player);

        //If the HashSet has zero player elements
        if (_playersInTrigger.Count == 0)
        {
            //then there are no longer any players along the boundry,
            //thus _playerinsde is set to false
            _playerInside = false;

            //Also, stop the coroutine, because zero players are standing on it!
            StopCoroutine(WiggleAndFall());

            //And if the WiggleTimer resets if a player leaves
            if (_resetOnEmpty)
                _wiggleTimer = 0f; //then reset the timer
        }

    }
}
