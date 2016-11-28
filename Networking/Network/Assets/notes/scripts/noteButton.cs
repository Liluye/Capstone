using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class noteButton : MonoBehaviour {

	public Button button;

	//add click listener to button
	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	//when button is click spawn panel prefab
	void TaskOnClick(){
		//If there isn't already a writeNotePanel open, create panel
		if (GameObject.Find ("writeNotePanel(Clone)") == null) {
			//pause game action
			Time.timeScale = 0.0f;
			GameObject.Find ("Canvas").GetComponent<notePanel> ().activateWritePanel ();
		}
	}

	//Change the note count
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
