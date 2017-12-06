using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	TextMesh text;

	// Use this for initialization
	void Start () 
	{
		text = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver()
	{
		text.text = anagramatic (text.text);
		print ("tstytst");
	}

	void OnMoustUp()
	{

	}

	string anagramatic(string original)
	{
		ArrayList ls = new ArrayList();
		string ret = "";
		System.Random rand = new System.Random ();

		foreach (char c in original) 
		{
			ls.Add (c);
		}

		while ((ls.Count == 0) == false) 
		{
			int i = rand.Next(0, ls.Count);
			ret = ret + ls [i];
			ls.RemoveAt (i);
		}
		return ret;
	}
}
