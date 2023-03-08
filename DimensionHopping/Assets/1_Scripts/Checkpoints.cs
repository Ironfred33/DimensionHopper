using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkpoints : MonoBehaviour
{

    [SerializeField] private EVPlayer _externalVariables;

    [SerializeField] private AudioClip _checkpointSound;
    [SerializeField] private GameObject _checkpointDisplay;
    private Vector3 _currentCP;
    private MeshRenderer _renderer;
    
    private float _currentOpacity;
    [SerializeField] private string _checkpointText;
    [SerializeField] private float _displayTime;

    private void Start()
    {
        _checkpointDisplay.GetComponent<TMP_Text>().text = _checkpointText;
        _renderer = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
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
