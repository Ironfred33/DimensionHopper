using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Material _flickerMaterial;
    private Color _emissionColor;

    [SerializeField] private float _minEmission;
    [SerializeField] private float _maxEmission;
    [SerializeField] private float _timeBetween;
    private float _emission;
    private void Start() 
    {
        _renderer = GetComponent<MeshRenderer>();
        _flickerMaterial = _renderer.materials[1];
        _emissionColor = _flickerMaterial.color;
    }
    void Update()
    {
        _emission = _minEmission + Mathf.PingPong(Time.time*_timeBetween, _maxEmission);

        _emissionColor = Color.yellow * Mathf.LinearToGammaSpace(_emission);

        _flickerMaterial.SetColor("_EmissionColor", _emissionColor);
        
        Debug.Log("Current Emission: " + _emission);
    }

}
