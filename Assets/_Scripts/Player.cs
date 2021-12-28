using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Tooltip("Is this Player 1, 2, 3, 4.. etc?")]
    [SerializeField] int _playerNumber = 1;
   
    #region Movement Attributes
    [Header("Movement")]
    [Tooltip("How fast should the player move on the X axis")]
    [SerializeField] float _horizontalSpeed = 1f;
    [Tooltip("How slippery should slippery surfaces be (check formula in method)")]
    [SerializeField] float _slipFactor = 1;
    #endregion 

    #region Jump Attributes
    [Header("Jump")]
    [Tooltip("What transform is the player's feet located")]
    [SerializeField] Transform _feet = null;
    [Tooltip("How many times can the player jump")]
    [SerializeField] int _maxJumps = 2;
    [Tooltip("Duriation of each jump can be")]
    [SerializeField] float _maxJumpDuration = 0.1f;
    [Tooltip("The speed the player jumps at")]
    [SerializeField] float _jumpVelocity = 10f;
    [Tooltip("The jerk experienced by the player when they start to fall")]
    [SerializeField] float _downwardJerk = 0.1f;
    #endregion

    #region Private Player Attributes
    Rigidbody2D _rigidbody2D = null;
    SpriteRenderer _spriteRenderer = null;
    Animator _animator = null;
    Vector2 _startPosition = Vector2.zero;
    float _horizontal = 0f;

    int _layerMask = 0;
    bool _isGrounded = false;
    bool _isOnSlipperySurface = false;

    int _jumpsRemaining = 0;
    float _jumpTimer = 0;
    float _fallTimer = 0;
    private string _jumpButton = null;
    private string _horizontalAxis = null;
    #endregion
   
    
    //the players number is given by whatever the editor has
    public int PlayerNumber => _playerNumber;

    //initalize the player and all the nesseary attributes
    void Start()
    {
        _startPosition = transform.position;
        _layerMask = LayerMask.GetMask("Default");
        _jumpsRemaining = _maxJumps;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpButton = $"P{_playerNumber}Jump";
        _horizontalAxis = $"P{_playerNumber}Horizontal";
    }

    void Update()
    {
        //Check if the Player is grounded
        UpdateIsGrounded();

        //Then read the horzontal input
        ReadHorizontalInput();

        //Check if the ground is slippery
        if (_isOnSlipperySurface)
            SlipHorizontal(); //is so, manipulate rigidbody's velocity to make it feel like player is slipping
        else
            MoveHorizontal(); //otherwise, move rigidbody in the horizontal directions

        //Update the animations
        UpdateAnimator();
        UpdateSpriteDirection();

        //If the player should jump
        if (ShouldStartJump())
            Jump(); //start jumping
        else if (ShouldContinueJump()) //if the player is holding the jump button down
            ContineJump(); //then continue the jump

        _jumpTimer += Time.deltaTime; //add to the jump timer, to check for how long the player has jumped for

        //if the player is ground yet the fall timer is greater than zero
        if (_isGrounded && _fallTimer > 0)
        {
            //restart the fall timer
            _fallTimer = 0;

            //and reset how many jumps the player can make
            _jumpsRemaining = _maxJumps;
        }

        //if the player hasnt been grounded
        else
        {

            //just keep increaseing the fall timer
            _fallTimer += Time.deltaTime;

            //calculate the gravitational force the player should be experiencing in relation to framerate time
            var downSpeed = _downwardJerk * _fallTimer * _fallTimer;

            //then use the force to calculate the velocity the rigidbody should experience while falling
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - downSpeed);
        }
    }

    #region Checking Platform
    //Update whether or not the player is on the ground and what type of ground
    void UpdateIsGrounded()
    {
        //Create a circle that can check if the player is touching a surface with _layerMask on it.
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, _layerMask);

        //If the circle did hit something, then yes, the player is groudned
        _isGrounded = hit != null;

        //And if their is ground
        if (hit != null)
            //check to see if the surface is slippery
            _isOnSlipperySurface = hit.CompareTag("Slippery");
        else
            //otherwise the ground is by deafult, not slippery
            _isOnSlipperySurface = false;
    }
    #endregion
    
    #region Jump Mechanics
    void ContineJump()
    {
        //Calculate what speed the rigidbody should continue experience while jumping
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);

        //if your jumping set the fall timer equal to 0, because the player is not falling
        _fallTimer = 0;
    }
    //check if the player is still holding the jump button and if the player is still experiencing jumping for too long
    bool ShouldContinueJump()
    {
        return Input.GetButton(_jumpButton) && _jumpTimer <= _maxJumpDuration;
    }

    //Calculate what speed the rigidbody should experience while jumping
    void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);

        //subtract one from how many jumps are remaining
        _jumpsRemaining--;

        //and set the fall timer to 0, because the player is not falling
        _fallTimer = 0;

        //and set the jump timer to 0, because the player has just started their jump
        _jumpTimer = 0;
    }

    //check if the player has the ability to jump
    bool ShouldStartJump()
    {
        return Input.GetButtonDown(_jumpButton) && _jumpsRemaining > 0;
    }
    #endregion

    #region Movement Mechanics
    //move in the left or right direction based on the hoziontals
    void MoveHorizontal()
    {
        //the rigidbody's y velocity stays the same regardless of the players horizontal movement.
        _rigidbody2D.velocity = new Vector2(_horizontal * _horizontalSpeed, _rigidbody2D.velocity.y);
    }

    //sort of slide in the right or left direction
    void SlipHorizontal()
    {
        //the velocity is calculated here
        var desiredVelocity = new Vector2(_horizontal * _horizontalSpeed, _rigidbody2D.velocity.y);
        
        //but the player only experiences a gradual change in their velocity based on how slippery the surface is
        var smoothedVelocty = Vector2.Lerp(
            _rigidbody2D.velocity, 
            desiredVelocity, 
            Time.deltaTime / _slipFactor);
        _rigidbody2D.velocity = smoothedVelocty;

    }

    //read the horizontal movement of the player
    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis(_horizontalAxis) * _horizontalSpeed;
    }
    #endregion

    #region Animations
    //Update which direction the Player should be facing
    void UpdateSpriteDirection()
    {
        //if the player has inputed something
        if (_horizontal != 0)
        {
            //Assuming left is negative, and right is positive
            //only flip the sprite if the player has input a button in the negative direction
            _spriteRenderer.flipX = _horizontal < 0;
        }
    }

    //Update the animation for the player
    void UpdateAnimator()
    {
        //check if the player has inputed something, 
        bool isWalking = _horizontal != 0;

        //if so, then let the animator know it's okay to animate walking
        _animator.SetBool("canWalk", isWalking);

        //but seperately, check if the player can jump at all
        _animator.SetBool("canJump", ShouldContinueJump());
    }
    #endregion

    #region Relocate Player (for example on Death or when going through a door)
    //resets the player's position to the starting position
    internal void ResetToStart()
    {
        _rigidbody2D.position = _startPosition;
    }

    //Teleports the player into another location
    internal void TeleportTo(Vector3 position)
    {
        _rigidbody2D.position = position;

        //reset the player's velocity up till that point
        _rigidbody2D.velocity = Vector2.zero;
    }
    #endregion
}
