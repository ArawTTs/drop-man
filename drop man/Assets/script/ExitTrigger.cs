using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public AudioClip exitSound; // Sound to play when the exit is triggered
    public int countdownDuration = 5; // Duration of the countdown in seconds

    private float countdownTimer; // Timer for the countdown
    private AudioSource audioSource; // AudioSource component to play the sound
    private bool exitTriggered = false; // Flag to track if the exit has been triggered

    private void Start()
    {
        countdownTimer = countdownDuration; // Set the initial countdown timer value
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component from the same game object
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !exitTriggered)
        {
            if (exitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(exitSound); // Play the exit sound if it's assigned and audioSource is not null
                exitTriggered = true; // Set the exitTriggered flag to true
                StartExitCountdown(); // Start the countdown when the exit is triggered
            }
        }
    }

    private void StartExitCountdown()
    {
        InvokeRepeating("DecreaseTimer", 1f, 1f); // Start a repeating Invoke to decrease the timer every second
    }

    private void DecreaseTimer()
    {
        countdownTimer--; // Decrease the countdown timer

        if (countdownTimer <= 0f)
        {
            ExitGame(); // Call the ExitGame function when the countdown reaches zero
        }
    }

    private void ExitGame()
    {
        Debug.Log("Exiting the game"); // Add a debug log to indicate game exit

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Exit the Unity editor play mode
        #else
            Application.Quit(); // Quit the standalone build of the game
        #endif
    }
}
