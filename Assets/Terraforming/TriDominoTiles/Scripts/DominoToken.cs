using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terraforming.Dominoes
{
    public class DominoToken : MonoBehaviour
    {
        [SerializeField] DominoPole[] poles;
        Vector2 pos;
        Vector2 dir;
        // Create a List to store raycast directions
        private List<Vector2> raycastDirections = new List<Vector2>();

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
        }

        public bool IsValidRotation(float targetRotation)
        {
            // Calculate the actual rotation of the token
            float actualRotation = transform.localEulerAngles.z;
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

            print("valid rotation");
            return isUpwardsValid || isDownwardsValid;
        }

        public bool IsValidBiome()
        {
            foreach (DominoPole pole in poles)
            {
                for (int direction = 0; direction < 2; direction++)
                {
                    float angleOffset = direction == 0 ? 60f : -60f;
                    Quaternion rotation = Quaternion.Euler(0, 0, pole.transform.eulerAngles.z + angleOffset);
                    Vector2 directionVector = rotation * Vector2.up;

                    int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");
                    pos = pole.transform.position;
                    dir = directionVector;

                    raycastDirections.Add(directionVector);

                    RaycastHit2D hit = Physics2D.Raycast(pole.transform.position, directionVector, 0.8f, dominoPoleLayerMask);

                    if (hit.collider != null && (dominoPoleLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        DominoPole hitPole = hit.collider.GetComponent<DominoPole>();

                        if (hitPole != null && hitPole.biome == pole.biome)
                        {
                            print("los biomas coinciden");
                            continue;
                        }
                        else
                        {
                            print("un bioma no coincidio");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        public void TurnOnColliders()
        {
            foreach(DominoPole pole in poles)
            {
                pole.TurncolliderOn();
            }
        }

        // Draw the raycast directions in the Update method
        private void Update()
        {
            foreach (Vector2 direction in raycastDirections)
            {
                Debug.DrawRay(transform.position, direction * 0.8f, Color.red); // Adjust the length (10f) and color as needed
            }
        }

    }



}
