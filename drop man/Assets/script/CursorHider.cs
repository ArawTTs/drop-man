using UnityEngine;

public class CursorHider : MonoBehaviour
{
    [SerializeField]
    private KeyCode hideCursorKey = KeyCode.H; // The key to hide/show the cursor

    private bool isCursorVisible = true;

    private void Start()
    {
        // Make the cursor visible at the beginning
        Cursor.visible = true;
    }

    private void Update()
    {
        // Check if the hideCursorKey is pressed
        if (Input.GetKeyDown(hideCursorKey))
        {
            isCursorVisible = !isCursorVisible;
            Cursor.visible = isCursorVisible;

            // Lock or unlock the cursor based on visibility
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
