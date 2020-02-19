using UnityEngine;
using System.Collections;
using VRTK;

public class HeadsetCubeCollision : MonoBehaviour {


    private VRTK_HeadsetCollision headsetCollision;

    // Use this for initialization
    void Start () {
	
	}


    private void OnEnable()
    {
        headsetCollision = gameObject.AddComponent<VRTK_HeadsetCollision>();
        //headsetCollision.GetComponent<Collider>().size = new Vector3(0.1f, 0.1f, 0.1f);


        headsetCollision.HeadsetCollisionDetect += new HeadsetCollisionEventHandler(OnHeadsetCollisionDetect);
        headsetCollision.HeadsetCollisionEnded += new HeadsetCollisionEventHandler(OnHeadsetCollisionEnded);
    }



    private void OnHeadsetCollisionDetect(object sender, HeadsetCollisionEventArgs e)
    {


        Debug.Log("si");
        if ((e.collider.gameObject != null) && (e.collider.gameObject.name.StartsWith("Cube"))) GoToCubeTop(e.collider.gameObject);
    }

    private void OnHeadsetCollisionEnded(object sender, HeadsetCollisionEventArgs e)
    {
        
    }

    private void OnDisable()
    {
        headsetCollision.HeadsetCollisionDetect -= new HeadsetCollisionEventHandler(OnHeadsetCollisionDetect);
        headsetCollision.HeadsetCollisionEnded -= new HeadsetCollisionEventHandler(OnHeadsetCollisionEnded);

        Destroy(headsetCollision);
       
    }

   
   

    private void GoToCubeTop(GameObject obj)
    {
        Debug.Log("si");
        Vector3 basePosition;

        if (obj.transform.parent!= null)
        if (obj.transform.parent.tag == "Fixed")
        {
            GameObject ScriptObj = GameObject.Find("/Scripts").gameObject;

            if (ScriptObj != null)
            {
                basePosition = ScriptObj.GetComponent<Grid>().GetMaxUsedY(obj.transform.position);
                transform.position = basePosition;
            }

            // basePosition.y = GetTopY(tipPosition, target);
            
        }
        

    }

}
