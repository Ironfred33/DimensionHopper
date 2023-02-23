using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloating : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private PlayerController _playerControls;
    private TransformPositionOnPerspective _pgoScript;
    [SerializeField] private EVPlatformFloating _floating;
    public float wobblingTime;
    public float wobblingAmount;
    [SerializeField] private bool _playerTouching;
    [SerializeField] private bool _platformLowering;
    [SerializeField] private bool _platformRaising;
    private bool _platformWobbling;
    public Vector3 standardPosition;
    public Vector3 loweredPosition;
    private float _elapsed;
    private float _dt;
    private bool _isPGO;


    // PROBLEM 2: Wenn Spieler sich nicht auf PGO befindet, bewegt sie sich, aber wird wieder zurückgesetzt. Theorie: Coroutine Positionsänderung sticht sich mit
    // der PGO Positionsänderung

    // DAS PROBLEM LIEGT IN DIESEM SCRIPT DIOS MIOS COROUTINE UFF OH NEIN


    // Variablen externalisieren!


    void Start()
    {
        AssignComponents();

        SetPositions();
    }

    void SetPositions()
    {

        standardPosition = this.transform.position;
        loweredPosition = new Vector3(standardPosition.x, standardPosition.y - _floating.loweringAmount, standardPosition.z);

    }

    void AssignComponents()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerControls = _player.GetComponent<PlayerController>();
        if (this.GetComponent<TransformPositionOnPerspective>() == true)
        {
            _isPGO = true;
            _pgoScript = this.GetComponent<TransformPositionOnPerspective>();
        }

        _floating = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVPlatformFloating>();

    }


    void Update()
    {

        if (_isPGO)
        {
            if ((_playerTouching && !_platformLowering) && (this.transform.position == standardPosition))
            {
                _platformLowering = true;
                StartCoroutine(LowerPlatform(loweredPosition));

            }
            else if ((_playerTouching && !_platformLowering) && (this.transform.position == _pgoScript.transformSecondPoint))
            {
                _platformLowering = true;
                StartCoroutine(LowerPlatform(_pgoScript.transformSecondPointLowered));

            }


            if ((!_playerTouching && !_platformRaising) && (this.transform.position == loweredPosition))  // <------- HIER FEHLER
            {
                _platformRaising = true;
                StartCoroutine(RaisePlatform(standardPosition));

            }
            else if((!_playerTouching && !_platformRaising) && (this.transform.position == _pgoScript.transformSecondPointLowered))
            {
                _platformRaising = true;
                StartCoroutine(RaisePlatform(_pgoScript.transformSecondPoint));
            }




            if (!_playerTouching) _platformWobbling = false;

        }
        else if (!_isPGO)
        {

            if ((_playerTouching && !_platformLowering) && (this.transform.position == standardPosition))
            {
                _platformLowering = true;
                StartCoroutine(LowerPlatform(loweredPosition));

            }

            if (((!_playerTouching && !_platformRaising) && (this.transform.position == loweredPosition)))
            {
                _platformRaising = true;
                StartCoroutine(RaisePlatform(standardPosition));

            }

            if (!_playerTouching) _platformWobbling = false;


        }


    }


    IEnumerator LowerPlatform(Vector3 loweredPosition)
    {
        //_lowerCoroutineRunning = true;

        _elapsed = 0f;


        while (_elapsed <= _floating.loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, loweredPosition, _elapsed / _floating.loweringTime);

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



    // HIER IST DER FEHLER XXXX
    IEnumerator RaisePlatform(Vector3 standardPosition)
    {

        Debug.Log("PLATFORM RAISING COROUTINE START");

        _elapsed = 0f;

        //_player.transform.parent = this.transform;


        while (_elapsed <= _floating.raisingTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, standardPosition, _elapsed / _floating.raisingTime);

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


        while (_elapsed <= _floating.loweringTime)
        {
            _dt = Time.deltaTime;
            _elapsed = _elapsed + _dt;

            this.transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + wobblingAmount, transform.position.z), _elapsed / wobblingTime);

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
