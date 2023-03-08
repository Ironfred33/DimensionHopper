using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    private CameraController _camScript;

    public GameObject cannon;
    private GameObject _player;
    private Rigidbody _playerRb;
    [SerializeField] private GameObject _bullet;
    private GameObject _bulletInstance;
    [SerializeField] private GameObject _target;

    [Range(3f, 10f)]
    public float attackRange;
    public float coolDown;
    [SerializeField] private bool _playerMovingRight;
    [SerializeField] private bool _playerMovingLeft;
    public bool randomBullets;
    public float bulletSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        Setup();

        StartCoroutine(ShootBullet());


    }

    void Setup()
    {
        _bullet = Resources.Load<GameObject>("Bossfight/Bullet");
        _camScript = Camera.main.GetComponent<CameraController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerRb = _player.GetComponent<Rigidbody>();

    }


    private void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            _playerMovingRight = true;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            _playerMovingLeft = true;

        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            _playerMovingRight = false;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _playerMovingLeft = false;
        }



    }

    Vector3 CalculateTargetPoint()
    {

        Vector3 targetPoint = new Vector3(0, 0, 0);

        float randomAttackRange = Random.Range(0, attackRange);


        if (_player.GetComponent<PlayerController>().state == PlayerState.Idle)
        {
            targetPoint = _player.transform.position;
        }
        else
        {
            switch (_camScript.playerOrientation)
            {
                case (CameraController.PlayerOrientation.XPositive):

                    if (_playerMovingRight) targetPoint = new Vector3(_player.transform.position.x + randomAttackRange, _player.transform.position.y, _player.transform.position.z);
                    else if (_playerMovingLeft) targetPoint = new Vector3(_player.transform.position.x - randomAttackRange, _player.transform.position.y, _player.transform.position.z);

                    break;


            }

        }


        return targetPoint;

    }


    IEnumerator ShootBullet()
    {
        float elapsedTime = 0;

        Vector3 pointToShootAt = CalculateTargetPoint();

        _bulletInstance = Instantiate(_bullet, cannon.transform.position, Quaternion.identity);

        Vector3 startPos = cannon.transform.position;

        _target.transform.position = pointToShootAt;


        while (elapsedTime < bulletSpeed)
        {


            _bulletInstance.transform.position = Vector3.Lerp(startPos, _target.transform.position, (elapsedTime / bulletSpeed));



            elapsedTime += Time.deltaTime;

            yield return null;


        }


        yield return new WaitForSeconds(coolDown);

        Destroy(_bulletInstance);

        StartCoroutine(ShootBullet());


        yield return null;
    }
}
