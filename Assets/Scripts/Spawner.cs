using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;



#if UNITY_EDITOR
     using UnityEditor;
 #endif


public class Spawner : MonoBehaviour {


    // Groups
    public GameObject[] groups;

    public AudioClip SpawnAudio;
    public AudioClip GameOverAudio;


    // Use this for initialization
    void Start () {
        // Spawn initial Group
      //  spawnNext();
               
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void spawnNext()
    {


        /*   int i = Random.Range(0, 1);
           if (i == 0)
           {
               i = 7;
           }
           else
           { i = 0; }

           Instantiate(groups[i],
                       transform.position,
                       Quaternion.identity);*/


        PlaySpawnSound();
        // Spawn Group at current Position
        // Random Index
        int i = Random.Range(0, groups.Length);
         Instantiate(groups[i],
                     transform.position,
                     Quaternion.identity);
    }

    public void OnStartButton()
    {
        GameObject.Find("TextOver").GetComponent<Text>().text = "";

        if (Grid.grid.Count > 0)
        {
            Grid.ResetGrid();
            Grid.grid.Clear();
            SceneManager.LoadScene("Tetris01");
           // spawnNext();
        
    }
       
            GameObject.Find("BtnStart").GetComponent<Button>().interactable = false;

            spawnNext();
                      
  


    }

    public void OnExitButton()
    {
        //Only in .exe
        // 

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif


    }


    void PlaySpawnSound()
    {
        AudioSource.PlayClipAtPoint(SpawnAudio, transform.position);
    }


    void PlayGameOverSound()
    {
        AudioSource.PlayClipAtPoint(GameOverAudio, transform.position);
    }


    public void GameOver()
    {
        PlayGameOverSound();
        GameObject.Find("TextOver").GetComponent<Text>().text = "GAME OVER";
        GameObject.Find("BtnStart").GetComponent<Button>().interactable = true;

    }
}
