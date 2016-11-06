using UnityEngine;
using System.Collections;

public class grapplingHookAction : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float count;
    private float distance;
    private bool grapple;
    private BoxCollider2D boxCollider;
    public float lineDrawSpeed = 6f;
    private bool returning = false;
    private Vector3 pointAlongLine;
    private Vector3 startLoc;
    private Vector3 destinationLoc;

    //public Transform origin;
    //public Transform destination;

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {
        
        lineRenderer.SetPosition(1, destinationLoc);
        if (count < distance)
        {
            count += .02f * lineDrawSpeed;
            float x = Mathf.Lerp(0, distance, count);

            if (returning)
            {
                if (Vector3.Distance(pointAlongLine, startLoc) < 0.2)
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
                Debug.Log(Vector3.Distance(destinationLoc, startLoc));
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

    //Function that allows the player object to set an initial direction
    public void InitialDirection(int dir)
    {
        destinationLoc.x = transform.position.x;
        destinationLoc.y = transform.position.y;
        //0 down, 1 left, 2 up, 3 right
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
                //something went wrong, do nothing!
                break;
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        //Grappling Hook has attached to a point
        if (col.gameObject.tag == "box")
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
            distance = Vector3.Distance(startLoc, transform.position);
            returning = true;
        }
    }

    public bool getGrapple()
    {
        return grapple;
    }

    public Vector3 getGrappleLocation()
    {
        return transform.position;
    }
    
    public void playerLocation(Vector3 playerLoc)
    {
        Debug.Log("bad");
        startLoc = playerLoc;
        lineRenderer.SetPosition(0, playerLoc);
        distance = Vector3.Distance(playerLoc, destinationLoc);
    }
    
    public void BreakGrapple()
    {
        Destroy(gameObject);
    }
}
