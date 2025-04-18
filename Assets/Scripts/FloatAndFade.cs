using UnityEngine;
using UnityEngine.UI;

public class FloatAndFade : MonoBehaviour
{
    public float floatDistance = 50f;
    public float duration = 0.5f;

    private Vector2 startPos;
    private Vector2 targetPos;
    private CanvasGroup canvasGroup;
    private float timer;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + new Vector2(0, floatDistance);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // Move upward
        transform.localPosition = Vector2.Lerp(startPos, targetPos, t);

        // Fade out
        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);

        // Destroy after animation
        if (t >= 1f)
            Destroy(gameObject);
    }
}
