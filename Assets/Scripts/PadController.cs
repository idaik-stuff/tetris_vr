using UnityEngine;
using System.Collections;
using VRTK;

public class PadController : MonoBehaviour {
	
    
    private static Vector2 StartPadTouch;
    private static Vector2 EndPadTouch;


 
    private float movementDeltaX = 0.2f;
    private float movementDeltaY = 0.15f;

    private Transform cam;
    private Transform vrCameraRig;



    // Use this for initialization
    void Start()
    {


        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);

        GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);


        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject += new ObjectInteractEventHandler(DoUpdateGrid);

        
    }



    private void  DoUpdateGrid(object sender, ObjectInteractEventArgs e)
    {
        GameObject CurrGroup = e.target;
        Group CurrGroupScript;


        if (CurrGroup != null)
        {
            CurrGroupScript = CurrGroup.GetComponent<Group>();

            if (CurrGroupScript != null)
            {
                CurrGroupScript.GoUpdateGrid();
            }
        }
    }


    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
       /* if (e.touchpadAxis.y < 0)
        {
            GameObject CurrGroup = GetComponent<VRTK_InteractGrab>().GetGrabbedObject();

            //To Rotate must be Touched
            //GameObject  CurrGroup= GetComponent<VRTK_InteractTouch>().GetTouchedObject();

            Group CurrGroupScript;

            if (CurrGroup != null)
            {
                CurrGroupScript = CurrGroup.GetComponent<Group>();

                if (CurrGroupScript != null)
                {
                    GetComponent<VRTK_InteractGrab>().ForceRelease();
                    CurrGroupScript.CompleteDown();
                }
            }

        }*/
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
       
       
        // Get reference to current camera (VR or Normal)
        cam = Camera.main.gameObject.transform;
        vrCameraRig = cam.parent.Find("Camera (eye)").transform;

       

        //To Rotate must be grabbed
        GameObject CurrGroup = GetComponent<VRTK_InteractGrab>().GetGrabbedObject();

        //To Rotate must be Touched
        //GameObject  CurrGroup= GetComponent<VRTK_InteractTouch>().GetTouchedObject();

       

        Group CurrGroupScript;

        if (CurrGroup!=null)
        {
            CurrGroupScript = CurrGroup.GetComponent<Group>();

            if (CurrGroupScript != null)
            {
                               
              EndPadTouch = e.touchpadAxis;

              float diffX = Mathf.Abs(StartPadTouch.x - EndPadTouch.x);
              float diffY = Mathf.Abs(StartPadTouch.y - EndPadTouch.y);

              if (diffY > diffX && diffY > movementDeltaY)
                {
                    if (StartPadTouch.y < EndPadTouch.y)
                    {
                        CurrGroupScript.RotateTop();
                    }
                    else
                    {
                        CurrGroupScript.RotateDown();
                    }
                    StartPadTouch = EndPadTouch;
                }
                else if (diffX > diffY && diffX > movementDeltaX)
                {
                    if (StartPadTouch.x > EndPadTouch.x)
                    {
                        CurrGroupScript.RotateLeft();
                    }
                    else
                    {
                        CurrGroupScript.RotateRight();
                    }
                    StartPadTouch = EndPadTouch;
                }



            }
        }
    }

    

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {

        Debug.Log(" END: " + StartPadTouch + "-" + EndPadTouch);
        StartPadTouch = Vector2.zero;
        EndPadTouch = Vector2.zero;
    }


    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        StartPadTouch = e.touchpadAxis;
        Debug.Log(" START: " + StartPadTouch);
    }


    private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        GameObject CurrGroup = GetComponent<VRTK_InteractGrab>().GetGrabbedObject();

        //To Rotate must be Touched
        //GameObject  CurrGroup= GetComponent<VRTK_InteractTouch>().GetTouchedObject();

        Group CurrGroupScript;

        if (CurrGroup != null)
        {
            CurrGroupScript = CurrGroup.GetComponent<Group>();

            if (CurrGroupScript != null)
            {
                GetComponent<VRTK_InteractGrab>().ForceRelease();
                CurrGroupScript.CompleteDown();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
      
    }



}
