using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Events;

namespace Terraforming.Dominoes
{
    public class DominoToken : MonoBehaviour
    {
        [SerializeField] public DominoPole[] poles;
        [SerializeField] SpriteRenderer dominoCover;
        public float uncoverDuration = 1f;
        // Create a List to store raycast directions
        private List<Vector2> raycastDirections = new List<Vector2>();
        private Collider2D dominoCollider;

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
            dominoCover = GetComponent<SpriteRenderer>();
            dominoCollider = GetComponent<Collider2D>();

        }
        private void OnDestroy()
        {
            
        }

        private void SetActive(bool eventData)
        {
           dominoCover.enabled = eventData;
           dominoCollider.enabled = eventData;
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
            // Dictionary to store connection information for each pole position
            Dictionary<ENUM_PolePosition , bool[]> poleConnections = new Dictionary<ENUM_PolePosition, bool[]>
        {
            { ENUM_PolePosition.Position1, new bool[2] }, // Index 0 is for "right", and Index 1 is for "left"
            { ENUM_PolePosition.Position2, new bool[2] },
            { ENUM_PolePosition.Position3, new bool[2] }
};
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

                        if (direction == 0)
                        {
                            poleConnections[pole.position][0] = true; // "right" connection
                        }
                        else
                        {
                            poleConnections[pole.position][1] = true; // "left" connection
                        }
                    }

                }
            }
            // Check if any of the specified connections are valid
            if ((poleConnections[ENUM_PolePosition.Position1][0] && poleConnections[ENUM_PolePosition.Position3][1]) || // Pole 1 Right Raycast and Pole 3 Left Raycast
                (poleConnections[ENUM_PolePosition.Position1][1] && poleConnections[ENUM_PolePosition.Position2][0]) || // Pole 1 Left Raycast and Pole 2 Right Raycast
                (poleConnections[ENUM_PolePosition.Position2][1] && poleConnections[ENUM_PolePosition.Position3][0]))   // Pole 2 Left Raycast and Pole 3 Right Raycast
            {
                return true;
            }
            else
            {
                return false;
            }

            return false;
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
