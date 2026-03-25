using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Représente une note visuelle dans l'UI (élément d'une séquence Simon).
/// Gère uniquement la visibilité de son Image : Hide() la rend transparente,
/// Show() la rend opaque.
/// </summary>
public class Note : MonoBehaviour
{
    private Image noteImage;
    private Color noteColor;
    private RectTransform rectTransform;

    [Header("Debug")] [Tooltip("Active ou désactive les logs de debug pour cette note.")] [SerializeField]
    private bool debugLogs = false;

    private void Awake()
    {
        noteImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        if(TryGetComponent(out Canvas canvas))
        {
            canvas.sortingOrder = -10;
        }

        if (noteImage == null)
        {
            Debug.LogError(
                $"[Note] Aucun composant Image trouvé sur {gameObject.name}. Attacher un Image sur ce GameObject.",
                this);
        }
        else
        {
            if (debugLogs)
                Debug.Log($"[Note] Awake → Image trouvée sur '{gameObject.name}'.", this);
        }
    }

    /// <summary>
    /// Démarre le mouvement de la note sur l'axe X.
    /// </summary>
    public void StartMove(float speed, float trackSize)
    {
        rectTransform.DOAnchorPosX(-trackSize, trackSize / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
            if (debugLogs)
                Debug.Log($"[Note] StartMove() → '{gameObject.name}' a atteint la fin du track et a été détruite.", this);
        });
    }

    /// <summary>
    /// Rend la note invisible (alpha = 0) et stoppe tous les tweens en cours pour éviter les conflits d'animation.
    /// </summary>
    public void Hide()
    {
        // Stoppe tous les tweens DOTween actifs sur cette note (taille et couleur)
        DOTween.Kill(noteImage.rectTransform);
        DOTween.Kill(noteImage);
        Color c = noteImage.color;
        c.a = 0;
        noteImage.color = c;
        
        if (debugLogs)
            Debug.Log($"[Note] Hide() → '{gameObject.name}' masquée.", this);
    }

    /// <summary>
    /// Rend la note visible (alpha = 1).
    /// </summary>
    public void Show()
    {
        Color c = noteImage.color;
        c.a = 1;
        noteImage.color = c;
        if (debugLogs)
            Debug.Log($"[Note] Show() → '{gameObject.name}' affichée.", this);
    }

    /// <summary>
    /// Définit la couleur de la note (utilisée lors de l'activation).
    /// </summary>
    public void SetColor(Color c)
    {
        noteColor = c;
        noteImage.color = c;
        if (debugLogs) Debug.Log($"[Note] SetColor({c}) → '{gameObject.name}' couleur définie.", this);
    }
    
    /// <summary>
    /// Définit le sprite de la note (utilisée lors de l'activation).
    /// </summary>
    public void SetSprite(Sprite c)
    {
        noteImage.sprite = c;
        if (debugLogs) Debug.Log($"[Note] SetColor({c}) → '{gameObject.name}' couleur définie.", this);
    }

    /// <summary>
    /// Anime la note pour la mettre en avant (taille/couleur).
    /// </summary>
    public void Enable(float factor = 1)
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(100 * factor, 100 * factor), 0.5f).SetEase(Ease.OutBack);
        noteImage.DOColor(noteColor, 0.5f).SetEase(Ease.OutQuad);
        if (debugLogs)
            Debug.Log($"[Note] Enable() → '{gameObject.name}' animée en avant.", this);
    }

    /// <summary>
    /// Anime la note pour la désactiver visuellement (taille/couleur grise).
    /// </summary>
    public void Disable()
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(20, 20), 0.5f).SetEase(Ease.OutBack);
        noteImage.DOColor(Color.gray, 0.5f).SetEase(Ease.OutQuad);
        if (debugLogs)
            Debug.Log($"[Note] Disable() → '{gameObject.name}' désactivée visuellement.", this);
    }

    /// <summary>
    /// Réinitialise la taille de la note à la valeur par défaut.
    /// </summary>
    public void Reset()
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(50, 50), 0.5f).SetEase(Ease.OutBack);
        noteImage.DOColor(noteColor, 0.5f).SetEase(Ease.OutQuad);
        if (debugLogs)
            Debug.Log($"[Note] Reset() → '{gameObject.name}' taille réinitialisée.", this);
    }
}