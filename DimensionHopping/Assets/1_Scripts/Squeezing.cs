using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeezing : MonoBehaviour
{

    [SerializeField] private float _minDistance;
    [SerializeField] private float _minSize;
    [SerializeField] private float _squeezeTime;
    [SerializeField] private int _squeezeDimension;
    private bool _squeezed;
    private float _squeezeDuration;
    private bool _squeezable;
    private GameObject _player;

    void Start()
    {   
        _squeezable = false;
        _squeezed = false;
        _squeezeDuration = Vector3.Distance(GetComponent<MovingPlatforms>().firstPosition, GetComponent<MovingPlatforms>().secondPosition) / GetComponent<MovingPlatforms>().movementTime;
    }

    void FixedUpdate()
    {

        Debug.DrawRay(transform.position, Vector3.forward*_minDistance, Color.magenta);

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.forward, out hit, _minDistance) && _squeezable)
        {
            if(hit.collider.gameObject.CompareTag("Squeezer"))
            {
                StartCoroutine(SqueezeProcess());      
                
            }
        }

        
        else if(_squeezed)
        {
            StartCoroutine(SqueezeDuration());
            StartCoroutine(SqueezeProcess());
        }

    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
            _squeezable = true;
        }   
    }

    private void OnCollisionExit(Collision other) 
    {
        _squeezable = false;

    }

    // Squeezes character via interpolation
    private IEnumerator SqueezeProcess()
    {
        float _elapsed = 0f;

        while(_elapsed <= _squeezeDuration)
        {
            _elapsed = _elapsed + Time.deltaTime;

            if(_squeezable)
            {
                switch (_squeezeDimension)
                {
                case 0:

                    if(_player.transform.localScale.x > _minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(_minSize, _player.transform.localScale.y, _player.transform.localScale.z), _elapsed/20);
                    }

                    else if(_player.transform.localScale.x < -_minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(-_minSize, _player.transform.localScale.y, _player.transform.localScale.z), _elapsed/20);
                    }
                    break;
                
                case 1:
                if(_player.transform.localScale.y > _minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(_player.transform.localScale.x, _minSize, _player.transform.localScale.z), _elapsed/20);
                    }

                    else if(_player.transform.localScale.y < -_minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(_player.transform.localScale.x, -_minSize, _player.transform.localScale.z), _elapsed/20);
                    }
                    break;

                case 2:
                if(_player.transform.localScale.z > _minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(_player.transform.localScale.x, _player.transform.localScale.y, _minSize), _elapsed/20);
                    }

                    else if(_player.transform.localScale.y < -_minSize)
                    {
                        _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(_player.transform.localScale.x, _player.transform.localScale.y, -_minSize), _elapsed/20);
                    }
                    break;
                    
                default:
                    break;
                }

                _squeezed = true; 
                               

            }

            else if(!_squeezed)
            {
                if(_player.transform.localScale.x > 0)
                {
                    _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(1,1,1), _elapsed/1000); 
                }

                else if(_player.transform.localScale.x < 0)
                {
                    _player.transform.localScale = Vector3.Lerp(_player.transform.localScale, new Vector3(1,1,-1), _elapsed/1000);
                }

            }
            
            yield return null;  

        }
    }

    // Decompresses character after a specified time
    private IEnumerator SqueezeDuration()
    {
        yield return new WaitForSeconds(_squeezeTime);
        _squeezed = false;
    }
    
}
