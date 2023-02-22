using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private BoolVariables _pause;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private IntVariables _bulletSpeed;
    private GameObject _player;
    public UnityEvent UnityEvent_AdditionalShot;
    [SerializeField] GameObject _levelUpMenu;
    [SerializeField] private IntVariables _killcount;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        if (UnityEvent_AdditionalShot == null) UnityEvent_AdditionalShot= new UnityEvent();
        UnityEvent_AdditionalShot.AddListener(AdditionalShot);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_killcount.value == 10) LevelUp();
    }

    public void AdditionalShot()
    {
        _player.GetComponent<PlayerShoot>().SideShoot();
    }

    private void LevelUp()
    {
        _pause.value = true;
        _levelUpMenu.SetActive(true);
    }
}
