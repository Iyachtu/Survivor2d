using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private IntVariables _killcount, _playerHP;
    private bool _dead;
    private Animator _animator;
    [SerializeField] private BoolVariables _pause, _deadShot, _playerDead;
    [SerializeField] private IntVariables _bulletSpeed;
    [SerializeField] private GameObject _bulletPrefab;
    private UnityEvent UnityEvent_EnemyDeathShot;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _dead= false;
        if(UnityEvent_EnemyDeathShot ==null) UnityEvent_EnemyDeathShot = new UnityEvent();
        UnityEvent_EnemyDeathShot.AddListener(DeathShot);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb= GetComponent<Rigidbody2D>();
        _animator= GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_pause.value) _rb.velocity = Vector2.zero;
        else if (!_pause.value && !_dead && !_playerDead.value) Move();   
    }

    private void Move()
    {
        Vector2 direction = _playerTransform.position - transform.position;
        if (direction.x < 0) direction.x = -1;
        else if (direction.x>0) direction.x = 1;
        if (direction.y < 0) direction.y = -1;
        else if (direction.y>0) direction.y= 1;
        _rb.velocity= direction * _speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerHP.value--;
            _dead = true;
            StartCoroutine( Death());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            _dead = true;
            StartCoroutine(Death());
        }
    }


    IEnumerator Death()
    {
        _rb.velocity = Vector2.zero;
        _animator.SetBool("Dead", true);
        yield return new WaitForSeconds(0.5f);
        _killcount.value++;
        if(_deadShot) UnityEvent_EnemyDeathShot.Invoke();
        Destroy(gameObject);
    }

    public void DeathShot()
    {
        GameObject go = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = Vector2.zero;
        do { direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)); }
        while (direction == Vector2.zero);
        go.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed.value;
    }
}
