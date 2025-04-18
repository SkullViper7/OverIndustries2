using DG.Tweening;
using UnityEngine;

public class InteractionButtonGroup : MonoBehaviour
{
    [SerializeField]
    private float _time;

    private RectTransform _rectTransform;
    private Vector2 _startPos;

    private Sequence _showButtonsSequence;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _startPos = new Vector2(_rectTransform.anchoredPosition.x, -_rectTransform.rect.height);

        _rectTransform.anchoredPosition = _startPos;
    }

    public void ShowButtons()
    {
        HideButtons();

        _showButtonsSequence = DOTween.Sequence();
        _showButtonsSequence.Append(_rectTransform.DOAnchorPos(new Vector2(_rectTransform.anchoredPosition.x, 0), _time)).SetEase(Ease.InExpo).OnComplete(() =>
        {
            CancelSequence();
        });
    }

    public void HideButtons()
    {
        CancelSequence();

        _rectTransform.anchoredPosition = _startPos;
    }

    /// <summary>
    /// Call to cancel a sequence if there is one.
    /// </summary>
    private void CancelSequence()
    {
        if (_showButtonsSequence != null)
        {
            _showButtonsSequence.Kill();
            _showButtonsSequence = null;
        }
    }
}
