using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour
{
	public AudioSource audioSource;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && !audioSource.isPlaying)
		{
			audioSource.Play();
		}
	}
}
