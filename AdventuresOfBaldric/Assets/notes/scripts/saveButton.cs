/*****************************************************************
Script to determine what the save button does on the note.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class saveButton : MonoBehaviour {

    /** text box for input */
	public Text field;

    /** save button */
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
    void TaskOnClick() {

        // when button is clicked save text, decrement count, and close panel
        string note = field.text.ToString ();
		Vector2 loc = GameObject.Find ("character").transform.position;
		GameObject.Find ("Main Camera").GetComponent<NetworkManager> ().addNote (note, loc);
		GameObject.Find ("openNote").GetComponent<noteButton> ().decrementCount ();
		Destroy (GameObject.Find("writeNotePanel(Clone)"));
		// unpause game action
		Time.timeScale = 1.0f;

	}
}
