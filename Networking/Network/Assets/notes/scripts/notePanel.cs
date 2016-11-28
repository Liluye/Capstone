using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class notePanel : MonoBehaviour {

	public GameObject writepanel;
	public GameObject readpanel;


	public void activateWritePanel () {
		GameObject wpanel = Instantiate(writepanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		wpanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
		//set the input field as focus
		EventSystem.current.SetSelectedGameObject (wpanel.GetComponentInChildren<InputField> ().gameObject, null);
	}

	public void activateReadPanel () {
		GameObject rpanel = Instantiate(readpanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		rpanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
	}

}
