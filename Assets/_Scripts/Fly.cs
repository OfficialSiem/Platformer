using UnityEngine;

public class Fly : MonoBehaviour, ITakeDamage
{
    private Vector2 _startPosition = Vector2.zero;
    [SerializeField] private Vector2 _direction = Vector2.up;
    [SerializeField] float _maxDistance = 2;
    [SerializeField] float _flightSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction.normalized * Time.deltaTime * _flightSpeed);
        var distance = Vector2.Distance(_startPosition, transform.position);
        if(distance >= _maxDistance)
        {
            transform.position = _startPosition + (_direction.normalized * _maxDistance);
            _direction *= -1;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = _direction.x > 0;
        }
        

    }

    public void TakeDamage()
    {
        gameObject.SetActive(false);
    }
}
