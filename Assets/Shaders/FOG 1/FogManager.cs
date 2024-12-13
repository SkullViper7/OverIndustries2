using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    MeshRenderer _renderer;
    MaterialPropertyBlock _materialPropertyBlock;

    [SerializeField] float _farFade;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new();
    }

    private void Update()
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetFloat("_SoftParticlesFarFadeDistance", _farFade);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
