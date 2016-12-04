/*****************************************************************
Script to control the behavior of the boomerang.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class boomerangAction : MonoBehaviour {

    /** the renderer connected to the item sprite */
    private SpriteRenderer spriteRenderer;

    /** initial location of the boomerang */
    private Vector3 startLoc = new Vector3(0, 0, 0);

    /** location of where the boomerang is being thrown */
    private Vector3 destinationLoc;

    /** array for boomerang sprites */
    public Sprite[] sprites;

    /** number of fps for the boomerang to animate */
    public float framesPerSecond;

    /** speed at which the boomerang moves */
    public float speed;

    /** state of the boomerang being sent or returning */
    private bool returning = false;

    /** time the item was initialized */
    private float initializedAt;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        startLoc = transform.position;
        initializedAt = Time.timeSinceLevelLoad;
    }

    /*******************************************************************
	 * Move the direction of the boomerang
     * @param dir Integer direction to move the boomerang
	 ******************************************************************/
    public void InitialDirection(int dir)
    {
        setInitialDestination(dir);
    }

    /*******************************************************************
	 * Method that allows the player object to set a return location
     * @param loc Vector3 location from the player object
	 ******************************************************************/
    public void UpdateLocation(Vector3 loc)
    {
        if (returning)
        {
            destinationLoc = loc;
        }
    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void Update()
    {
        float timeSinceInitialized = Time.timeSinceLevelLoad - initializedAt;
        Vector3 currentPosition = transform.position;

        // when boomerang reaches its destination
        if (Vector3.SqrMagnitude(currentPosition - destinationLoc) < 0.2)
        {
            // if object hits its return location destroy it
            if (returning)
                Destroy(gameObject);
            else
                // have it to return to player
                boomerangReturn();
        }

        // update sprite image
        int nextSprite = (int)(timeSinceInitialized * framesPerSecond);
        nextSprite %= sprites.Length;
        spriteRenderer.sprite = sprites[nextSprite];

        // linear interpolate between current position and destination
        transform.position = Vector3.Lerp(transform.position, destinationLoc, Time.deltaTime * speed);
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D col)
    {
        // boomerang has returned to player, destroy it
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        // boomerang has collided with something else, have it return to player
        boomerangReturn();
    }

    /*******************************************************************
	 * Sets the initial direction of the boomerang
     * @param direction Integer direction to move the boomerang
	 ******************************************************************/
    private void setInitialDestination(int direction)
    {
        destinationLoc.x = transform.position.x;
        destinationLoc.y = transform.position.y;

        //0 down, 1 left, 2 up, 3 right
        switch (direction)
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
                // something went wrong, do nothing!
                break;
        }
    }

    /*******************************************************************
	 * Sends the boomerang back to the player
	 ******************************************************************/
    private void boomerangReturn()
    {
        destinationLoc = startLoc;
        returning = true;
    }
}
