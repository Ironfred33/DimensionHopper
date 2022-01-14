using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSight : MonoBehaviour
{

    public Camera camera;

    public float range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.V))
        {
            ShootRay();
        }
        
    }


    void ShootRay()
    {

        RaycastHit hit;

        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

        }

    }
}
