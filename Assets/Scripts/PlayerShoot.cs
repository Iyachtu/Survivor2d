using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float _attCD;
    private float _nextAtt;

    [SerializeField] private GameObject _bulletPrefab, _rewardManager;
    [SerializeField] private IntVariables _bulletSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoolVariables _pause;
    [SerializeField] private BoolVariables _dead;

    [SerializeField] private BoolVariables _addShot, _deathShot;
    [SerializeField] private GameObject _levelUpMenu;


    List<Vector2> Cardinal = new List<Vector2>()
        {
            new Vector2(1.5f,0),
            new Vector2(0,1.5f),
            new Vector2(-1.5f,0),
            new Vector2(0,-1.5f),
        };

    List<Vector2> NonCardinal = new List<Vector2>()
        {
            new Vector2(1,1),
            new Vector2(-1,-1),
            new Vector2(1,-1),
            new Vector2(-1,1),
        };

    private void Awake()
    {
        _rewardManager = GameObject.Find("RewardManager");
    }

    // Start is called before the first frame update
    void Start()
    {
        _addShot.value = _deathShot.value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_pause.value && !_dead.value) Shoot();
    }

    private void Shoot()
    {
        if (Time.timeSinceLevelLoad > _nextAtt)
        {
            _nextAtt= Time.timeSinceLevelLoad+_attCD;
            foreach (var item in Cardinal)
            {
                InstBullet(item);
            }
            StartCoroutine(ShootAnim());
        }
    }

    public void SideShoot()
    {
        foreach (var item in NonCardinal)
        {
            InstBullet(item);
        }
    }

    private void InstBullet(Vector2 dir)
    {
        Vector2 vect = new Vector2(dir.x + transform.position.x, dir.y + transform.position.y);
        GameObject go = Instantiate(_bulletPrefab,vect, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = dir * _bulletSpeed.value;
    }

    IEnumerator ShootAnim()
    {
        _animator.SetBool("Shoot", true);
        yield return new WaitForSeconds(_attCD / 6);
        if (_addShot.value)
        {
            int a = Random.Range(1,3);
            if (a == 2) _rewardManager.GetComponent<RewardManager>().UnityEvent_AdditionalShot.Invoke();
        }
        yield return new WaitForSeconds(_attCD / 6);
        _animator.SetBool("Shoot", false);
    }

    public void AddShot()
    {
        _addShot.value=true;
        _pause.value = false;
        _levelUpMenu.SetActive(false);
    }

    public void DeathShot()
    {
        _deathShot.value=true;
        _pause.value = false;
        _levelUpMenu.SetActive(false);
    }
}
