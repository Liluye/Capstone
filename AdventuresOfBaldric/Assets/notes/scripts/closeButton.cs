/*****************************************************************
Script to determine what the close button does on the note.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class closeButton : MonoBehaviour {

    /** close button */
    public Button button;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start () {

        // add click listener to button
        Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}

    /*******************************************************************
	 * Method to determine what to do on click
	 ******************************************************************/
    void TaskOnClick(){

        // when button is clicked close panel prefab
        if (GameObject.Find ("writeNotePanel(Clone)") != null) {
			Destroy (GameObject.Find ("writeNotePanel(Clone)"));
		} else {
			Destroy (GameObject.Find ("readNotePanel(Clone)"));
		}

		// unpause game action
		Time.timeScale = 1.0f;
	}
}
