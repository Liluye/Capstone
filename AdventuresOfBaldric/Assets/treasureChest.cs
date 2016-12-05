using UnityEngine;
using System.Collections;

public class treasureChest : MonoBehaviour {
    string note;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hello");
        GameObject impactor = other.gameObject;


        if (impactor.CompareTag("Player") && impactor.name.Equals("character"))
        {
            Time.timeScale = 0.0f;
            GameObject.Find("Canvas").GetComponent<notePanel>().activateReadPanel("Congratulations, you have completed the Adventures of Baldric!");
            Destroy(gameObject);
        }
    }
}
