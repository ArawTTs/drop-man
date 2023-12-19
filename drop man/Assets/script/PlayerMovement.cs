using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float maximumSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    
    private float normalJump;
    public float boostedJump;
    public float jumpCooldown;
    private float normalSpeed;
    public float boostedSpeed;
    public float speedCooldown;
    
    private Vector3 targetPosition;  // The target position to move towards
    private Vector3 checkpointPosition;  // The last checkpoint position
    
    public AudioClip exitSound; // Sound to play when the exit is triggered
    public int countdownDuration = 5; // Duration of the countdown in seconds
    private float countdownTimer; // Timer for the countdown
    private AudioSource audioSource; // AudioSource component to play the sound
    private bool exitTriggered = false;
    public float waitTime = 2f; // Wait time before starting the countdown

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        
        normalJump = jumpSpeed;
        normalSpeed = maximumSpeed;
        
        targetPosition = transform.position;
        checkpointPosition = transform.position;
        
        countdownTimer = countdownDuration; // Set the initial countdown timer value
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component from the same game object
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        float speed = inputMagnitude * maximumSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
		
		 if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maximumSpeed * Time.deltaTime);
        }
		        
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
    	if(other.CompareTag("jumpBoost"))
    	{
			jumpSpeed = boostedJump;
			StartCoroutine("JumpDuration");
		}
		
		if(other.CompareTag("SpeedBoost"))
    	{
			maximumSpeed = boostedSpeed;
			StartCoroutine("JumpDuration");
		}
		
		if (other.CompareTag("Platform") && !exitTriggered)
        {
            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false; // Disable player movement

                if (exitSound != null && audioSource != null)
                {
                    exitTriggered = true; // Set the exitTriggered flag to true
                    StartExitCountdown(); // Start the countdown when the exit is triggered
                }
            }
        }
	}
	
	//time
	IEnumerator JumpDuration()
	{
		yield return new WaitForSeconds(jumpCooldown);
		jumpSpeed = normalJump;
	}
	
	IEnumerator SpeedDuration()
	{
		yield return new WaitForSeconds(speedCooldown);
		maximumSpeed = normalSpeed;
	}
	
	private IEnumerator DelayedExit()
{
    yield return new WaitForSeconds(exitSound.length); // Wait for the length of the exit sound

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit the Unity editor play mode
    #else
        Application.Quit(); // Quit the standalone build of the game
    #endif
}
	
	 public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;  // Set the checkpoint position
    }

    public void MoveToCheckpoint()
    {
        targetPosition = checkpointPosition;  // Move the player to the checkpoint position
    }
    
	private void StartExitCountdown()
	{
	    StartCoroutine(ExitCountdownWithDelay()); // Start a coroutine for the countdown with delay
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

    StartCoroutine(DelayedExit()); // Call a coroutine to delay the game exit
	}
	

	private IEnumerator ExitCountdownWithDelay()
	{
	    yield return new WaitForSeconds(waitTime); // Wait for the specified wait time before starting the countdown
	
	    StartCoroutine(ExitCountdown()); // Start the exit countdown coroutine
	}
	
	private IEnumerator ExitCountdown()
	{
    yield return new WaitForSeconds(exitSound.length); // Wait for the length of the exit sound

    Debug.Log("Exiting the game"); // Add a debug log to indicate game exit

    if (exitSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(exitSound); // Play the exit sound if it's assigned and audioSource is not null
    }

    yield return new WaitForSeconds(countdownDuration); // Wait for the specified countdown duration

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit the Unity editor play mode
    #else
        Application.Quit(); // Quit the standalone build of the game
    #endif
	}
}
