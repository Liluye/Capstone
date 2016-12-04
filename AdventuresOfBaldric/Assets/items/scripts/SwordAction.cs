/*****************************************************************
Script to control the behavior of the sword.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class SwordAction : MonoBehaviour {

    /** the renderer connected to the item sprite */
    private SpriteRenderer spriteRenderer;

    /** time the item was initialized */
    float initializedAt;
    
    /** count to determine sword usage time */
    private int count = 0;

    /** speed at which the sword moves */
    public int speed;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start () {

        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        initializedAt = Time.timeSinceLevelLoad;

    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void Update () {
        
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

    /*******************************************************************
	 * Move the direction of the sword
     * @param dir Integer direction to move the sword
	 ******************************************************************/
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

    }
}
