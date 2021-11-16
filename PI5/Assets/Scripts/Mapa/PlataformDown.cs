using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformDown : MonoBehaviour
{
    Rigidbody2D rg;
    public float duration = 1.0f;
    public float fadeDuration;
    public float spawnDuration;
    Vector3 posOb;
    public SpriteRenderer targetRenderer;
    public GameObject spawnPlataform;
    BoxCollider2D game;
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
        rg.gravityScale = 0f;
        game = gameObject.GetComponent<BoxCollider2D>();

        //posOb = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CheckPlataform"))
        {
            Debug.Log("indent");
            StartCoroutine(DownCoroutine());
           
        }
        

    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 15 || col.gameObject.layer == 8)
        {
            SpawnPosition();
            StartCoroutine(SpawnTime());
        }
    }

    private IEnumerator DownCoroutine()
    {
        yield return new WaitForSecondsRealtime(duration);
        rg.gravityScale = 1f;

        

    }

    void SpawnPosition()
    {
        
        Color currentColor = targetRenderer.color;
        currentColor.a = 0f;
        targetRenderer.color = currentColor;
        rg.gravityScale = 0f;
        game.enabled = false;
        gameObject.transform.position = new Vector3(spawnPlataform.transform.position.x, spawnPlataform.transform.position.y, spawnPlataform.transform.position.z);
    }
    IEnumerator SpawnTime()
    {
        yield return new WaitForSecondsRealtime(spawnDuration);
        game.enabled = true;
        StartCoroutine(FadeCoroutine(1));
    }

    private IEnumerator FadeCoroutine(float fadeTo)
    {
        float timer = 0;
        Color currentColor = targetRenderer.color;
        float startAlpha = targetRenderer.color.a;

        while (timer < 1)
        {
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime / fadeDuration;

            currentColor.a = Mathf.Lerp(startAlpha, fadeTo, timer);
            targetRenderer.color = currentColor;

        }

    }

}
