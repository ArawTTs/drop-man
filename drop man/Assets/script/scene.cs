using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene : MonoBehaviour
{

	public float scene_time = 4f; 
	
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(scene_clip());
    }
    
    IEnumerator scene_clip()
    {
		yield return new WaitForSeconds(scene_time);
		
		SceneManager.LoadScene(2);
	}

}
