using DG.Tweening;
using UnityEngine;

/// <summary>
/// Plateforme enfant d'un Obstacle qui reste au sol par défaut et monte
/// verticalement dès que le joueur valide la séquence Simon.
/// Le joueur doit sauter dessus pour franchir l'obstacle.
///
/// Setup requis :
///   • Attacher ce composant sur la plateforme (enfant de l'Obstacle parent).
///   • L'Obstacle parent (ou l'un de ses enfants) doit porter un ObstaclePlayerDetector.
/// </summary>
public class RisingPlatform : MonoBehaviour
{
    // ─── Inspector ───────────────────────────────────────────────────────────

    [Header("Mécanique de la plateforme")] [SerializeField]
    private float overDrive = 1f; // Multiplicateur de vitesse de montée

    [SerializeField] private float vitesse = 5f; // Vitesse générale du mouvement (unités/s)
    [SerializeField] private float hauteur = 4f; // Distance maximale de montée (unités)

    // ─── Privé ───────────────────────────────────────────────────────────────

    private ObstaclePlayerDetector obstaclePlayerDetector;

    // ─── Lifecycle ───────────────────────────────────────────────────────────

    private void Awake()
    {
        obstaclePlayerDetector = transform.GetComponentInChildren<ObstaclePlayerDetector>();

        if (obstaclePlayerDetector == null)
            Debug.LogWarning(
                $"[RisingPlatform] Aucun ObstaclePlayerDetector trouvé dans le parent de {gameObject.name}.");
    }

    private void OnEnable()
    {
        if (obstaclePlayerDetector == null) return;
        obstaclePlayerDetector.unlocked += OnUnlock;
    }

    private void OnDisable()
    {
        if (obstaclePlayerDetector == null) return;
        obstaclePlayerDetector.unlocked -= OnUnlock;
    }

    // ─── Déverrouillage ──────────────────────────────────────────────────────

    private void OnUnlock()
    {
        float targetY = transform.position.y + hauteur;
        float duration = hauteur / (vitesse * overDrive);

        transform.DOMoveY(targetY, duration).SetEase(Ease.OutQuad);
    }
}