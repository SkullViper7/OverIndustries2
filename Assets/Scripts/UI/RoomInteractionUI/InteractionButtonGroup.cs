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

        _startPos = new Vector2(_rectTransform.anchoredPosition.x, -_rectTransform.rect.height);

        _rectTransform.anchoredPosition = _startPos;
    }

    public void ShowButtons()
    {
        HideButtons();

        _showButtonsSequence = DOTween.Sequence();
        _showButtonsSequence.Append(_rectTransform.DOAnchorPos(_startPos, _time)).SetEase(Ease.InExpo).OnComplete(() =>
        {
            CancelSequence();
        });
    }

    public void HideButtons()
    {
        CancelSequence();

        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 0);
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
