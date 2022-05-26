using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert Wallrun des Spieler-Charakters
public class WallRun_v2 : MonoBehaviour
{
    public Transform orientation;
    public Camera cam;
    public float wallDistance;

    public bool _wallLeft = false;
    public bool _wallRight = false;
    public bool isWallRunning = false;

    public float upForce;
    public float sideForce;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    public float minHeight;

    public float wallRunFOV;
    public float normalFOV;

    private Rigidbody _rb;

    private SFX _soundEffects;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        _soundEffects = GetComponent<SFX>();
    }

    void FixedUpdate()
    {
        WallCheck();
    }

    private void Update()
    {

        if (isWallRunning && _wallLeft)
        {
            cam.transform.Rotate(0f, 0f, -30f);
            Debug.Log("Left Tilt");
        }

        else if (isWallRunning && _wallRight)
        {
            cam.transform.Rotate(0f, 0f, 30f);
            Debug.Log("Right Tilt");
        }

        WallJump();
    }

    // Prüft, ob sich rechts und links vom Spieler Wände befinden
    public void WallCheck()
    {
        _wallLeft = Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, wallDistance);
        Debug.DrawRay(transform.position, -orientation.right, Color.green);

        _wallRight = Physics.Raycast(transform.position, orientation.right, out _rightWallHit, wallDistance);
        Debug.DrawRay(transform.position, orientation.right, Color.red);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(transform.position.y >= minHeight)
        {
            if (collision.transform.CompareTag("RunnableWall"))
            {

                cam.fieldOfView = wallRunFOV;

                _rb.velocity = new Vector3(0, 0, 0);
                _rb.useGravity = false;

            }
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("RunnableWall"))
        {
            isWallRunning = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.CompareTag("RunnableWall"))
            {
            cam.fieldOfView = normalFOV;
            cam.transform.rotation = Quaternion.Euler(0, 90, 0);

            _rb.useGravity = true;
            isWallRunning = false;
            }

    }

    // Steuert Walljumps während des Wallruns
    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isWallRunning)
        {
            _soundEffects.PlayJumpSound();
            if (_wallLeft)
            {
                Debug.Log("Wall is left");
                _rb.AddForce(Vector3.up * upForce * Time.deltaTime, ForceMode.Impulse);
                _rb.AddForce(orientation.right * sideForce * Time.deltaTime, ForceMode.Impulse);
                Debug.Log("Wall Jump");
            }

            else if (_wallRight)
            {
                _rb.AddForce(Vector3.up * upForce * Time.deltaTime, ForceMode.Impulse);
                _rb.AddForce(-orientation.right * sideForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
