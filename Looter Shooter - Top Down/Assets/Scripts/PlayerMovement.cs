using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.Core
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Data
        public float movementSpeed = 2f;
        public float rotationSpeed = 8f;
        public float jumpForce = 5f;
        public float gravityScale = 2f;
        public float jumpCooldown = 1f;
        public float airborneGravityScale = 3f;
        public float climbSpeed = 2f; //Speed at which the player climbs
        #endregion

        #region Private Fields
        private Rigidbody rb;
        private Camera cam;
        private bool isGrounded = true;
        private float lastJumpTime = -Mathf.Infinity;
        private bool isOnLadder = false; //Flag to indicate if Player is on ladder
        private float verticalInput; // Vertical input for climbing ladders
        private Ladder currentLadder = null; // Reference to the current ladder that the player is on
        #endregion

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Change Collision detection mode
            rb.freezeRotation = true;
            cam = Camera.main;
        }

        void Update()
        {
            // Get the input vector for movement (horizontal and vertical)
            Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //Stop Player from continuing to move downwards when they fall off high place
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }

            if (Time.time > lastJumpTime + jumpCooldown)
            {
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    lastJumpTime = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.Space) && isOnLadder && currentLadder != null)
                {
                    currentLadder.DetachPlayer(gameObject);
                }

            }

            if (movementInput.magnitude > 0 && !isOnLadder)
            {
                // Calculate the movement direction based on the input and the camera orientation
                Vector3 movementDirection = MoveDir(movementInput);

                // Rotate the player towards the movement direction
                if (movementDirection.magnitude > 0)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                // Move the player in the movement direction
                rb.velocity = movementDirection.normalized * movementSpeed;
            }
            else if (!isOnLadder) // If not on a ladder and no input, stop the player
            {
                // Stop the player if there is no input
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }

            if (Input.GetKeyDown(KeyCode.F) && isOnLadder && currentLadder != null)
            {
                rb.velocity = Vector3.zero;
                transform.position = currentLadder.transform.position;
                transform.parent = currentLadder.transform;
            }

            // Apply custom gravity
            if (isGrounded)
            {
                rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(Physics.gravity * airborneGravityScale, ForceMode.Acceleration);
            }
        }

        private Vector3 MoveDir(Vector3 movementInput)
        {
            Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movementDirection = movementInput.z * cameraForward + movementInput.x * cam.transform.right;
            return movementDirection;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        public void SetIsOnLadder(bool onLadder)
        {
            isOnLadder = onLadder;
        }
    }
}
