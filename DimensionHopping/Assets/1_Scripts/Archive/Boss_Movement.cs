using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Movement : MonoBehaviour
{
    private GameObject _player;

    public float distanceToPlayer;
    public float floatingRange;
    public float floatingSpeed;
    private float _floatingMin;
    private float _floatingMax;
    public float followDelay;
    // FALSE = UNTERSTER PUNKT AN DEM FLOATING GESTARTET WIRD ---- TRUE = OBERSTER PUNKT, ABHÄNGIG VON FLOATINGRANGE
    [SerializeField] private bool _floated;
    public float floatingAmount;


    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");


        _floatingMax = floatingAmount / 2;
        _floatingMin = -1 * (floatingAmount / 2);


        StartCoroutine(FloatingCoroutine());


    }


    // Update is called once per frame
    void Update()
    {

        FollowPlayer();

    }


    void FollowPlayer()
    {

        this.transform.position = Vector3.Lerp(transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y + floatingAmount, _player.transform.position.z + distanceToPlayer), followDelay);

    }

    IEnumerator FloatingCoroutine()
    {
        float elapsedTime = 0;



        if (!_floated)
        {

            while (elapsedTime < floatingSpeed)
            {


                floatingAmount = Mathf.Lerp(_floatingMin, _floatingMax, floatingSpeed);

                elapsedTime += Time.deltaTime;

                yield return null;

            }
        }
        else if (_floated)
        {

            while (elapsedTime < floatingSpeed)
            {

                floatingAmount = Mathf.Lerp(_floatingMax, _floatingMin, floatingSpeed);


                elapsedTime += Time.deltaTime;

                yield return null;

            }

        }

        if (_floated) _floated = false;
        else if (!_floated) _floated = true;


        StartCoroutine(FloatingCoroutine());



        yield return null;
    }



}
