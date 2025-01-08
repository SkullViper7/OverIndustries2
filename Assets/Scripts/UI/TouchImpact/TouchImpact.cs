using System;
using UnityEngine;

public class TouchImpact : MonoBehaviour
{
    private Animator _animator;

    public Action<GameObject> ImpactFinished;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play("Impact");
    }

    public void FinishImpact()
    {
        ImpactFinished?.Invoke(gameObject);
    }
}
