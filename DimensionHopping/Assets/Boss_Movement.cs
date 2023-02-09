using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Movement : MonoBehaviour
{

    public float distanceToPlayer;

    public float floatingRange;
    public float floatingSpeed;

    private GameObject _player;
    public float followDelay;

    // private Vector3 _floatMax;
    // private Vector3 _floatMin;

    // FALSE = UNTERSTER PUNKT AN DEM FLOATING GESTARTET WIRD ---- TRUE = OBERSTER PUNKT, ABHÄNGIG VON FLOATINGRANGE
    [SerializeField] private bool _floated;
    private Vector3 _floatingMinPos;
    private Vector3 _floatingMaxPos;




    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");

        // _floatMax = new Vector3(this.transform.position.x, this.transform.position.y + (0.5f * floatingRange), this.transform.position.z);

        // _floatMin = new Vector3(this.transform.position.x, this.transform.position.y - (0.5f * floatingRange), this.transform.position.z);

        // this.transform.position = _floatMax;

        _floatingMinPos = this.transform.position;

        _floatingMaxPos = new Vector3(this.transform.position.x, this.transform.position.y + floatingRange, this.transform.position.z);

        //StartCoroutine(Floating());


    }


    // Update is called once per frame
    void Update()
    {

        FollowPlayer();
        // Floating();

    }



    void FollowPlayer()
    {

        this.transform.position = Vector3.Lerp(transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z + distanceToPlayer), followDelay);

        // this.transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y, _player.transform.position.z + distanceToPlayer);


    }

    IEnumerator Floating()
    {
        float elapsedTime = 0;



        if (!_floated)
        {

            while (elapsedTime < floatingSpeed)
            {

                this.transform.position = Vector3.Lerp(_floatingMinPos, _floatingMaxPos, (elapsedTime / floatingSpeed));


                elapsedTime += Time.deltaTime;

                yield return null;

            }
        }
        else if (_floated)
        {

            while (elapsedTime < floatingSpeed)
            {

                this.transform.position = Vector3.Lerp(_floatingMaxPos, _floatingMinPos, (elapsedTime / floatingSpeed));


                elapsedTime += Time.deltaTime;

                yield return null;

            }

        }

        if(_floated) _floated = false;
        else if (!_floated) _floated = true;


        //ToggleBool(_floated);

        StartCoroutine(Floating());


    


    // while (elapsedTime < floatTime)
    // {




    //     elapsedTime += Time.deltaTime;

    //     yield return null;

    // }


    yield return null;
    }


    bool ToggleBool(bool boolean)
    {
        if(boolean) boolean = false;
        else if (!boolean) boolean = true;

        return boolean;
    }

    // void Floating()
    // {
    //     if(this.transform.position == _floatMax)
    //     {
    //         this.transform.position = Vector3.Lerp(transform.position, _floatMin, floatingSpeed);
    //     }
    //     else if (this.transform.position == _floatMax)
    //     {
    //         this.transform.position = Vector3.Lerp(transform.position, _floatMax, floatingSpeed);
    //     }



    // }

   
}
