using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ToggleSwitch : MonoBehaviour
{
    #region Sprite Direction
    [Tooltip("The default direction of the switch")]
    [SerializeField] ToggleDirection _startingDirection = ToggleDirection.Center;

    [Tooltip("What does the switch look like from the left")]
    [SerializeField] Sprite _left;

    [Tooltip("From the right, how does the switch look")]
    [SerializeField] Sprite _right;

    [Tooltip("The center swtich looks like what")]
    [SerializeField] Sprite _center;

    [SerializeField] UnityEvent _onToggledRight;
    [SerializeField] UnityEvent _onToggledLeft;
    [SerializeField] UnityEvent _onToggledCenter;

    [SerializeField] AudioClip _leftSound;
    [SerializeField] AudioClip _rightSound;

    private AudioSource _audioSource;


    SpriteRenderer _spriteRenderer;


    //Toggle Direction
    ToggleDirection _currentDirection;

    //The type of directions there are
    enum ToggleDirection
    {
        Left,
        Center,
        Right,
    }
    #endregion

    //In case I wanted a specific player to be able to move a switch
    //[SerializeField] int _playerNumber;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        SetToggleDirection(_startingDirection, true);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //in case I wantd a specific player to move the lever, I'd sandwhich the bellow comment
        //between the var player check and then teh return statement.
        //if (player == null || player.PlayerNumber != _playerNumber)

        //Check if a collider with a player component enters the collision area
        var player = collision.GetComponent<Player>();
        if (player == null)
            return; //stop reading code from here

        //Check if the player object also has a rigidbody component
        var playerRigidBody = player.GetComponent<Rigidbody2D>();
        if (playerRigidBody == null)
            return; //if there is not Rigidbody then stop reading code from here

        //Check which direction the player object came from 
        bool wasOnRight = collision.transform.position.x > transform.position.x;
        //And which direction their moving in
        bool playerWalkingRight = playerRigidBody.velocity.x > 0;
        bool playerWalkingLeft = playerRigidBody.velocity.x < 0;


        if (wasOnRight && playerWalkingRight)
            SetToggleDirection(ToggleDirection.Right);
        else if (!wasOnRight && playerWalkingLeft)
            SetToggleDirection(ToggleDirection.Left);
    }

    //Set the position of the switch to either left, center or right!
   void SetToggleDirection(ToggleDirection direction, bool force = false){


        //If the switch is already in the same direction, and we're not forcing a direction
        if (force == false && _currentDirection == direction)
            return; //stop reading code from here!
        
        _currentDirection = direction;

        //Litterally switches direction lol
        switch (direction)
        {
            case ToggleDirection.Left:
                _spriteRenderer.sprite = _left;
                _onToggledLeft.Invoke();
                if (_audioSource != null)
                    _audioSource.PlayOneShot(_leftSound);
                break;
            case ToggleDirection.Center:
                _spriteRenderer.sprite = _center;
                _onToggledCenter.Invoke();
                break;
            case ToggleDirection.Right:
                _spriteRenderer.sprite = _right;
                _onToggledRight.Invoke();
                if (_audioSource != null)
                    _audioSource.PlayOneShot(_rightSound);
                break;
            default:
                break;
        }
   }

    //In the editor, instead of swapping out sprites, I can just do this!
    void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        switch (_startingDirection)
        {
            case ToggleDirection.Left:
                _spriteRenderer.sprite = _left;
                break;
            case ToggleDirection.Center:
                _spriteRenderer.sprite = _center;
                break;
            case ToggleDirection.Right:
                _spriteRenderer.sprite = _right;
                break;
            default:
                break;
        }
    }

    public void TestDirection()
   {
        Debug.Log($"Switch turned: {_currentDirection}!");
   }
}
