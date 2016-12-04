/*****************************************************************
Script to control the behavior of the grappling hook.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class grapplingHookAction : MonoBehaviour
{
    /** the renderer connected to the grappling hook chain */
    private LineRenderer lineRenderer;

    /** count to determine grappling hook usage time */
    private float count;

    /** distance the hook is stretched to */
    private float distance;

    /** state of the grappling hook being used */
    private bool grapple;

    /** collider for the boxes to hook to */
    private BoxCollider2D boxCollider;

    /** speed at which the cahin is drawn */
    public float lineDrawSpeed = 6f;

    /** state of the grappling hook being sent or returning */
    private bool returning = false;

    /** location along the hook chain */
    private Vector3 pointAlongLine;

    /** initial location of the grappling hook */
    private Vector3 startLoc;

    /** location of where the grappling hook is being shot */
    private Vector3 destinationLoc;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.sortingOrder = 10;
        lineRenderer.SetWidth(.25f, .25f);
        startLoc = transform.position;
        distance = 3;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.transform.position = transform.position;

    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void Update()
    {
        
        lineRenderer.SetPosition(1, destinationLoc);
        if (count < distance)
        {
            count += .02f * lineDrawSpeed;
            float x = Mathf.Lerp(0, distance, count);

            if (returning)
            {
                if (Vector3.Distance(pointAlongLine, startLoc) < 0.3)
                    Destroy(gameObject);
                pointAlongLine = x * Vector3.Normalize(startLoc - destinationLoc) + destinationLoc;

            }
            else
            {
                pointAlongLine = x * Vector3.Normalize(destinationLoc - startLoc) + startLoc;
            }

            boxCollider.transform.position = pointAlongLine;

            lineRenderer.SetPosition(1, pointAlongLine);
        }
        else
        {
            if (grapple)
            {
                if (Vector3.Distance(destinationLoc, startLoc) < 0.4)
                    Destroy(gameObject);
                
            }
            else
            {
                returning = true;
                count = 0;
            }

        }
    }

    /*******************************************************************
	 * Move the direction of the grappling hook
     * @param dir Integer direction to move the grappling hook
	 ******************************************************************/
    public void InitialDirection(int dir)
    {
        destinationLoc.x = transform.position.x;
        destinationLoc.y = transform.position.y;

        // 0 down, 1 left, 2 up, 3 right
        switch (dir)
        {
            case 0:
                destinationLoc.y = transform.position.y - 3;
                break;
            case 1:
                destinationLoc.x = transform.position.x - 3;
                break;
            case 2:
                destinationLoc.y = transform.position.y + 3;
                break;
            case 3:
                destinationLoc.x = transform.position.x + 3;
                break;
            default:
                // something went wrong, do nothing!
                break;
        }
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D col)
    {
        // grappling hook has attached to a point
        if (col.gameObject.tag == "post")
        {
            grapple = true;
            destinationLoc = transform.position;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
        else if (col.gameObject.tag == "character")
        {
                Destroy(gameObject);
		}
            else
        {
            destinationLoc = transform.position;
            distance = Vector3.Distance(startLoc, destinationLoc);
            returning = true;
            count = 0;
        }
    }

    /*******************************************************************
	 * Method that gets the current grappling state
	 ******************************************************************/
    public bool getGrapple()
    {
        return grapple;
    }

    /*******************************************************************
	 * Method that gets the current grappling location
	 ******************************************************************/
    public Vector3 getGrappleLocation()
    {
        return transform.position;
    }

    /*******************************************************************
	 * Method that gets the current player location
	 ******************************************************************/
    public void playerLocation(Vector3 playerLoc)
    {
        startLoc = playerLoc;
        lineRenderer.SetPosition(0, playerLoc);
        distance = Vector3.Distance(playerLoc, destinationLoc);
    }

    /*******************************************************************
	 * Method that ends the current grapple
	 ******************************************************************/
    public void BreakGrapple()
    {
        Destroy(gameObject);
    }
}
