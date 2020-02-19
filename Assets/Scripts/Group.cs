using UnityEngine;
using System.Collections;
using VRTK;


public class Group : MonoBehaviour {

    static float lastFall = 0;
    static float FallVelocity = 0.5f;


    //For Rotation
    private Quaternion target;
    private float start;
    private float speed = 0.2f; // Time it takes to rotate 90 degrees

    
    public AudioClip hitAction;
    public AudioClip hitGround;
 
    private float currY;

    private bool forceDown = false;

    void Start()
    {
        target = transform.rotation;
        start = 0.0f;
        forceDown = false;

        currY = transform.position.y;

        transform.position += new Vector3(0, -1, 0);
        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            //Debug.Log("GAME OVER");
            Destroy(gameObject);

            GameObject.Find("/Spawner").GetComponent<Spawner>().GameOver();

            // Disable script
            enabled = false;


        }
        else
        {
            transform.position += new Vector3(0, +1, 0);
        
        }

    }


   public  void GoUpdateGrid()
    {       
       
        ForceGoodPosition();       

        updateGrid();
    }



    public void CompleteDown()
    {
        forceDown = true;
       // GetComponent<VRTK_InteractableObject>().isGrabbable = false;
    }


    public void Rotate_Up()
    {
        if (start == 0.0f)
        {
            target = worldAxisRotate(upDownRotationAxis() * 90);
            start = Time.time;
            Debug.Log("UP " + start);
        }
    }

    public void Rotate_Down()
    {
        if (start == 0.0f)
        {
            target = worldAxisRotate(upDownRotationAxis() * -90);
            start = Time.time;
            Debug.Log("DOWN " + start);
        }
    }

    public void Rotate_Left()
    {
        if (start == 0.0f)
        {
            target = worldAxisRotate(Vector3.up * 90);
            start = Time.time;
            Debug.Log("LEFT " + start);
        }
    }

    public void Rotate_Right()
    {
        if (start == 0.0f)
        {
            target = worldAxisRotate(Vector3.up * -90);
            start = Time.time;
            Debug.Log("RIGHT " + start);
        }
    }

    private Quaternion worldAxisRotate(Vector3 rot)
    {
        Quaternion current = transform.rotation;
        Quaternion target = transform.rotation * Quaternion.Euler(Quaternion.Inverse(current) * rot);
        // Make sure we round the numbers to avoid accumulative error
        // We can do this because we deal in steps of 90 degrees anyway
        float x = Mathf.Round(target.eulerAngles.x);
        float y = Mathf.Round(target.eulerAngles.y);
        float z = Mathf.Round(target.eulerAngles.z);
        target = Quaternion.Euler(x, y, z);
        return target;
    }

    private Vector3 upDownRotationAxis()
    {
        float rot = Camera.main.transform.rotation.eulerAngles.y;
        if (rot <= 45 || rot > 315)
        {
            return Vector3.right;
        }
        else if (rot > 45 && rot <= 135)
        {
            return Vector3.back;
        }
        else if (rot > 135 && rot <= 225)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.forward;
        }
    }


    public void RotateDown()
    {
        
        if (GetComponent<VRTK_InteractableObject>() && GetComponent<VRTK_InteractableObject>().IsTouched())
            {

            //transform.Rotate(0, 0, 90);
            //transform.Rotate(Vector3.down, 90.0f, Space.World);
            Rotate_Down();
            // See if valid
            if (isValidGridPos())
            {
                PlayActionsSound();
                // It's valid. Update grid.
                updateGrid();
            }
            else
                // It's not valid. revert.
                //transform.Rotate(0, 0, -90);
                //transform.Rotate(Vector3.down, -90.0f, Space.World);
                Rotate_Up();

        }
        
    }


  public  void RotateTop()
    {
        
        if (GetComponent<VRTK_InteractableObject>() && GetComponent<VRTK_InteractableObject>().IsTouched())
        {

            //transform.Rotate(0, 0, -90);
            //transform.Rotate(Vector3.up, 90.0f, Space.World);
            Rotate_Up();

            // See if valid
            if (isValidGridPos())
            {
                PlayActionsSound();
                // It's valid. Update grid.
                updateGrid();
            }
            else
                // It's not valid. revert.
                // transform.Rotate(0, 0, 90);
                //transform.Rotate(Vector3.up, -90.0f, Space.World);
                Rotate_Down();

        }

    }


   

    public  void RotateLeft()
    {
       
        if (GetComponent<VRTK_InteractableObject>() && GetComponent<VRTK_InteractableObject>().IsTouched())
        {

            // transform.Rotate(0, 90, 0);
            //transform.Rotate(Vector3.left, 90.0f, Space.World);
            Rotate_Left();


            // See if valid
            if (isValidGridPos())
            {
                PlayActionsSound();
                // It's valid. Update grid.
                updateGrid();
            }
            else
                // It's not valid. revert.
                // transform.Rotate(0, -90, 0);
                //transform.Rotate(Vector3.left, -90.0f, Space.World);
                Rotate_Right();

        }
    }

 public   void RotateRight()
    {
        
        if (GetComponent<VRTK_InteractableObject>() && GetComponent<VRTK_InteractableObject>().IsTouched())
        {

            //transform.Rotate(0, -90, 0);
            //transform.Rotate(Vector3.right, 90.0f, Space.World);
            Rotate_Right();

            // See if valid
            if (isValidGridPos())
            {
                PlayActionsSound();
                // It's valid. Update grid.
                updateGrid();
            }
            else
                // It's not valid. revert.
                //transform.Rotate(0, 90, 0);
                //transform.Rotate(Vector3.right, -90.0f, Space.World);
                Rotate_Left();

        }
    }


    void ForceGoodPosition()
    {

        int dirx; int diry; int dirz;


        do
        {
             dirx = 0;  diry = 0;  dirz = 0;

            transform.position = Grid.roundVec3(transform.position);


            foreach (Transform child in transform)
            {
                
                if (!child.name.StartsWith("Cube")) continue;

                Vector3 v = Grid.roundVec3(child.position);

                
                if ((Mathf.Sign(v.x) == 1) && v.x >= Grid.max_x)
                {
                    dirx = -1;
                }
                else
                if ((Mathf.Sign(v.x) == -1) && v.x <= Grid.min_x)
                {
                    dirx = 1;
                }


                if ((Mathf.Sign(v.y) == 1) && v.y >= Grid.max_y)
                {
                    diry = -1;
                }
                else
               if ((Mathf.Sign(v.y) == -1) && v.y <= Grid.min_y)
                {
                    diry = 1;
                }

                if ((Mathf.Sign(v.z) == 1) && v.z >= Grid.max_z)
                {
                    dirz = -1;
                }
                else
             if ((Mathf.Sign(v.z) == -1) && v.z <= Grid.min_z)
                {
                    dirz = 1;
                }

                if (dirx != 0 || diry != 0 || dirz != 0)
                {
                   break;
                }
            }//forecah

            if ((dirx != 0) || (diry != 0) || (dirz != 0))
            {
                transform.position += new Vector3(dirx, diry, dirz);

            }

            }
        while  (dirx!=0 || diry!=0 || dirz!=0);

        if (!isValidGridPos())
        {
           // Destroy(gameObject);
            PlayHitGroundSound();
            transform.position = new Vector3(transform.position.x, Grid.max_y, transform.position.z);
        }
        

        }



    bool isValidGridPos()
    {
        Transform temp;

        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("Cube")) continue;

             Vector3 v = Grid.roundVec3(child.position);
           

           /*if (!Grid.insideMainBox(v))
                 return false;*/

            if (!Grid.AboveFloor(v)) return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid.TryGetValue(new Vector3(v.x, v.y, v.z), out temp) && temp !=null &&
                Grid.grid[new Vector3(v.x, v.y, v.z)].parent != transform)
                return false;
        }
        return true;
    }



    void updateGrid()
    {
        Transform temp;

        // Remove old children from grid
        for (int y = 0; y < Grid.max_y; ++y)
            for (int x = Grid.min_x; x < Grid.max_x; ++x)
                for (int z = Grid.min_z; z < Grid.max_z; ++z)
                    if (Grid.grid.TryGetValue(new Vector3(x, y, z), out temp) && temp !=null)
                    if (Grid.grid.TryGetValue(new Vector3(x, y, z), out temp) && temp != null && (Grid.grid[new Vector3(x, y, z)].parent == transform))
                             Grid.grid.Remove(new Vector3(x, y, z));

       

        // Add new children to grid
        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("Cube")) continue;

            Vector3 v = Grid.roundVec3(child.position);

            
            if (child == null)  { continue; }

             Grid.grid[new Vector3(v.x, v.y, v.z)] = child;
         }

      }


    void SetLayer( int newLayer  )
    {
        gameObject.layer = newLayer;
        gameObject.tag = "Fixed";

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = newLayer;
        }
    }


    void PlayHitGroundSound()
    {
        AudioSource.PlayClipAtPoint(hitGround, transform.position);
    }

    void PlayActionsSound()
    {
        AudioSource.PlayClipAtPoint(hitAction, transform.position);
    }

   


    void Update()
    {
        if (start > 0.0f)
        {
            float frac = Mathf.Min((Time.time - start) / speed, 1.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, frac);
            if ((Time.time - start) > speed)
            {
                // Rotation has reached its end, we stop moving
                start = 0.0f;
                Debug.Log("FINISH");
            }
        }

        if (!GetComponent<VRTK_InteractableObject>().IsGrabbed())
        {
            currY = transform.position.y;
            // Move Downwards and Fall
            if ((Time.time - lastFall >= FallVelocity)|| forceDown)
            {
                // ForceGoodPosition();
                // Modify position
                transform.position += new Vector3(0, -1, 0);

                // See if valid
                if (isValidGridPos())
                {
                    // It's valid. Update grid.
                    updateGrid();
                }
                else
                {
                    if (isValidGridPos()) { };
                    // It's not valid. revert.
                    transform.position += new Vector3(0, 1, 0);


                    //Freeze the Object
                    SetLayer(8);


                    // Clear filled horizontal lines
                    Grid.deleteFullRows();


                    // Spawn next Group
                    FindObjectOfType<Spawner>().spawnNext();

                    // Disable script
                    enabled = false;
                }

                lastFall = Time.time;
            }
            
        }
        else
        {
            if (transform.position.y > currY) transform.position = new Vector3(transform.position.x,currY, transform.position.z);
        }
    }
}
