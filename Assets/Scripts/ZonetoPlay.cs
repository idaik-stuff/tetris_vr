using UnityEngine;
using System.Collections;

public class ZonetoPlay : MonoBehaviour {

    public int maxi_x;
    public int maxi_y;
    public int maxi_z;

    public AudioClip FullRowBomb;


    // Use this for initialization
    void Start () {

        transform.position = new Vector3(0, -0.49f, 0);
        transform.localScale = new Vector3(maxi_x*2-1, 0.01f, maxi_z*2-1);


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
