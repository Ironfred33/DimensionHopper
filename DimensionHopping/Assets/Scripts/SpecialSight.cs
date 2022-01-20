using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSight : MonoBehaviour
{

    public Camera camera;

    public TransformPositionOnPerspective scriptPGO;

    public Vector3 transformFirstPoint;
    public Vector3 transformSecondPoint;

    public GameObject copy;

    public bool finishedCoolDown;

    public bool finishedSightTime;


    public float coolDown;
    public float sightTime;
    public float range;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            ShootRay();
        }

    }


    void ShootRay()
    {

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {

            if (hit.transform.tag == "PGOxPositive" || hit.transform.tag == "PGOzPositive" || hit.transform.tag == "PGOxNegative" || hit.transform.tag == "PGOzNegative")
            {

                Debug.Log(hit.transform.tag);

                scriptPGO = hit.transform.GetComponent<TransformPositionOnPerspective>();

                transformFirstPoint = scriptPGO.transformFirstPoint;
                transformSecondPoint = scriptPGO.transformSecondPoint;

                CreateCopy(hit);

            }

        }

    }

    void CreateCopy(RaycastHit hit)
    {

        // gibt alle gameobjects zurück, auch den player, wenn er zu dem zeitpunkt auf der plattform steht
        copy = hit.transform.gameObject;

        if(hit.collider.gameObject.transform.position == transformFirstPoint) Instantiate(copy, transformSecondPoint, Quaternion.identity);
        else if(hit.collider.gameObject.transform.position == transformSecondPoint) Instantiate(copy, transformFirstPoint, Quaternion.identity);
        

    }

    IEnumerator WaitForSecond(float time)
    {

        return null;
    }


}
