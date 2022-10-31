using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloating : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    private PlayerController _playerControls;
    
    public int loweringAmount;
    [SerializeField]  private bool _playerTouching;
    private bool _platformLowered;

    [SerializeField] private Vector3 _standardPosition;
    [SerializeField] private Vector3 _loweredPosition;

    private IEnumerator _lowerPlatform;

    private bool _lowerCoroutineRunning;
    private bool _raiseCoroutineRunning;
    

    void Start()
    {   
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerControls = _player.GetComponent<PlayerController>();
        _standardPosition = this.transform.position;
        _loweredPosition = new Vector3 (_standardPosition.x, _standardPosition.y - loweringAmount, _standardPosition.z);
        _lowerPlatform = LowerPlatform();
        
    }



    void Update()
    {
       if(_playerTouching && !_platformLowered)
       {
        	StartCoroutine(_lowerPlatform);
            _platformLowered = true;

       }

       if(!_playerTouching && _lowerCoroutineRunning) StopCoroutine(_lowerPlatform);
       




    }


    IEnumerator LowerPlatform()
    {
        _lowerCoroutineRunning = true;


        yield return new WaitForSeconds(1f);

        Debug.Log("PIEP PIEP PIEP");
        _platformLowered = false;



        


        yield return null;


        _lowerCoroutineRunning = false;
    }

    IEnumerator RaisePlatform()
    {
        _raiseCoroutineRunning = true;


        yield return new WaitForSeconds(1f);

        Debug.Log("PIEP PIEP PIEP");
        _platformLowered = false;



        


        yield return null;


        _raiseCoroutineRunning = false;
    }


    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player")
        {
            _playerTouching = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            _playerTouching = false;
        }
    }






}
