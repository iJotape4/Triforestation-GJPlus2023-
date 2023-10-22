using UnityEngine;

namespace LevelSelector
{
    public class LobbyCamera : MonoBehaviour
    {
        public PolygonCollider2D limitCollider;
        float pointer_x;
        float pointer_y;

        [SerializeField] PregamePopUp pregamePopUP;
        bool canMove = true;

        private void Awake()
        {
            pregamePopUP = FindObjectOfType<PregamePopUp>();
            pregamePopUP.popUpEnabled += SwitchCameraActivation;
        }

        private void SwitchCameraActivation(bool activated) => canMove = !activated;

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
                        MoveCamera();
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
                        MoveCamera();
                    }
                    }
#endif
                
            }
        }

        void MoveCamera()
        {
            Vector3 vec3 = new Vector3(
                    Mathf.Clamp(gameObject.transform.position.x - pointer_x, limitCollider.bounds.min.x, limitCollider.bounds.max.x),
                    Mathf.Clamp(gameObject.transform.position.y - pointer_y, limitCollider.bounds.min.y, limitCollider.bounds.max.y),
                    0);
            gameObject.transform.position = Vector3.MoveTowards(transform.position, vec3, 0.3f);
        }
    }
}