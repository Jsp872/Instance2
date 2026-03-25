using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoteAnimation : MonoBehaviour
{
    [Header("Rotation Animation Parameters")]
    [SerializeField] private float rotationZ = 10f;
    [SerializeField] private float rotationDuration = 0.7f;
    [SerializeField] private RotateMode rotationMode = RotateMode.FastBeyond360;
    [SerializeField] private Ease rotationEase = Ease.Linear;
    [SerializeField] private int rotationLoops = -1;
    [SerializeField] private LoopType rotationLoopType = LoopType.Yoyo;

    [Header("Scale Animation Parameters")]
    [SerializeField] private float scaleTo = 1.2f;
    [SerializeField] private float scaleDuration = 1f;
    [SerializeField] private Ease scaleEase = Ease.Linear;
    [SerializeField] private int scaleLoops = -1;
    [SerializeField] private LoopType scaleLoopType = LoopType.Yoyo;

    [Header("Move Y Animation Parameters")]
    [SerializeField] private float moveYOffset = 40f;
    [SerializeField] private float moveYDuration = 1f;
    [SerializeField] private Ease moveYEase = Ease.InOutCubic;
    [SerializeField] private int moveYLoops = -1;
    [SerializeField] private LoopType moveYLoopType = LoopType.Yoyo;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.transform.DORotate(new Vector3(0, 0, rotationZ), rotationDuration, rotationMode)
            .SetEase(rotationEase)
            .SetLoops(rotationLoops, rotationLoopType);
        image.transform.DOScale(scaleTo, scaleDuration)
            .SetEase(scaleEase)
            .SetLoops(scaleLoops, scaleLoopType);
        var rectTransform = image.GetComponent<RectTransform>();
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + moveYOffset, moveYDuration)
            .SetEase(moveYEase)
            .SetLoops(moveYLoops, moveYLoopType);
    }
}
