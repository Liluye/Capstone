using UnityEngine;
using System.Collections;

public class SwordAction : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    float initializedAt;
    //private Vector3 position = new Vector3(0,0,0);
    private int count = 0;
    public int speed;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        initializedAt = Time.timeSinceLevelLoad;
        //position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        if (count < 90)
        {
            transform.Rotate(Vector3.forward * speed);
            count += speed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitialDirection(int dir)
    {
        //0 down, 1 left, 2 up, 3 right
        switch (dir)
        {
            case 0:
                transform.Translate(0, -0.6f, 0);
                transform.Rotate(Vector3.forward * 180);
                break;
            case 1:
                transform.Translate(-0.2f, 0, 0);
                transform.Rotate(Vector3.forward * 90);
                break;
            case 2:
                transform.Translate(0, 0.3f, 0);
                break;
            case 3:
                transform.Translate(0.2f, 0,  0);
                transform.Rotate(Vector3.forward * 270);
                break;
            default:
                //something went wrong, do nothing!
                break;
        }
        //transform.position = position;
    }
}
