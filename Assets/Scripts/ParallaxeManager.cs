using UnityEngine;

public class ParallaxeManager : MonoBehaviour
{
    [SerializeField] private Sprite Layer1Prefab;
    [SerializeField] private Sprite Layer2Prefab;
    [SerializeField] private Sprite Layer3Prefab;
    [SerializeField] private Sprite Layer4Prefab;
    [SerializeField] private Sprite Layer5Prefab;

    [SerializeField] private float parallaxSpeed;
    [SerializeField] private float parallaxSpeedOffset;

    [Tooltip("Longueur totale du niveau attendue en unités monde")]
    [SerializeField] private float levelLength = 100f;

    private void Start()
    {
        CreateLayer(Layer1Prefab, 0.2f);
        CreateLayer(Layer2Prefab, 0.4f);
        CreateLayer(Layer3Prefab, 0.6f);
        CreateLayer(Layer4Prefab, 0.8f);
        CreateLayer(Layer5Prefab, 1f);
    }

    private void CreateLayer(Sprite layer, float speedFactor)
    {
        float spriteWidth = layer.bounds.size.x;
        
        // Calcule le nombre de tuiles requises pour remplir la longueur du niveau (minimum 2 pour le loop)
        int tileCount = Mathf.Max(2, Mathf.CeilToInt(levelLength / spriteWidth) + 1);

        GameObject layerRoot = new GameObject(layer.name);
        layerRoot.transform.SetParent(transform);
        layerRoot.transform.localPosition = Vector3.zero;
        layerRoot.transform.localScale = Vector3.one;

        for (int i = 0; i < tileCount; i++)
        {
            GameObject tile = new GameObject($"{layer.name}_tile{i}");
            tile.transform.SetParent(layerRoot.transform);
            tile.transform.localPosition = new Vector3(spriteWidth * i, 0f, 0f);
            tile.transform.localScale = Vector3.one;
            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = layer;
            sr.sortingOrder = (int)(speedFactor * 5) - 4;
        }

        ParallaxLayer pl = layerRoot.AddComponent<ParallaxLayer>();
        pl.speed = parallaxSpeed + speedFactor * parallaxSpeedOffset;
        pl.spriteWidth = spriteWidth;
    }
}

internal class ParallaxLayer : MonoBehaviour
{
    public float speed;
    public float spriteWidth;

    private Transform[] _tiles;
    private Vector3[] _startLocalPos;
    private float _movedDistance = 0f;

    private void Start()
    {
        _tiles = new Transform[transform.childCount];
        _startLocalPos = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _tiles[i] = transform.GetChild(i);
            // Sauvegarde de l'ancrage parfait généré par votre script
            _startLocalPos[i] = _tiles[i].localPosition; 
        }
    }

    private void Update()
    {
        // On cumule la distance parcourue
        _movedDistance += speed * Time.deltaTime;

        // Dès qu'on a avancé de la taille exacte d'un sprite, on boucle la distance
        if (_movedDistance >= spriteWidth)
            _movedDistance -= spriteWidth;
        else if (_movedDistance <= -spriteWidth)
            _movedDistance += spriteWidth;

        // On applique ce décalage local par rapport aux positions de départ intactes
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].localPosition = _startLocalPos[i] + (Vector3.left * _movedDistance);
        }
    }
}
