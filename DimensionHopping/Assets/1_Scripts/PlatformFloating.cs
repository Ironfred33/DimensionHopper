using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloating : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    private PlayerController _playerControls;

    public float loweringTime;
    public float raisingTime;

    public float loweringAmount;
    [SerializeField] private bool _playerTouching;
    [SerializeField] private bool _platformLowering;
    [SerializeField] private bool _platformRaising;
    private Vector3 _standardPosition;
    private Vector3 _loweredPosition;

    private IEnumerator _lowerPlatform;
    private IEnumerator _raisePlatform;

    private float _elapsed;
    private float _dt;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerControls = _player.GetComponent<PlayerController>();
        _standardPosition = this.transform.position;
        _loweredPosition = new Vector3(_standardPosition.x, _standardPosition.y - loweringAmount, _standardPosition.z);
        _lowerPlatform = LowerPlatform();
        _raisePlatform = RaisePlatform();


    }



    void Update()
    {
        if (_playerTouching && !_platformLowering)
        {
            StartCoroutine(_lowerPlatform);
            _platformLowering = true;

        }

        if ((!_playerTouching && !_platformRaising) && (this.transform.position != _standardPosition) )
        {
            StartCoroutine(_raisePlatform);
            _platformRaising = true;
        }

        // if (!_playerTouching && _lowerCoroutineRunning)
        // {
        //     StopCoroutine(_lowerPlatform);
        //     _platformLowering = false;
        //     _platformRaising = false;


        // }



    }


    IEnumerator LowerPlatform()
    {
        //_lowerCoroutineRunning = true;

        _elapsed = 0f;


        // PLayer wird zum Child

        _player.transform.parent = this.transform;

        //object1.transform.parent = object2.transform 
        //object1 is now the child of object2

        while (_elapsed <= loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;


            this.transform.position = Vector3.Lerp(transform.position, _loweredPosition, _elapsed / loweringTime);

            yield return null;


        }



        this.transform.position = _loweredPosition;


        Debug.Log("PIEP PIEP PIEP");
        _platformLowering = false;

        yield return null;


        //_lowerCoroutineRunning = false;
    }

    IEnumerator RaisePlatform()
    {
        //_raiseCoroutineRunning = true;

        _elapsed = 0f;


        _player.transform.parent = this.transform;


        while (_elapsed <= loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;


            this.transform.position = Vector3.Lerp(transform.position, _standardPosition, _elapsed / raisingTime);

            yield return null;


        }


        this.transform.position = _standardPosition;


        _platformRaising = false;

        yield return null;


        //_lowerCoroutineRunning = false;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            _playerTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerTouching = false;
        }
    }






}
