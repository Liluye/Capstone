/*****************************************************************
Script to control the behavior of the bomb.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class bombAction : MonoBehaviour {

    /** the renderer connected to the item sprite */
    private SpriteRenderer spriteRenderer;

    /** array for bomb sprites */
    public Sprite[] sprites;

    /** number of fps for the bomb to animate */
    public float framesPerSecond;

    /** speed at which the bomb moves */
    public float speed;

    /** time the item was initialized */
    private float initializedAt;


    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start() {
        
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        initializedAt = Time.timeSinceLevelLoad;

    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void Update()
    {
        float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        int nextSprite = (int)(timeSinceInitialized * framesPerSecond);
		if (nextSprite == 8) {
			AudioSource audio = GetComponent<AudioSource> ();
			audio.Play ();
		}
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

    /*******************************************************************
	 * Method to cause the explosion
	 ******************************************************************/
    private void Explode()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(DestroyDelay());       
    }

    /*******************************************************************
	 * Method for a slight delay that is required to allow colliders 
     * to take affect
	 ******************************************************************/
    IEnumerator DestroyDelay()
    {

        yield return new WaitForSeconds(0.04f);
        Destroy(gameObject);
    }
}
