using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hpScript : MonoBehaviour {

    public Text hp;
	// Use this for initialization
	void Start () {
	    hp = GetComponent<Text>();
        //hp = 10;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
