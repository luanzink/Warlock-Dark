using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoTransparency : MonoBehaviour
{
    [Range(0, 1)]
    public float transparency;
    public Tilemap targetRenderer;
    public LayerMask layerMask;
    public float fadeduration;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (layerMask == (layerMask | (1 << col.gameObject.layer)))
        {
            //Debug.Log("colodiu");
            SetTransparency(transparency);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (layerMask == (layerMask | (1 << col.gameObject.layer)))
        {
            //Debug.Log("saiu");
            SetTransparency(1);
        }
    }

    void SetTransparency(float alpha)
    {
        //Debug.Log("chamou");
        StopCoroutine("FadeCoroutine");
        StartCoroutine("FadeCoroutine", alpha);
    }

    private IEnumerator FadeCoroutine(float fadeTo)
    {
        float timer = 0;
        Color currentColor = targetRenderer.color;
        float startAlpha = targetRenderer.color.a;

        while (timer < 1)
        {
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime / fadeduration;

            currentColor.a = Mathf.Lerp(startAlpha, fadeTo, timer);
            targetRenderer.color = currentColor;

        }

    }
}
