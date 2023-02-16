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
        #endregion

        #region Private Fields
        private Rigidbody rb;
        private Camera cam;
        #endregion

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Change Collision detection mode
            cam = Camera.main;
        }

        void Update()
        {
            // Get the input vector for movement (horizontal and vertical)
            Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (movementInput.magnitude > 0)
            {
                // Calculate the movement direction based on the input and the camera orientation
                Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
                Vector3 movementDirection = movementInput.z * cameraForward + movementInput.x * cam.transform.right;

                // Rotate the player towards the movement direction
                if (movementDirection.magnitude > 0)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                // Move the player in the movement direction
                rb.velocity = movementDirection.normalized * movementSpeed;
            }
            else
            {
                // Stop the player if there is no input
                rb.velocity = Vector3.zero;
            }
        }
    }

}
