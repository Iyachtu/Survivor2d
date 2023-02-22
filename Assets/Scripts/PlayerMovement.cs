using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _movement;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    private Vector2 _move;
    [SerializeField] private IntVariables _playerHP,_killcount;
    private int _previousCount;
    [SerializeField] private BoolVariables _dead;
    [SerializeField] private BoolVariables _pause;
    [SerializeField] private TMP_Text _killText;
    [SerializeField] GameObject _deathMenu;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dead.value= false;
        _playerHP.value = 1;
        _killcount.value = 0;
        _pause.value = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_pause.value) _rb.velocity = Vector2.zero;
        else if (!_pause.value)
        {
            if (_playerHP.value <= 0 && _dead.value==false) Death();
            //_move = _movement.action.ReadValue<Vector2>();
            _move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (_previousCount<_killcount.value)
            {
                UpdateKillUI();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_pause.value)
        {
            if (_dead.value == false) _rb.velocity = _move * _speed;
            else if (_dead.value) _rb.velocity = Vector2.zero;
        }
    }

    private void Death()
    {
        if(_dead.value==false)
        {
            _deathMenu.SetActive(true);
        }
        _dead.value= true;
    }

    private void UpdateKillUI()
    {
        _previousCount= _killcount.value;
        _killText.text = "Killcount: " + _previousCount;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
