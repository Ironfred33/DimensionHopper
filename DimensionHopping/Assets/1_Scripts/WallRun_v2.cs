using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert Wallrun des Spieler-Charakters
public class WallRun_v2 : MonoBehaviour
{
    public Transform orientation;
    public GameObject cam;

    public bool _wallLeft = false;
    public bool _wallRight = false;
    public bool isWallRunning = false;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    private Rigidbody _rb;

    public SFX soundEffects;
    private EVPlayer _playerVariables;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        _playerVariables = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVPlayer>();
    }

    void FixedUpdate()
    {
        WallCheck();
    }

    private void Update()
    {

        if (isWallRunning && _wallLeft)
        {
            cam.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,-30);
            Debug.Log("Left Tilt");
        }

        else if (isWallRunning && _wallRight)
        {
            cam.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,30);
            Debug.Log("Right Tilt");
        }

        if(isWallRunning && !Input.GetKey(KeyCode.W))
        {
            if(_wallLeft)
            {
                _rb.AddForce((orientation.right) * _playerVariables.wallRunKnockback, ForceMode.Impulse);
                Debug.Log("Push from wall");
            }

            else if(_wallRight)
            {
                _rb.AddForce((-orientation.right) * _playerVariables.wallRunKnockback, ForceMode.Impulse);
                Debug.Log("Push from wall");
            }
            
        }

        WallJump();
    }

    // Prüft, ob sich rechts und links vom Spieler Wände befinden
    public void WallCheck()
    {
        if(Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, _playerVariables.wallRunDistance))
        {
            _wallLeft = true;
            Debug.DrawRay(transform.position, -orientation.right, Color.green);
        }
        
        else if(Physics.Raycast(transform.position, orientation.right, out _rightWallHit, _playerVariables.wallRunDistance))
        {
            _wallRight = true;
            Debug.DrawRay(transform.position, orientation.right, Color.red);
        }

        else
        {
            _wallLeft = false;
            _wallRight = false;
        }
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(transform.position.y >= _playerVariables.wallRunHeight)
        {
            if (collision.transform.CompareTag("RunnableWall"))
            {
                if (isWallRunning && _wallLeft)
                {
                    _playerVariables.gravityScale = 0;
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,-30), 2);
                }

                else if (isWallRunning && _wallRight)
                {
                    _playerVariables.gravityScale = 0;
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,30), 2);
                }
                _rb.velocity = new Vector3(0, 0, 0);

            }
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("RunnableWall"))
        {
            isWallRunning = true;
            Debug.Log("Current gravityScale: " + _playerVariables.gravityScale);
        }

        Debug.DrawRay(transform.position, -orientation.right*10, Color.magenta);

        
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.CompareTag("RunnableWall"))
        {
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,0), 2);
            isWallRunning = false;
        }


    }

    // Steuert Walljumps während des Wallruns
    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isWallRunning)
        {
            soundEffects.PlayJumpSound();
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            if (_wallLeft)
            {
                _rb.AddForce(Vector3.up * _playerVariables.jumpForceFP + orientation.right * _playerVariables.wallJumpSideForce, ForceMode.VelocityChange);
            }

            else if (_wallRight)
            {
                _rb.AddForce(Vector3.up * _playerVariables.jumpForceFP + -orientation.right * _playerVariables.wallJumpSideForce, ForceMode.VelocityChange);
            }
        }
    }
}
