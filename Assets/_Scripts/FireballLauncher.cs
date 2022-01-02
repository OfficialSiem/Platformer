using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLauncher : MonoBehaviour
{
    #region Fire Ball & Shooting
    [Tooltip("The Fireballs Prefab")]
    [SerializeField] FireBall _fireballPrefab = null;
    string _fireButton;
    private float _nextFireTime;
    [Tooltip("At what rate should the fireball be shooting at?")]
    [SerializeField] float _fireRate = 0.25f;
    #endregion

    //Figure out which player is firing
    Player _player = null;
    string _horizontalAxis;

    //Used to figure out which button is being pressed




    void Awake()
    {
        _player = GetComponent<Player>();
        _fireButton = $"P{_player.PlayerNumber}Fire";
        _horizontalAxis = $"P{_player.PlayerNumber}Horizontal";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(_fireButton) && Time.time >= _nextFireTime)
        {
            var horizontal = Input.GetAxis(_horizontalAxis);
            FireBall fireball = Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
            fireball.Direction = horizontal >= 0 ? 1f : -1f;
            _nextFireTime = Time.time + _fireRate;
        }
            
    }
}
