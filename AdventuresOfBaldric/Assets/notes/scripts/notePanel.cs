/*****************************************************************
Script for creating note panels.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class notePanel : MonoBehaviour {

    /** game objects associated with the note panels */
	public GameObject writepanel;
	public GameObject readpanel;

    /*******************************************************************
	 * Instantiates a new write note panel
	 ******************************************************************/
    public void activateWritePanel () {

		GameObject wpanel = Instantiate(writepanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		wpanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
		// set the input field as focus
		EventSystem.current.SetSelectedGameObject (wpanel.GetComponentInChildren<InputField> ().gameObject, null);

    }

    /*******************************************************************
	 * Instantiates a new read note panel
	 ******************************************************************/
    public void activateReadPanel (string note) {

		GameObject rpanel = Instantiate(readpanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		rpanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
		Transform child = rpanel.transform.Find ("Message");
		child.GetComponent<Text> ().text = note;

	}

}
