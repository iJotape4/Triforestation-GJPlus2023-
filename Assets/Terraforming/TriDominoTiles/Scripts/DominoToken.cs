using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terraforming.Dominoes
{
    public class DominoToken : MonoBehaviour
    {
        DominoPole[] poles;

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
        }

        public bool IsValidRotation(float targetRotation)
        {
            print(targetRotation);
          
            // Calculate the actual rotation of the token
            float actualRotation = transform.localEulerAngles.z;
            print(actualRotation);
            // Normalize both rotations to the range [0, 360]
            actualRotation = (360 + actualRotation) % 360;
            targetRotation = (360 + targetRotation) % 360;

            // Define the valid rotation angles for upwards and downwards tokens
            float[] validUpwardsRotations = new float[] { 0f, 120f, 240f };
            float[] validDownwardsRotations = new float[] { 60f, 180f, 300f };

            // Check if the targetRotation is within the valid rotations for upwards or downwards tokens
            float tolerance = 0.1f; // Adjust this tolerance as needed
            bool isUpwardsValid = validUpwardsRotations.Any(angle => Mathf.Abs(angle - actualRotation) < tolerance) && validUpwardsRotations.Any(angle => Mathf.Abs(angle - targetRotation) < tolerance);
            bool isDownwardsValid = validDownwardsRotations.Any(angle => Mathf.Abs(angle - actualRotation) < tolerance) && validDownwardsRotations.Any(angle => Mathf.Abs(angle - targetRotation) < tolerance);

            return isUpwardsValid || isDownwardsValid;
        }

        public bool IsValidBiome()
        {
            foreach (DominoPole pole in poles)
            {
                // Iterate through both possible directions (+60 and -60 degrees from Z rotation).
                for (int direction = 0; direction < 2; direction++)
                {
                    // Calculate the direction vector based on the direction index.
                    float angleOffset = direction == 0 ? 60f : -60f;
                    Quaternion rotation = Quaternion.Euler(0, 0, pole.transform.eulerAngles.z + angleOffset);
                    Vector3 directionVector = rotation * Vector3.up;

                    // Create a ray from the position of the DominoToken in the calculated direction.
                    Ray ray = new Ray(transform.position, directionVector);
                    RaycastHit hit;

                    // Define a layer mask for the DominoPole layer.
                    int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");

                    // Perform the raycast.
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, dominoPoleLayerMask))
                    {
                        DominoPole hitPole = hit.transform.GetComponent<DominoPole>();

                        // Check if the hit DominoPole has the same biome as the pole in the loop.
                        if (hitPole != null && hitPole.biome == pole.biome)
                        {
                            // Biome matches, continue checking the next pole.
                            continue;
                        }
                    }
                }
            }

            // If all poles have matching biomes in at least one direction, return false.
            return false;
        }

    }

}
