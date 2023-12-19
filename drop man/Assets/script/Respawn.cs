using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    public CharacterController controller;

    private Vector3 checkpointPosition;

    void Start()
    {
        checkpointPosition = player.transform.position; // Set the initial checkpoint position to the player's starting position
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (respawnPoint != null)
            {
                checkpointPosition = player.transform.position; // Update the checkpoint position to the current respawn point
                Debug.Log("Checkpoint Reached\n");
            }
            else
            {
                Debug.LogWarning("Respawn point is not assigned!");
            }
        }
        else if (other.CompareTag("DeathZone"))
        {
            controller.enabled = false; // Prevents the CharacterController from overriding the transform
            player.transform.position = checkpointPosition; // Move the player to the checkpoint position
            Debug.Log("Player Respawned\n");
            controller.enabled = true; // Gives control back to the CharacterController
        }
    }
}
