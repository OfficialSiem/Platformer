using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Key Attributes")]
    [Tooltip("Which keylock this key should open")]
    [SerializeField] KeyLock _keyLock;

    [Tooltip("How many times the key can be used")]
    [SerializeField] int maxKeyUses;

    //How many times the key has been used
    int numOfKeyUses;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if a collider with a player component enters the trigger area
        var player = collision.GetComponent<Player>();

        //Check if an object with a KeyLock component has entered the trigger area
        var keyLock = collision.GetComponent<KeyLock>();

        if (player != null)
        {
            //Make the object a child of whomever collided with it
            transform.SetParent(player.transform);

            //Make the object appear above the player
            transform.localPosition = Vector3.up;
            Debug.Log("Handing it over to Player");
        }

        //If we have met the right keylock
        if (keyLock != null && keyLock == _keyLock)
        {
            //Then unlock it
            keyLock.Unlock();

            numOfKeyUses++;
        }

        //If the number of times we use the key is equal to our max (or above)
        if(maxKeyUses <= numOfKeyUses)
        {
            Debug.Log("Key Has Been Used");
            //Destory the key
            Destroy(gameObject);
        }

    }

}
