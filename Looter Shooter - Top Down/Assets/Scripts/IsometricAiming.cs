using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricAiming : MonoBehaviour
{
    #region Datamembers

    #region Editor Settings

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject aimLine;

    #endregion
    #region Private Fields

    private Camera mainCamera;
    private LineRenderer aimLineRenderer;
    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        // Cache the camera, Camera.main is an expensive operation.
        mainCamera = Camera.main;
        aimLineRenderer = GameObject.Find("AimLine").GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Aim();
    }

    #endregion

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;



            // Update the positions of the Line Renderer.
            aimLineRenderer.SetPosition(0, transform.position);
            aimLineRenderer.SetPosition(1, position);

            //Enable the Line Renderer component of the AimLine object.
            aimLine.GetComponent<LineRenderer>().enabled = true;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            aimLine.GetComponent<LineRenderer>().enabled = false;
            return (success: false, position: Vector3.zero);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5f);
    }


    #endregion
}


