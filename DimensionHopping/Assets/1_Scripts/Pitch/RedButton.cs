using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{

    public CameraController camControl;


    public KeyCode buttonPress;
    private Rigidbody blockRigidbody;
    public GameObject block;
    public GameObject player;


    public float interactionRadius;
    public bool enemiesDefeated = false;


    void Start()
    {
        blockRigidbody = block.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= interactionRadius && Input.GetKeyDown(buttonPress) && !enemiesDefeated)
        {
            StartCoroutine(PressButton(blockRigidbody));
        }
    }

    public IEnumerator PressButton(Rigidbody blockRigidbody)
    {
        blockRigidbody.isKinematic = false;

        yield return new WaitForSeconds(3f);

        if (!enemiesDefeated)
        {
            if (!camControl.is2DView)
            {
                blockRigidbody.isKinematic = true;
                yield return new WaitForSeconds(1f);
                block.transform.position = block.GetComponent<Block>().positionFPP;

            }

            else
            {
                yield break;
            }
        }

        else
        {
            yield return null;
        }

        

    }
}
