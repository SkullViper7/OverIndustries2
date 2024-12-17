using UnityEngine;

public class BlurManager : MonoBehaviour
{
    public static BlurManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }       
    }

    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowBlur()
    {
        _animator.Play("Blur");
    }

    public void HideBlur()
    {
        _animator.Play("Unblur");
    }
}
