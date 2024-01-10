using DG.Tweening;
using UnityEngine;

public class LevelFocus : MonoBehaviour
{
    public Transform World;
    [SerializeField] float rotationSpeed = 2f;

    private Vector3 mouseWorldPos;
    private Vector3 mousePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Use GetMouseButtonDown to trigger rotation only on click
        {
            rotateWorld();
        }
    }

    private void rotateWorld()
    {
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - World.position.z);

        // Convert the screen space mouse position to world space.
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calculate the direction from the sphere's position to the clicked position
        Vector3 direction = mouseWorldPos - World.position;

        // Use LookRotation to create a rotation based on the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Apply the rotation using DOTween
        World.DORotateQuaternion(targetRotation, rotationSpeed);
    }
}