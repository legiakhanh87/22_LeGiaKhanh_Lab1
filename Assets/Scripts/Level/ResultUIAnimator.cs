using UnityEngine;
using TMPro;
using System.Collections;

public class ResultUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public CanvasGroup canvasGroup;

    public float fadeSpeed = 4f;
    public float pulseSpeed = 4f;

    private Vector3 baseScale;
    private bool pulsing;

    void Awake()
    {
        baseScale = transform.localScale;
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void Show(bool isWin, int finalScore)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;              
        transform.localScale = baseScale;     
        resultText.text = isWin ? "YOU WIN" : "YOU LOSE";
        resultText.color = isWin ? Color.green : Color.red;
        scoreText.text = "SCORE: " + finalScore;
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        float t = 0f;
        pulsing = false;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = t;
            transform.localScale = Vector3.Lerp(
                baseScale * 0.7f,
                baseScale,
                t
            );
            yield return null;
        }

        pulsing = true;
    }

    void Update()
    {
        if (!pulsing) return;

        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.05f;
        transform.localScale = baseScale * (1f + pulse);
    }
}
