using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour {

    private bool clearingText = false;
    private float textAppliedTime = 0.0f;
    public Text dialog;

    // Use this for initialization
    void Start () {
        dialog = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!clearingText && dialog.text != "")
        {
            clearingText = true;
            textAppliedTime = Time.time;
        }
        if (Time.time - 5 > textAppliedTime)
        {
            clearText();
            clearingText = false;
        }
	}

    public void clearText()
    {
        dialog.text = "";
    }
}
