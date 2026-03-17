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

    [Header("Debug")]
    [Tooltip("Active ou désactive les logs de debug pour cette note.")]
    [SerializeField] private bool debugLog = false;

    private void Awake()
    {
        noteImage = GetComponent<Image>();

        if (noteImage == null)
        {
            Debug.LogError($"[Note] Aucun composant Image trouvé sur {gameObject.name}. Attacher un Image sur ce GameObject.", this);
        }
        else
        {
            if (debugLog)
                Debug.Log($"[Note] Awake → Image trouvée sur '{gameObject.name}'.", this);
        }
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
        if (debugLog)
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
        if (debugLog)
            Debug.Log($"[Note] Show() → '{gameObject.name}' affichée.", this);
    }

    /// <summary>
    /// Définit la couleur de la note (utilisée lors de l'activation).
    /// </summary>
    public void SetColor(Color c)
    {
        noteColor = c;
    }

    /// <summary>
    /// Anime la note pour la mettre en avant (taille/couleur).
    /// </summary>
    public void Enable()
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(100, 100), 0.5f).SetEase(Ease.OutBack);
        noteImage.DOColor(noteColor, 0.5f).SetEase(Ease.OutQuad);
        if (debugLog)
            Debug.Log($"[Note] Enable() → '{gameObject.name}' animée en avant.", this);
    }

    /// <summary>
    /// Anime la note pour la désactiver visuellement (taille/couleur grise).
    /// </summary>
    public void Disable()
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(20, 20), 0.5f).SetEase(Ease.OutBack);
        noteImage.DOColor(Color.gray, 0.5f).SetEase(Ease.OutQuad);
        if (debugLog)
            Debug.Log($"[Note] Disable() → '{gameObject.name}' désactivée visuellement.", this);
    }

    /// <summary>
    /// Réinitialise la taille de la note à la valeur par défaut.
    /// </summary>
    public void Reset()
    {
        noteImage.rectTransform.DOSizeDelta(new Vector2(50, 50), 0.5f).SetEase(Ease.OutBack);
        if (debugLog)
            Debug.Log($"[Note] Reset() → '{gameObject.name}' taille réinitialisée.", this);
    }
}
