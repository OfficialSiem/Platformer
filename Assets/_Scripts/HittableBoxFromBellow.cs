using UnityEngine;

public class HittableBoxFromBellow : MonoBehaviour
{
    [Tooltip("What should the box look like when it's done!")]
    [SerializeField] protected Sprite _usedBox = null;

    [Tooltip("Can the box be used?")]
    protected virtual bool CanUse => true;

    //Animator for the HittableBox
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if we can use the box
        if (CanUse == false)
            return; //if not then end reading code from here
        
        //Check if a collider with a player component enters the trigger area
        var player = collision.collider.GetComponent<Player>();
        if (player == null) //if the check turns out empty
            return; // then end reading code from here



        //But if what ever hit the box was hitting them from bellow
        if (collision.contacts[0].normal.y > 0)
        {
            //Play the animation
            PlayAnimation();

            //Use the box
            Use();

            //And update it's apperance if it can no longer be used
            if(CanUse == false)
                GetComponent<SpriteRenderer>().sprite = _usedBox;
        }
    }

    private void PlayAnimation()
    {
        //Play the animator if we have one
        if (_animator != null)
            //Set trigger to 0
            _animator.SetTrigger("Use");

    }

    protected virtual void Use()
    {

    }
}
