using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControllerIdiom : MonoBehaviour
{
    // script handles movement of the player
    [Header("Movement")]
    Vector3 moveH, moveV, move;
    [SerializeField] Transform playerHead;
    [SerializeField] CharacterController characterController; // our character controller
    [SerializeField] float moveSpeed, gravity, jumpVelocity, playerJumpVelocity; // set in editor for controlling
    float gravityValue, verticalVelocity; // hidden because is calculated
    bool landed;
    [SerializeField] Transform respawnPoint;

    [Header("Camera")]
    [SerializeField] float aimSensitivity;
    [SerializeField] float minYAngle, maxYAngle;
    float currentSensitivity, yRotate, xRotate;

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // declare our motion
        float pAxisV = Input.GetAxisRaw("Vertical");
        float pAxisH = Input.GetAxisRaw("Horizontal");
        moveV = playerHead.forward * pAxisV;
        moveH = playerHead.right * pAxisH;

        RaycastHit groundedHit;
        Physics.Raycast(transform.position, Vector3.down, out groundedHit, 1.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        // movement application
        // jump calculations
        gravityValue = gravity;

        if (groundedHit.transform == null)
        {
            playerJumpVelocity += gravityValue * Time.deltaTime;
            landed = false;
        }
        else if (groundedHit.transform != null)
        {
            // jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerJumpVelocity = Mathf.Sqrt(jumpVelocity * -3.0f * gravity);
            }
            else if (!landed)
            {
                playerJumpVelocity = 0f;
                landed = true;
            }
        }

        verticalVelocity = playerJumpVelocity;
        move = new Vector3((moveH.x + moveV.x), verticalVelocity / moveSpeed, (moveH.z + moveV.z));
        characterController.Move(move * Time.deltaTime * moveSpeed);

        // our camera control
        currentSensitivity = aimSensitivity;
        // run math to rotate the head of the player as we move the mouse
        yRotate += (Input.GetAxis("Mouse Y") * -currentSensitivity * Time.deltaTime);
        // clamp the rotation so we don't go around ourselves
        yRotate = Mathf.Clamp(yRotate, minYAngle, maxYAngle);
        // calculate our X rotation
        xRotate += (Input.GetAxis("Mouse X") * currentSensitivity * Time.deltaTime);
        // add in our rotate mods if we have any
        float finalxRotate = xRotate;
        float finalyRotate = yRotate;

        Mathf.SmoothStep(xRotate, finalxRotate, 5 * Time.deltaTime);
        Mathf.SmoothStep(yRotate, finalyRotate, 5 * Time.deltaTime);

        // apply it to our head
        playerHead.eulerAngles = new Vector3(finalyRotate, finalxRotate, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Deadly")
        {
            characterController.enabled = false;
            transform.position = respawnPoint.position;
            characterController.enabled = true;
        }
    }
}