using UnityEngine;

public class CoinBox : HittableBoxFromBellow
{
    [Tooltip("How many coins the box has")]
    [SerializeField] int _totalCoins = 3;

    protected override bool CanUse => _remainingCoins > 0;

    //How many coins are left in the box
    int _remainingCoins;

    private void Start()
    {
        _remainingCoins = _totalCoins;
    }

    protected override void Use()
    {
        //Use the base's method
        base.Use();
        //subtract one from how many coins the box has
        _remainingCoins--;
        //increment the number of coins collected by 1
        Coin.CoinsCollected++;
    }
}
