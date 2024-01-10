using DG.Tweening;
using Events;
using LevelSelector;
using System;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public GameObject world;
    float pointer_x;
    float pointer_y;
    [SerializeField] float rotationSpeed =2f;

    [SerializeField] PregamePopUp pregamePopUP;
    bool canMove = true;
    Quaternion initialRotation;

    private void Awake()
    {
        // pregamePopUP = FindObjectOfType<PregamePopUp>();
        // pregamePopUP.popUpEnabled += SwitchCameraActivation;
        EventManager.AddListener(ENUM_LevelSelectorEvent.LevelSelected, RestartRotation);

    }

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void RestartRotation()
    {
        transform.DORotate(initialRotation.eulerAngles, 0.4f, RotateMode.Fast);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_LevelSelectorEvent.LevelSelected, RestartRotation);

    }

    private void SwitchCameraActivation(bool arg0)
    {
        
    }

    void Update()
    {
        if (canMove)
        {

#if UNITY_STANDALONE_WIN || PLATFORM_STANDALONE_WIN 
                if (Input.GetMouseButton(0) || Input.touchCount == 1)
                {
                    if (Input.GetMouseButton(0))
                    {
                        pointer_x = Input.GetAxis("Mouse X");
                        pointer_y = Input.GetAxis("Mouse Y");
                    MoveWorld();
                    }
                }
#endif

#if UNITY_ANDROID
            if (Input.touchCount == 1)
            {

                Touch touchZero = Input.GetTouch(0);
                if (touchZero.phase == TouchPhase.Moved)
                {
                    pointer_x = Input.GetTouch(0).deltaPosition.x;
                    pointer_y = Input.GetTouch(0).deltaPosition.y;
                    MoveWorld();
                }
            }
#endif
        }
    }

    void MoveWorld()
    {
        Vector3 rotationVector = new Vector3(-pointer_y , pointer_x , 0f);

        transform.Rotate( Vector3.right, Mathf.LerpAngle(transform.eulerAngles.x, rotationVector.x, rotationSpeed), Space.World);
        transform.Rotate(Vector3.up, Mathf.LerpAngle(transform.eulerAngles.y, -rotationVector.y, rotationSpeed), Space.World);
    }
}