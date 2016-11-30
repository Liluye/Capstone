using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class closeButton : MonoBehaviour {

	public Button button;

	//add click listener to button
	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	//when button is clicked close panel prefab
	void TaskOnClick(){
		if (GameObject.Find ("writeNotePanel(Clone)") != null) {
			Destroy (GameObject.Find ("writeNotePanel(Clone)"));
		} else {
			Destroy (GameObject.Find ("readNotePanel(Clone)"));
		}
		//unpause game action
		Time.timeScale = 1.0f;
	}
}
