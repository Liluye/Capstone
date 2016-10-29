using UnityEngine;
using System.Collections;

public class bombAction : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float framesPerSecond;
    public float speed;
    private float initializedAt;


    // Use this for initialization
    void Start() {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        GetComponent<BoxCollider2D>().enabled = false;
        initializedAt = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        int nextSprite = (int)(timeSinceInitialized * framesPerSecond);
        if (nextSprite == sprites.Length)
        {
            Explode();
        }          
        else
        {
            nextSprite %= sprites.Length;
            spriteRenderer.sprite = sprites[nextSprite];
        }
    }

    private void Explode()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(DestroyDelay());       
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("collided");
    }

    IEnumerator DestroyDelay()
    {
        //A slight delay is required to allow colliders to take affect
        yield return new WaitForSeconds(0.04f);
        Destroy(gameObject);
    }

}
