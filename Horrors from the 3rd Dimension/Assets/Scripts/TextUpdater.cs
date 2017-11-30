using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
        public Text dialog;
	}
	
	// Update is called once per frame
	void Update () {
        dialog.text = "Hey?";
	}
}
