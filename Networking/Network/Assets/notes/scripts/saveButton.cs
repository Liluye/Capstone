using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class saveButton : MonoBehaviour {

	public Text field;
	public Button button;

	//add click listener to button
	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	//when button is clicked save text, decrement count, and close panel
	void TaskOnClick(){
		string note = field.text.ToString ();
		Vector2 loc = GameObject.Find ("character").transform.position;
		GameObject.Find ("Main Camera").GetComponent<NetworkManager> ().addNote (note, loc);
		GameObject.Find ("openNote").GetComponent<noteButton> ().decrementCount ();
		Destroy (GameObject.Find("writeNotePanel(Clone)"));
		//unpause game action
		Time.timeScale = 1.0f;
	}
}
