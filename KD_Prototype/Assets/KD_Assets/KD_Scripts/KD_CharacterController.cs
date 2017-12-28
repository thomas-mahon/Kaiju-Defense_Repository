using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KD_CharacterController : MonoBehaviour
{
    #region MouseFields
    public GameObject playerCamera;
    float mouseSensitivity = 1;

    float xAxisClamp = 0.0f;

    public float xRotMaxUp = -90;
    public float xRotMinDown = 90;
    #endregion

    #region MovementFields
    CharacterController characterController;
    float walkSpeed = 4;
    public bool isGrounded;
    Rigidbody rigidBody;
    #endregion

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    //Use this for every frame jolly good tip tip
    void Update()
    {
        RotateCamera();
        MovePlayer();
        GroundCheck();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = playerCamera.transform.rotation.eulerAngles;
        Vector3 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;

        targetRotBody.y += rotAmountX;

        if (xAxisClamp > xRotMinDown)
        {
            xAxisClamp = xRotMinDown;
            targetRotCam.x = xRotMinDown;
        }

        else if (xAxisClamp < xRotMaxUp)
        {
            xAxisClamp = xRotMaxUp;
            targetRotCam.x = -3 * xRotMaxUp;
        }

        playerCamera.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirSide = transform.right * horizontal * walkSpeed;
        Vector3 moveDirForward = transform.forward * vertical * walkSpeed;

        characterController.SimpleMove(moveDirSide);
        characterController.SimpleMove(moveDirForward);
    }

    bool GroundCheck()
    {
        if (characterController.isGrounded)
        {
            return true;
        }

        Vector3 bottom = characterController.transform.position
            - new Vector3(0, characterController.height / 2, 0);

        RaycastHit hit;

        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.2f))
        {
            characterController.Move(new Vector3 (0, -hit.distance, 0));
            return true;
        }

        return false;
    }
}