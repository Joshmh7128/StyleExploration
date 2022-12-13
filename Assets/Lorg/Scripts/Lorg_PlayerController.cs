using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lorg_PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null; //remember the camera
    [SerializeField] float mouseSensitivity = 3.5f; //set the sensitivity of how fast we're rotating camera
    [SerializeField] float walkSpeed = 6.0f; //speed
    [SerializeField] float gravity = -13.0f; // gravity to influence our movement
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; //smoothing out the position when we stop walking 
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f; //same with mouse

    [SerializeField] bool lockCursor = true; //no cursor bool

    float cameraPitch = 0.0f; //rotation of the transverse axis
    float velocityY = 0.0f;
    CharacterController controller = null; //we use the character controller component to simplify the Move function 

    Vector2 currentDir = Vector2.zero; //direction  of the camera
    Vector2 currentDirVelocity = Vector2.zero; 

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>(); //remember controller
        if(lockCursor) //ger rid of the cursor
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //create a vector2d based of our current axis imput

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime); //and update it smoothly

        cameraPitch -= currentMouseDelta.y * mouseSensitivity; //apply sensitivity to how fast we update the pitch
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f); //and lock the pitch in 180 degrees angle to not be able to move our head around

        playerCamera.localEulerAngles = Vector3.right * cameraPitch; //so the localEularAngles consists of all 3 separate rotation
        //we're setting it as a camera pitch calculated previosly and basically the Vecotr3(1,0,0)
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); //updating the rotation with delta created, considering sensitivity
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //get the new dir of the axis set in the project settings
        targetDir.Normalize(); //normalize it! cause we normalize vectors so they aren't too long

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); //smoothing it our

        if(controller.isGrounded) //if the object was dragged to the ground
            velocityY = 0.0f; //we set the velocity to zero so we would stay on the ground

        velocityY += gravity * Time.deltaTime; //if it's not we are getting dragged to the ground depending on the gravity scale

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY; //creating velocity based of all current modifiers

        controller.Move(velocity * Time.deltaTime); //actually move it with the controller component

    }
}
