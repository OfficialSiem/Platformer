using UnityEngine;

public class ItemBox : HittableBoxFromBellow
{
    [Tooltip("Which prefab will the item use")]
    [SerializeField] GameObject _itemPrefab = null;

    [Tooltip("What item is in the box")]
    [SerializeField] GameObject _item = null;

    [Tooltip("Can the item be launched from the box")]
    [SerializeField] bool isLaunchable;

    //For future iterations, we can make it so the number of items equals the number of players!
    [Tooltip("How many items does the box have")]
    [SerializeField] int _totalitems = 3;

    //How many items are left in the box
    int _remainingItems;
    
    [Tooltip("When the item leaves the box, how fast should it be going")]
    [SerializeField] Vector2 _itemLaunchVelocity = Vector2.zero;

    //You can only  use the box if the box has an item, if all the items haven't been used, and if the box stil has items left.
    protected override bool CanUse => (_completelyUsed == false && _remainingItems > 0);


    //Were all the items in the box used!
    bool _completelyUsed = false;

    private void Start()
    {
        _remainingItems = _totalitems;
        if (_itemPrefab != null)
        {
            _itemPrefab.SetActive(false);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _usedBox;
        }

    }

    protected override void Use()
    {
        Debug.Log("Using Item Box");

        _item = Instantiate(_itemPrefab, transform.position + Vector3.up,
            Quaternion.identity,
            transform);

        //If there is no item
        if (_item == null)
            return; //stop the code right here

        base.Use();

        if (_remainingItems > 0)
        {
            //subtract one from how many items are in the box
            _remainingItems--;
            _item.SetActive(true);
            if(isLaunchable)
                LaunchItem();
        }

    }

    private void LaunchItem()
    {
        var itemRigidbody = _item.GetComponent<Rigidbody2D>();
        if (itemRigidbody != null)
        {
            itemRigidbody.velocity = _itemLaunchVelocity;
        }
    }
}
