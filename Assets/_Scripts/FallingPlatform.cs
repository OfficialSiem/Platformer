using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    #region Initialization
    [Header("Initialization ")]
    [Tooltip("Does the platform have a parent gameObject?")]
    [SerializeField] Transform _parentGameObject;

    Vector3 _initialPosition = Vector3.zero;

    //This is to count how many times the platform has been re-created
    private int replacementNumber = 1;
    #endregion

    #region Wiggling
    //A HashSet will contain the first instance of an element (no dupplicates)
    //Perfect data structure to check how many players are on the platform.
    [Header("Platform Player Checks")]
    [Tooltip("Is there a collider with the player script on the platform?")]
    [SerializeField] bool _playerInside = false;

    HashSet<Player> _playersInTrigger = new HashSet<Player>();

    [Tooltip("Reset the wiggle timer when no players are on the platform")]
    [SerializeField] bool _resetTimersOnEmpty = true;

    [Tooltip("How long to wait until the platform starts to wiggle")]
    [SerializeField] float _delayWigglingForHowLong;

    [Header("Platform Wiggling Conditions")]
    [Tooltip("How long the platform should wiggle for")]
    [Range(0.1f, 5f)] [SerializeField] float _wiggleDuration;

    float _wiggleTimer = 0;

    [Tooltip("Platform can be offset on the Y axis by this much while shaking")]
    [Range(0.005f, 0.10f)] [SerializeField] float _shakeY;

    [Tooltip("How far the platform's x position can be offset from")]
    [Range(0.005f, 0.10f)] [SerializeField] float _shakeX;
    #endregion

    #region Falling
    [Header("Platform Falling Conditions")]

    [Tooltip("The number of seconds it takes for the platform to fall")]
    [Range(0.1f, 5f)] [SerializeField] float _fallAfterSeconds = 3f;

    float _fallTimer = 0;

    [Tooltip("Speed in which the platform falls")]
    [SerializeField] float _fallspeed = 9f;

    [Tooltip("Is the platform falling?")]
    [SerializeField] bool _falling = false;
    #endregion

    #region Create New Platform
    [Header("New Platform Conditions")]
    [Tooltip("How long should a new platform take to be made")]
    [Range(0.1f, 5f)]  [SerializeField] float _createNewAfterSeconds = 5f;
  
    private float _creationTimer = 0f;

    //When turning on the transparency of a gameObject
    private float _alpha = 0f;
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
        if (_playersInTrigger.Count >= 1)
            StartCoroutine(WiggleAndFall());
    }

    IEnumerator WiggleAndFall()
    {
        //Waiting for platform to wiggle
        yield return new WaitForSeconds(_delayWigglingForHowLong);

        //Start Wiggling
        while (_playerInside && (_wiggleTimer < _wiggleDuration))
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

        //If after wiggling, a player is still inside
        if(_playerInside)
        {
            //Start Falling
            _falling = true;

            if (_falling)
            {
                //First grab all of the colliders 
                Collider2D[] colliders = GetComponents<Collider2D>();

                //Then cycle through the colliders
                foreach (var collider in colliders)
                {
                    //And turn off each collider
                    collider.enabled = false;
                }

                while (_fallTimer < _fallAfterSeconds)
                {

                    //Platform will move downward per frame at a falling speed
                    transform.position += Vector3.down * Time.deltaTime * _fallspeed;

                    //Increment timer as we fall
                    _fallTimer += Time.deltaTime;

                    //Caculate the transparency of the Object as it fades away
                    float alpha = ((_fallAfterSeconds - _fallTimer) / _fallAfterSeconds);
                    Color newColor = new Color(1.0f, 1.0f, 1.0f, alpha);
                    GetComponent<SpriteRenderer>().color = newColor;

                    yield return null;
                }
                _falling = false;
                _playerInside = false;
                _playersInTrigger.Clear();
                //Create a New Platform
                StartCoroutine(CreateNewPlatform());
            }

        }

    }

    IEnumerator CreateNewPlatform()
    {
        //Only create about 99 clones of the platform
        GameObject _newPlatform = Instantiate(gameObject, _initialPosition, Quaternion.identity, _parentGameObject);
        _newPlatform.name = gameObject.name + $"({replacementNumber})";
        //if we ever get past the clone limit
        if (replacementNumber == 99)
        {
            //destroy everything
            Destroy(_newPlatform);
            Destroy(gameObject);
            yield break; //stop running code form here (if for any reason we get to there)
        }
        replacementNumber++;
        //While the creation process is happening
        while (_creationTimer <= _createNewAfterSeconds)
        {

            //Caculate the transparency of the Object as it fades back into game
            _alpha = ((_creationTimer) / _createNewAfterSeconds);
            Debug.Log(_alpha);
            _newPlatform.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, _alpha);
            _creationTimer += Time.deltaTime;
            yield return null;
        }

        //First grab all of the colliders 
        Collider2D[] colliders = _newPlatform.GetComponents<Collider2D>();

        //Then cycle through the colliders
        foreach (var collider in colliders)
        {
            //And turn off each collider
            collider.enabled = true;
        }
        //destroy the old platform
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Check if The Platform is Falling");

        //If the platform is falling  
        if (_falling)
        {
            Debug.Log("YES! THE PLATFORM WILL CONTINUE TO FALL");
            return;//Then no need to read the rest of the code
        }

        //Check to see if a player has exited the Trigger Area
        var player = collision.GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("No Player Was Found");
            return; //if not, then return null
        }

        //Remove one player element from the HashSet
        _playersInTrigger.Remove(player);

        Debug.Log("The Hash Count is at: " + _playersInTrigger.Count);
        //If the HashSet has zero player elements
        if (_playersInTrigger.Count == 0)
        {
            //thus _playerinsde is set to false
            _playerInside = false;
            Debug.Log("Stopping Wiggling");

            //Turn off the falling
            _falling = false;

            //then there are no longer any players along the boundry,
                 

            Debug.Log("Stopping Coroutine");
            //Also, stop the coroutine, because zero players are standing on it!
            StopCoroutine(WiggleAndFall());


            Debug.Log("Everything Stopped");
            //And if the WiggleTimer resets if a player leaves
            if (_resetTimersOnEmpty)
            {
                _wiggleTimer = 0f; //then reset the timer
                _fallTimer = 0f;
            }
                
        }

    }
}
