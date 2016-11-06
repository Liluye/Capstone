using UnityEngine;
using System.Collections;

public class boomerangAction : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Vector3 startLoc = new Vector3(0, 0, 0);
    private Vector3 destinationLoc;
    public Sprite[] sprites;
    public float framesPerSecond;
    public float speed;
    private bool returning = false;
    private float initializedAt;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        startLoc = transform.position;
        initializedAt = Time.timeSinceLevelLoad;
    }

    //Function that allows the player object to set an initial direction
    public void InitialDirection(int dir)
    {
        destinationLoc.x = transform.position.x;
        destinationLoc.y = transform.position.y;
        //0 down, 1 left, 2 up, 3 right
        switch (dir)
        {
            case 0:
                destinationLoc.y = transform.position.y - 4;
                break;
            case 1:
                destinationLoc.x = transform.position.x - 4;
                break;
            case 2:
                destinationLoc.y = transform.position.y + 4;
                break;
            case 3:
                destinationLoc.x = transform.position.x + 4;
                break;
            default:
                //something went wrong, do nothing!
                break;
        }
    
    }

    //unction that allows the player object to set a return location
    public void UpdateLocation(Vector3 loc)
    {
        if (returning)
        {
            destinationLoc = loc;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        Vector3 currentPosition = transform.position;

        //When boomerang reaches its destination
        if (Vector3.SqrMagnitude(currentPosition - destinationLoc) < 0.2)
        {
            //If object hits its return location destroy it
            if (returning)
                Destroy(gameObject);
            else
                //Have it to return to player
                boomerangReturn();
        }

        //Update sprite image
        int nextSprite = (int)(timeSinceInitialized * framesPerSecond);
        nextSprite %= sprites.Length;
        spriteRenderer.sprite = sprites[nextSprite];
        //Linear interpolate between current position and destination
        transform.position = Vector3.Lerp(transform.position, destinationLoc, Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //Boomerang has returned to player, destroy it
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        //Booomerang has collided with something else, have it return to player
        boomerangReturn();
    }
        

    private void boomerangReturn()
    {
        destinationLoc = startLoc;
        returning = true;
    }
}
