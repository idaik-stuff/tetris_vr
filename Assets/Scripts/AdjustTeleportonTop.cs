// Height Adjust Teleport|Scripts|0080

    using UnityEngine;
    using VRTK;


public class AdjustTeleportonTop : VRTK_HeightAdjustTeleport
    {
              

       

        protected override Vector3 GetNewPosition(Vector3 tipPosition, Transform target)
        {

       // Vector3 basePosition = base.GetNewPosition(tipPosition, target);
       
        if (target.tag == "Fixed")
        {

          

              Collider[] colliders;
               GameObject cube;

               colliders = Physics.OverlapSphere(tipPosition, 0.1f);

               if (colliders != null)
               {
                   foreach (Collider hit in colliders)
                   {
                       if (hit.gameObject.name.StartsWith("Cube"))
                       {
                           cube = hit.gameObject;

                        /* GameObject ScriptObj = GameObject.Find("/Scripts").gameObject;

                         if (ScriptObj != null)
                         {
                             basePosition = ScriptObj.GetComponent<Grid>().GetMaxUsedY(cube.transform.position);

                         }*/

                        //return (new Vector3(cube.transform.position.x, basePosition.y, cube.transform.position.z));

                        // return cube.transform.position;
                        tipPosition = cube.transform.position;
                        target = cube.transform;
                       }

                   }

                }
            //  GameObject obj = rayCollidedWith.collider.gameObject;
       
            // basePosition.y = GetTopY(tipPosition, target);

            // basePosition += Vector3.forward;
        }

        Vector3 basePosition = base.GetNewPosition(tipPosition, target);

        return basePosition;
        }


    
    }
