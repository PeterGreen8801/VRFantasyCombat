using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DetectTeleportationBlocker : MonoBehaviour
{
    public XRRayInteractor xRRayInteractor;

    public float defaultRaycastDistance = 10f;

    float rayDistance = 10f;

    private void Update()
    {
        RaycastHit hit;

        // Ensure xRRayInteractor is not null and is active
        if (xRRayInteractor != null && xRRayInteractor.enabled)
        {
            // Cast a ray from the xRRayInteractor's position and direction
            if (Physics.Raycast(xRRayInteractor.transform.position, xRRayInteractor.transform.forward, out hit, rayDistance))
            {
                // Check if the raycast hit a collider
                if (hit.collider != null)
                {
                    // If the collider hit has the TeleportationBlocker script, prevent teleportation
                    if (hit.collider.TryGetComponent(out TeleportationBlocker teleportationBlocker))
                    {
                        // Handle teleportation being blocked (e.g., display a message or prevent teleportation)
                        Debug.Log("Teleportation blocked by " + hit.collider.gameObject.name);
                        // Example: Prevent teleportation by disabling the teleporter or displaying a message
                        //xRRayInteractor.enabled = false;
                        xRRayInteractor.maxRaycastDistance = hit.distance;
                        // Or you could handle this in a different way based on your game logic
                    }
                    else
                    {
                        // If the collider hit does not have the TeleportationBlocker script, allow teleportation
                        // Reset or enable teleportation (if it was previously disabled)
                        //xRRayInteractor.enabled = true;
                        xRRayInteractor.maxRaycastDistance = defaultRaycastDistance;
                    }
                }
            }
        }
    }

    /*
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            // Check if the raycast hit a collider
            if (hit.collider != null)
            {
                // If the collider hit is not the Box Collider, allow teleportation
                if (!hit.collider.TryGetComponent(out TeleportationBlocker teleportationBlocker))
                {
                    
                }
                else
                {
                    
                }
            }
        }
    }
    */

}
