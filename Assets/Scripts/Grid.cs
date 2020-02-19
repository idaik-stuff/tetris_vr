using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

   

    // The Grid itself
    public static int max_x;
    public static int max_y;
    public static int max_z;


    public static int min_x;
    public static int min_y;
    public static int min_z;


    public static AudioClip FullRow;

    public static  Dictionary<Vector3, Transform> grid = new Dictionary<Vector3, Transform>();


    

    // Use this for initialization
    void Awake () {

        max_x = GetComponent<ZonetoPlay>().maxi_x;
        max_y = GetComponent<ZonetoPlay>().maxi_y;
        max_z = GetComponent<ZonetoPlay>().maxi_z;

        FullRow = GetComponent<ZonetoPlay>().FullRowBomb;

        min_x = -1 * max_x;
        min_y = 0;
        min_z = -1 * max_z;

       
}

     public static void ResetGrid()
    {
        for (int y = 0; y < max_y; ++y)
        {
           
                deleteRow(y);
               // decreaseRowsAbove(y + 1);
               // --y;
            
        }

    }


    public Vector3 GetMaxUsedY(Vector3 ini)
    {
        Transform temp;
        Vector3 MaxUsed_y = Vector3.zero;


        for (int y = min_y ; y < max_y; ++y)
        {
            if (grid.TryGetValue(new Vector3(ini.x, y, ini.z), out temp))
            {
                if (temp.parent.tag=="Fixed") MaxUsed_y = new Vector3(ini.x, y, ini.z);

            }
        }

        MaxUsed_y += new Vector3(0, 1, 0);

        return MaxUsed_y;
    }

    public static Vector3 roundVec3(Vector3 v)
    {
        

        return new Vector3((float)System.Math.Round(v.x),
                            (float)System.Math.Round(v.y),
                            (float)System.Math.Round(v.z));

    }


    public static bool insideMainBox(Vector3 pos)
    {
        return (Mathf.Abs((int)pos.x) >= 0 && Mathf.Abs((int)pos.x) < max_x &&
                Mathf.Abs((int)pos.z) >= 0 && Mathf.Abs((int)pos.z) < max_z &&
                (int)pos.y >= 0 );
    }


    public static bool AboveFloor(Vector3 pos)
    {
        return ( (int)pos.y >= 0);
    }


     public static void deleteRow(int y)
    {
        Transform temp;
        for (int x = min_x+1; x < max_x; ++x)
        {
            for (int z = min_z+1; z < max_z; ++z)
            {
                if (grid.TryGetValue(new Vector3(x, y, z), out temp))
                {
                    PlayFullRowSound(temp.transform);

                    Destroy(temp.gameObject);
                    //grid[new Vector3(x, y, z)] = null;
                    grid.Remove(new Vector3(x, y, z));
                    
                }
            }
            
        }
    }

    public static void PlayFullRowSound(Transform t)
    {
        AudioSource.PlayClipAtPoint(FullRow, t.position);
    }

    public static void decreaseRow(int y)
    {
        Transform temp;
        for (int x = min_x+1; x < max_x; ++x)
        {
            for (int z = min_z+1; z < max_z; ++z)
            {
                if (grid.TryGetValue(new Vector3(x, y, z), out temp) && temp!=null)
                {
                    // Move one towards bottom
                    grid[new Vector3(x, y - 1, z)] = grid[new Vector3(x, y, z)];
                    
                    grid.Remove(new Vector3(x, y, z));

                    // Update Block position
                    grid[new Vector3(x, y - 1, z)].position += new Vector3(0, -1, 0);
                    
                }
            }
        }
    }

    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < max_y; ++i)
            decreaseRow(i);
    }


   
    public static bool isRowFull(int y)
    {
        Transform temp;
        for (int x = min_x+1; x < max_x; ++x)
        {
            for (int z = min_z+1; z < max_z; ++z)
            {
                if  (!grid.TryGetValue(new Vector3(x, y, z), out temp) || temp==null)
                    return false;
            }
        }

        return true;
    }

    public  static void deleteFullRows()
    {
        for (int y = 0; y < max_y; ++y)
        {
            if (isRowFull(y))
            {
                if (GameObject.Find("/ScoreBoard")!=null) GameObject.Find("/ScoreBoard").GetComponent<ScoreBorad>().AddPoints();
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
            }
        }
    }


}


