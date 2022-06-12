using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimateBody : MonoBehaviour
{
    [SerializeField] float defaultAnimTime = 1f;
    [SerializeField] Vector2 xSqueeze = new Vector2(1.2f, 0.8f);
    [SerializeField] Vector2 ySqueeze = new Vector2(0.8f, 1.2f);
    Coroutine coroutine;
    [SerializeField] Color airColor = Color.black;
    Color originColor = Color.black;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        originColor = myRenderer.color;
    }


    public void SqueezeHorizontal()
    {
        StopAllCoroutines();
        coroutine = StartCoroutine(LerpTransform(transform, transform.localScale, xSqueeze, defaultAnimTime));
    }

    public void SqueezeVertical(float duration)
    {
        StopAllCoroutines();
        coroutine = StartCoroutine(LerpTransform(transform, transform.localScale, ySqueeze, duration));
    }

    public void ResetBody(float extend = 0f)
    {
        StopAllCoroutines();
        coroutine = StartCoroutine(LerpTransform(transform, transform.localScale, Vector2.one, defaultAnimTime - 0.2f + extend));
    }


    public static IEnumerator LerpTransform(Transform transform,Vector2 startValue, Vector2 endValue, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            transform.localScale = Vector2.Lerp(startValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endValue;
    }

    public void SetAirColor(bool isAir)
    {
        if (isAir && myRenderer.color != airColor)
            myRenderer.color = airColor;
        else if(!isAir && myRenderer.color != originColor)
            myRenderer.color = originColor;
    }
}
