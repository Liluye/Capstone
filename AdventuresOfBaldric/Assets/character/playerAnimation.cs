/*****************************************************************
Script to control the player animation.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    /** the animator connected to the player sprite */
    private Animator animator;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
	{
		animator = this.GetComponent<Animator>();
	}

    /*******************************************************************
	 * Method called once per frame to update movement and direction
	 ******************************************************************/
    void Update()
	{

		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		if (vertical > 0)
		{
			animator.SetInteger("Direction", 2);
		}
		else if (vertical < 0)
		{
			animator.SetInteger("Direction", 0);
		}
		else if (horizontal > 0)
		{
			animator.SetInteger("Direction", 3);
		}
		else if (horizontal < 0)
		{
			animator.SetInteger("Direction", 1);
		}
	}
}