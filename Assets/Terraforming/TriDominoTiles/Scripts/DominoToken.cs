using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
namespace Terraforming.Dominoes
{
    public class DominoToken : MonoBehaviour
    {
        [SerializeField] DominoPole[] poles;
        [SerializeField] SpriteRenderer dominoCover;
        public float uncoverDuration = 1.0f;
        // Create a List to store raycast directions
        private List<Vector2> raycastDirections = new List<Vector2>();
        private Collider2D dominoCollider;

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
            dominoCover = GetComponent<SpriteRenderer>();
            dominoCollider = GetComponent<Collider2D>();
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


                    RaycastHit2D hit = Physics2D.Raycast(pole.transform.position, directionVector, 0.8f, dominoPoleLayerMask);

                    if (hit.collider != null)
                    {
                        DominoPole hitPole = hit.collider.GetComponent<DominoPole>();

                        if (hitPole != null && hitPole.biome == pole.biome)
                        {
                            continue;
                        }
                        else
                        {
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
                pole.TurnColliderOn();
            }
        }

        public void TurnOffColliders()
        {
            foreach (DominoPole pole in poles)
            {
                pole.TurnColliderOff();
            }
        }

        public void UncoverDomino()
        {
            // Rotate the GameObject by 90 degrees in the Y-axis
            transform.DORotate(new Vector3(0, 90, 0), uncoverDuration)
                .OnComplete(() =>
                {
                    // Deactivate the dominoCover
                    dominoCover.enabled = false;

                    // Rotate the GameObject back to 0 degrees
                    transform.DORotate(Vector3.zero, uncoverDuration);
                });
        }

        public void ResetDomino()
        {
            dominoCover.enabled = true;
            dominoCollider.enabled = false;
            foreach (DominoPole pole in poles)
            {
                pole.AssignBiome();
            }
        }

        public void ActiveDrag()
        {
            dominoCollider.enabled = true;
        }
    }



}
