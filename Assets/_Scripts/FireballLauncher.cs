using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLauncher : MonoBehaviour
{
    [Tooltip("The Fireballs Prefab")]
    [SerializeField] FireBall _fireballPrefab;
    void Start()
    {
        Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
