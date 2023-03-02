using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Movement : MonoBehaviour
{

    public float distanceToPlayer;

    public float floatingRange;
    public float floatingSpeed;

    private float _floatingMin;
    private float _floatingMax;

    private GameObject _player;
    public float followDelay;

    // private Vector3 _floatMax;
    // private Vector3 _floatMin;

    // FALSE = UNTERSTER PUNKT AN DEM FLOATING GESTARTET WIRD ---- TRUE = OBERSTER PUNKT, ABHÄNGIG VON FLOATINGRANGE
    [SerializeField] private bool _floated;
    public float floatingAmount;




    // ------------------------------------------------TODO
    // floatingAmount mit Kurve geilo machen?




    // Start is called before the first frame update
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


        // POSITION 

        this.transform.position = Vector3.Lerp(transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y + floatingAmount, _player.transform.position.z + distanceToPlayer), followDelay);

        //this.transform.rotation = new Quaternion(0, 90, 0, 0);
        
       //(new Vector3(0, 0, 0));
        
      

        //this.transform.rotation = new Quaternion( 0, _player.transform.rotation.y, 0, 0);

        

    }

    IEnumerator FloatingCoroutine()
    {
        float elapsedTime = 0;



        if (!_floated)
        {

            while (elapsedTime < floatingSpeed)
            {


                floatingAmount = Mathf.Lerp(_floatingMin, _floatingMax, floatingSpeed);

                

                //this.transform.position = Vector3.Lerp(_floatingMinPos, _floatingMaxPos, (elapsedTime / floatingSpeed));


                elapsedTime += Time.deltaTime;

                yield return null;

            }
        }
        else if (_floated)
        {

            while (elapsedTime < floatingSpeed)
            {

                floatingAmount = Mathf.Lerp(_floatingMax, _floatingMin, floatingSpeed);

                // this.transform.position = Vector3.Lerp(_floatingMaxPos, _floatingMinPos, (elapsedTime / floatingSpeed));


                elapsedTime += Time.deltaTime;

                yield return null;

            }

        }

        if (_floated) _floated = false;
        else if (!_floated) _floated = true;


        StartCoroutine(FloatingCoroutine());



        yield return null;
    }


    void Floating()
    {
        if (!_floated)
        {
            floatingAmount = Mathf.Lerp(_floatingMin, _floatingMax, floatingSpeed);
            _floated = true;
        }
        else if (_floated)
        {
            floatingAmount = Mathf.Lerp(_floatingMax, _floatingMin, floatingSpeed);
            _floated = false;
        }


    }



}
