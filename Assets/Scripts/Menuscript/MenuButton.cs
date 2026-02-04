using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Events")]
    public UnityEvent onClick;

    [Header("Options")]
    public bool haveDelay = true;
    public bool changeScale = true;

    [Header("Animation")]
    [SerializeField] float hoverWidthMultiplier = 1.5f;
    [SerializeField] float clickWidthMultiplier = 1.6f;
    [SerializeField] float hoverDuration = 0.15f;
    [SerializeField] float clickDuration = 0.5f;

    RectTransform rect;
    float defaultWidth;
    Vector3 defaultScale;

    Image leftTriangle;
    Image rightTriangle;
    Image mainBg;
    TextMeshProUGUI[] texts;

    Color bgColor;
    Color textColor;

    bool isClicked = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        defaultWidth = rect.sizeDelta.x;
        defaultScale = transform.localScale;

        leftTriangle = transform.Find("LeftTriangleTHing").GetComponent<Image>();
        rightTriangle = transform.Find("RightTriangleThing").GetComponent<Image>();
        mainBg = transform.Find("MainBg").GetComponent<Image>();
        texts = mainBg.GetComponentsInChildren<TextMeshProUGUI>();

        bgColor = mainBg.color;
        textColor = texts[0].color;
    }

    #region Pointer Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isClicked) return;

        KillTweens();
        ResizeWidth(hoverWidthMultiplier, hoverDuration);
        BringToFront();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked) return;

        KillTweens();
        ResetSize();
        ResetScale();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return;
        isClicked = true;

        KillTweens();
        ClickStretch();
        ClickScale();
        FlashWhite();
        StartCoroutine(ClickDelay());
    }
    #endregion

    #region Animations
    void ResizeWidth(float multiplier, float duration)
    {
        rect.DOSizeDelta(
            new Vector2(defaultWidth * multiplier, rect.sizeDelta.y),
            duration
        )
        .SetEase(Ease.OutBack)
        .SetLink(gameObject);
    }

    void ResetSize()
    {
        ResizeWidth(1f, hoverDuration);
    }

    void ClickStretch()
    {
        rect.sizeDelta = new Vector2(defaultWidth * clickWidthMultiplier, rect.sizeDelta.y);
        ResizeWidth(hoverWidthMultiplier, clickDuration);
    }

    void ClickScale()
    {
        if (!changeScale) return;

        transform.localScale = defaultScale * 1.3f;
        transform.DOScale(defaultScale, clickDuration)
            .SetEase(Ease.OutExpo)
            .SetLink(gameObject);
    }

    void FlashWhite()
    {
        SetAllImages(Color.white);
        AnimateAllImages(bgColor, 0.2f);
    }
    #endregion

    #region Helpers
    void KillTweens()
    {
        rect?.DOKill();
        transform?.DOKill();
        leftTriangle?.DOKill();
        rightTriangle?.DOKill();
        mainBg?.DOKill();
    }

    void BringToFront()
    {
        Vector3 pos = rect.position;
        pos.z = 10;
        rect.position = pos;
    }

    void ResetScale()
    {
        if (changeScale)
            transform.localScale = defaultScale;
    }

    void SetAllImages(Color color)
    {
        leftTriangle.color = color;
        rightTriangle.color = color;
        mainBg.color = color;
    }

    void AnimateAllImages(Color target, float duration)
    {
        leftTriangle.DOColor(target, duration).SetLink(gameObject);
        rightTriangle.DOColor(target, duration).SetLink(gameObject);
        mainBg.DOColor(target, duration).SetLink(gameObject);
    }

    IEnumerator ClickDelay()
    {
        if (haveDelay)
            yield return new WaitForSeconds(0.5f);

        onClick?.Invoke();
    }

    void OnDestroy()
    {
        KillTweens();
    }
    #endregion
}
