/*****************************************************************
Script to determine what the note button does.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class noteButton : MonoBehaviour {

    /** note button */
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

        // when button is click spawn panel prefab
        // if there isn't already a writeNotePanel open, create panel
        if (GameObject.Find ("writeNotePanel(Clone)") == null) {

			// pause game action
			Time.timeScale = 0.0f;
			GameObject.Find ("Canvas").GetComponent<notePanel> ().activateWritePanel ();

		}

	}

    /*******************************************************************
	 * Method to change the note count when a note is used.
	 ******************************************************************/
    public void decrementCount () {

		string text = button.GetComponentInChildren<Text> ().text;
		int count = int.Parse (text);
		if (count > 0) {
			count--;
			button.GetComponentInChildren<Text> ().text = count.ToString ();
		}
		if (count <= 0) {
			button.interactable = false;
		}

	}

}
