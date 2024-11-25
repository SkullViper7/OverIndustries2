using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MaintainAspectRatio : MonoBehaviour
{
    [SerializeField] private float aspectRatio = 1f; // Aspect ratio (largeur/hauteur), 1 pour un carré

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Ajuste la largeur en fonction de la hauteur
        float height = rectTransform.rect.height;
        rectTransform.sizeDelta = new Vector2(height * aspectRatio, height);
    }
}
