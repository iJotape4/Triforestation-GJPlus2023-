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
        private Collider dominoCollider;
        public TokenData tokenData;

        // Define the valid rotation angles for upwards tokens
        float[] validUpwardsRotations = new float[] { 0f, 120f, 240f };

        private void Awake()
        {
            poles = GetComponentsInChildren<DominoPole>();
            dominoCover = GetComponent<SpriteRenderer>();
            dominoCollider = GetComponent<Collider>();

        }
        private void OnDestroy()
        {
            
        }

        private void SetActive(bool eventData)
        {
           dominoCover.enabled = eventData;
           dominoCollider.enabled = eventData;
        }


        public bool IsUpwards()
        {
            // Calculate the actual rotation of the token
            float actualRotation = transform.localEulerAngles.y;
            // Normalize the rotation to the range [0, 360]
            actualRotation = (360 + actualRotation) % 360;

            // Check if the actualRotation is within the valid rotations for upwards tokens
            float tolerance = 0.1f; // Adjust this tolerance as needed

            // Check if the actualRotation is pointing upwards
            bool isUpwardsValid = validUpwardsRotations.Any(angle => Mathf.Abs(angle - actualRotation) < tolerance);

            return isUpwardsValid;


            // CODE FOR 2D TRIFORESTATION
            /*
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
            */
        }

        public bool IsValidBiome()
        {
            // Dictionary to store connection information for each pole position
            Dictionary<ENUM_PolePosition, bool[]> poleConnections = new Dictionary<ENUM_PolePosition, bool[]>
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
                    Quaternion rotation = Quaternion.Euler(0, pole.transform.eulerAngles.y + angleOffset, 0); // Rotate in the Y-axis
                    Vector3 directionVector = rotation * Vector3.forward; // Use Vector3.forward for the X-Z plane

                    int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");

                    RaycastHit hit;

                    if (Physics.Raycast(pole.transform.position, directionVector, out hit, 1.2f, dominoPoleLayerMask))
                    {
                        DominoPole hitPole = hit.collider.GetComponent<DominoPole>();

                        if (hitPole != null && (hitPole.biome == pole.biome || ((int)hitPole.biome == -1)))
                        {
                            if (direction == 0)
                            {
                                poleConnections[pole.position][1] = true; // "left" connection
                            }
                            else
                            {
                                poleConnections[pole.position][0] = true; // "right" connection
                            }
                        }
                    }
                }
            }

            // Check if any of the specified connections are valid
            if ((poleConnections[ENUM_PolePosition.Position1][0] && poleConnections[ENUM_PolePosition.Position3][1]) ||
                (poleConnections[ENUM_PolePosition.Position1][1] && poleConnections[ENUM_PolePosition.Position2][0]) ||
                (poleConnections[ENUM_PolePosition.Position2][1] && poleConnections[ENUM_PolePosition.Position3][0]))
            {
                print("si");
                return true;
            }
            else
            {
                print("no");
                return false;
            }
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

            for(int i=0; i<poles.Length; i++)
            {
                poles[i].AssignBiome(tokenData.biomes[i]);
            }
        }

        public void ActiveDrag()
        {
            dominoCollider.enabled = true;
        }

        private void OnDrawGizmos()
        {
            foreach (DominoPole pole in poles)
            {
                for (int direction = 0; direction < 2; direction++)
                {
                    float angleOffset = direction == 0 ? 60f : -60f;
                    Quaternion rotation = Quaternion.Euler(0, pole.transform.eulerAngles.y + angleOffset, 0); // Rotate in the Y-axis
                    Vector3 directionVector = rotation * Vector3.forward; // Use Vector3.forward for the X-Z plane

                    Gizmos.color = Color.red; // Set the color of the Gizmo line
                    Gizmos.DrawRay(pole.transform.position, directionVector * 1.2f); // Draw the ray

                    int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");

                    RaycastHit hit;

                    if (Physics.Raycast(pole.transform.position, directionVector, out hit, 1.2f, dominoPoleLayerMask))
                    {
                        Gizmos.color = Color.green; // Set the color of the Gizmo line for successful hit
                        Gizmos.DrawLine(pole.transform.position, hit.point);
                    }
                }
            }
        }
    }
}
