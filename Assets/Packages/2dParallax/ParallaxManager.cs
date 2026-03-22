//Author: Small Hedge Games
//Date: 13/06/2024

using UnityEngine;
using System;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private Background[] backgrounds;
    [SerializeField] private bool isBackground;
    [SerializeField] private bool changeAnchorsOnStart;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private Transform anchor;

    public Transform gameCamera;
    public static ParallaxManager instance;
    private Vector3 anchorPosition;
    private Camera cam;
    private Transform loopContainer;

    private void Awake()
    {
        instance = this;
        cam = gameCamera != null ? gameCamera.GetComponent<Camera>() : Camera.main;
    }

    private void Start()
    {
        if (!anchor) anchorPosition = gameCamera.position;

        if (changeAnchorsOnStart)
            for (int i = 0; i < backgrounds.Length; i++)
                backgrounds[i].anchor = backgrounds[i].sprite.position;

        // Conteneur séparé pour ne pas polluer les enfants directs (ResetBackgrounds)
        GameObject container = new GameObject("_LoopDuplicates");
        container.transform.SetParent(transform);
        loopContainer = container.transform;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (!backgrounds[i].loop || backgrounds[i].sprite == null) continue;

            SpriteRenderer sr = backgrounds[i].sprite.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            backgrounds[i].spriteWidth = sr.bounds.size.x;

            GameObject dup = Instantiate(backgrounds[i].sprite.gameObject, loopContainer);
            dup.name = backgrounds[i].sprite.name + "_Loop";
            backgrounds[i].duplicate = dup.transform;
        }
    }

    private void Update()
    {
        float cameraHalfWidth = cam != null ? cam.orthographicSize * cam.aspect : 10f;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (anchor) anchorPosition = anchor.position;

            float adjustedIntensity = backgrounds[i].intensity;
            if (isBackground) --adjustedIntensity;

            Vector2 pos = (Vector2)anchorPosition + backgrounds[i].anchor +
                          new Vector2(-adjustedIntensity, adjustedIntensity * heightMultiplier) *
                          (gameCamera.position - anchorPosition);

            backgrounds[i].sprite.position = pos;

            if (!backgrounds[i].loop || backgrounds[i].duplicate == null) continue;

            // Le duplicata est toujours collé à droite du sprite principal
            backgrounds[i].duplicate.position = pos + new Vector2(backgrounds[i].spriteWidth, 0);

            // Si le sprite principal est entièrement sorti à gauche → téléportation à droite
            float cameraLeft = gameCamera.position.x - cameraHalfWidth;
            if (pos.x + backgrounds[i].spriteWidth * 0.5f < cameraLeft)
            {
                Vector2 bgAnchor = backgrounds[i].anchor;
                bgAnchor.x += backgrounds[i].spriteWidth;
                backgrounds[i].anchor = bgAnchor;
            }
        }
    }

    public void ChangeAnchor(int index, Vector2 anchor)
    {
        backgrounds[index].anchor = anchor;
    }

    public void ResetBackgrounds()
    {
        // On exclut le conteneur "_LoopDuplicates" du comptage
        int realChildCount = 0;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name != "_LoopDuplicates")
                realChildCount++;

        Array.Resize(ref backgrounds, realChildCount);
        int index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == "_LoopDuplicates") continue;

            if (!backgrounds[index].sprite)
                backgrounds[index].intensity = GetIntensity(index);

            backgrounds[index].sprite = child;
            backgrounds[index].name = child.name;
            index++;
        }
    }

    public void ResetIntensities()
    {
        for (int i = 0; i < backgrounds.Length; ++i)
        {
            backgrounds[i].intensity = (float)(i + 1) / (backgrounds.Length + 1);
            backgrounds[i].loop = true;
        }
    }

    public float GetIntensity(int index)
    {
        return backgrounds[index].intensity;
    }
}

[Serializable]
public struct Background
{
    [HideInInspector] public string name;
    [Range(0, 1)] public float intensity;
    public Transform sprite;
    public Vector2 anchor;
    public bool loop;                        // ← cocher dans l'Inspector
    [HideInInspector] public Transform duplicate;
    [HideInInspector] public float spriteWidth;
}
