using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkpoints : MonoBehaviour
{
    private Vector3 _currentCP;
    [SerializeField] private EVPlayer _externalVariables;
    [SerializeField] private AudioClip _checkpointSound;
    [SerializeField] private float _displayTime;
    [SerializeField] private GameObject _checkpointDisplay;
    [SerializeField] private string _checkpointText;
    private MeshRenderer _renderer;
    private float _currentOpacity;

    private void Start() 
    {
            _checkpointDisplay.GetComponent<TMP_Text>().text = _checkpointText;
            _renderer = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.CompareTag("Player"))
        {
            StartCoroutine(DisplayCheckpointText());
        }    
    }

    private IEnumerator DisplayCheckpointText()
    {
        _checkpointDisplay.SetActive(true);
        _renderer.sharedMaterial.SetFloat("Opacity", 0); 
        yield return new WaitForSeconds(_displayTime);
        _checkpointDisplay.SetActive(false);
        _externalVariables.spawnPoint = this.transform.position;
        this.gameObject.SetActive(false);
        _renderer.sharedMaterial.SetFloat("Opacity", 0.6f); 
    }
}
