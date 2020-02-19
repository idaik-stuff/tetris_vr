using UnityEngine;
using System.Collections;

public class ScoreBorad : MonoBehaviour {
    public TextMesh text;

    private int points=0;

    // Use this for initialization
    void Start () {
        text.text = "Go go go!";
    }
	
	// Update is called once per frame
	void Update () {
        text.text = " " + points + " Ptos";

    }

    public void reset()
    {
        points = 0;
    }

    public void AddPoints()
    {
        points = points + 100;
    }
}
