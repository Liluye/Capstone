using UnityEngine;
using System.Collections;

public class SwordAction : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    float initializedAt;
    private int count = 0;
    public int speed = 20;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        initializedAt = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject);
        //float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        /*while (count < 50)
        {
            transform.Rotate(Vector3.forward * speed);
            count += speed;
        }
            */
    }
}
