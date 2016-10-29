using UnityEngine;
using System.Collections;

public class boomerangAction : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 startLoc = new Vector3(0, 0, 0);
    private Vector3 destinationLoc;
    public Sprite[] sprites;
    public float framesPerSecond;
    public float speed;
    private bool returning = false;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        this.startLoc = transform.position;
        setInitialDestination(2);
    }

    void Start(Vector3 startLoc, int direction)
    {
        this.Start();
        this.startLoc = startLoc;
        setInitialDestination(direction);
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;

        if (Vector3.SqrMagnitude(currentPosition - destinationLoc) < 0.2)
        {
            if (returning)
                Destroy(gameObject);
            else
                boomerangReturn();
        }

        int nextSprite = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        nextSprite %= sprites.Length;
        spriteRenderer.sprite = sprites[nextSprite];

        Vector3 target = destinationLoc;

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        //Debug.Log(Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        boomerangReturn();
    }

    private void setInitialDestination(int direction)
    {
        destinationLoc.x = startLoc.x;
        destinationLoc.y = startLoc.y;
        //0 down, 1 left, 2 up, 3 right
        switch (direction)
        {
            case 0:
                destinationLoc.y = startLoc.y - 4;
                break;
            case 1:
                destinationLoc.x = startLoc.x - 4;
                break;
            case 2:
                destinationLoc.y = startLoc.y + 4;
                break;
            case 3:
                destinationLoc.x = startLoc.x + 4;
                break;
            default:
                //something went wrong, do nothing!
                break;
        }
    }

    private void boomerangReturn()
    {
        destinationLoc = startLoc;
        returning = true;
    }
}
