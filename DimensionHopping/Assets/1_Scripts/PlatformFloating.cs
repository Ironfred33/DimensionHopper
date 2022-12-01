using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloating : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private PlayerController _playerControls;
    private TransformPositionOnPerspective _pgoScript;
    public float loweringTime;
    public float raisingTime;
    public float wobblingTime;
    public float wobblingAmount;
    public float loweringAmount;
    [SerializeField] private bool _playerTouching;
    [SerializeField] private bool _platformLowering;
    [SerializeField] private bool _platformRaising;
    private bool _platformWobbling;
    public Vector3 standardPosition;
    public Vector3 loweredPosition;
    private IEnumerator _lowerPlatform;
    private IEnumerator _raisePlatform;
    private float _elapsed;
    private float _dt;




    // PROBLEM 2: Wenn Spieler sich nicht auf PGO befindet, bewegt sie sich, aber wird wieder zurückgesetzt. Theorie: Coroutine Positionsänderung sticht sich mit
    // der PGO Positionsänderung

    // DAS PROBLEM LIEGT IN DIESEM SCRIPT DIOS MIOS COROUTINE UFF OH NEIN


    // Variablen externalisieren!
    

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerControls = _player.GetComponent<PlayerController>();
        standardPosition = this.transform.position;
        loweredPosition = new Vector3(standardPosition.x, standardPosition.y - loweringAmount, standardPosition.z);
        _pgoScript = this.GetComponent<TransformPositionOnPerspective>();

    }


    void Update()
    {
        if ((_playerTouching && !_platformLowering) && (this.transform.position == standardPosition) || (_playerTouching && !_platformLowering) && (this.transform.position == _pgoScript.transformSecondPoint))
        {
            _platformLowering = true;
            StartCoroutine(LowerPlatform());
            
        }

        if (((!_playerTouching && !_platformRaising) && (this.transform.position == loweredPosition)) || (!_playerTouching && !_platformRaising) && (this.transform.position == _pgoScript.transformSecondPointLowered) )  // <------- HIER FEHLER
        {
            _platformRaising = true;
            StartCoroutine(RaisePlatform());
            
        }

        if(!_playerTouching) _platformWobbling = false;

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

        //_player.transform.parent = this.transform;


        while (_elapsed <= loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, loweredPosition, _elapsed / loweringTime);

            yield return null;

        }

        //this.transform.position = loweredPosition;


        Debug.Log("PIEP PIEP PIEP");
        _platformLowering = false;


        

        yield return null;

        // if(!_platformWobbling)
        // {
        //     _platformWobbling = true;
        //     StartCoroutine(Wobbling());

        // }
    

    }

    IEnumerator RaisePlatform()
    {

        Debug.Log("PLATFORM RAISING COROUTINE START");

        _elapsed = 0f;

        //_player.transform.parent = this.transform;


        while (_elapsed <= raisingTime) 
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, standardPosition, _elapsed / raisingTime);

            yield return null;


        }

        //this.transform.position = standardPosition;

        _platformRaising = false;

        yield return null;

        //_lowerCoroutineRunning = false;
    }

    IEnumerator Wobbling()
    {
        _elapsed = 0f;


        _player.transform.parent = this.transform;


        while (_elapsed <= loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, new Vector3 (transform.position.x, transform.position.y + wobblingAmount, transform.position.z), _elapsed / wobblingTime);

            yield return null;

        }

        yield return null;
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
