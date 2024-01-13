using Terraforming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Vector3 offset;
    CanvasGroup canvasGroup;
    public string destinationTag = "DropArea";

    RectTransform rectTransform;
    Vector3 originalRectPosition;
    [SerializeField] Sprite imageFile;
    Transform transform;
    private float distanceToCamera;
    Camera mainCamera;
    Canvas canvas;
    float movementSpeed=0.1f;
    Vector3 currentDragPosition;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        imageFile = GetComponent<Image>().sprite;
        mainCamera = Camera.main;
        canvas = FindObjectOfType<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalRectPosition = rectTransform.position;
        SpriteRenderer spr= gameObject.AddComponent<SpriteRenderer>();
        spr.sprite = imageFile;
        Destroy(GetComponent<Image>());
        Destroy(GetComponent<CanvasGroup>());
        Destroy(GetComponent<CanvasRenderer>());
        gameObject.transform.parent = null;
        transform = gameObject.AddComponent<Transform>();
       // gameObject.AddComponent<BoxCollider>();
        //dragView = gameObject.AddComponent<DragView>();
        //transform.position = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, distanceToCamera, eventData.position.y));
        //transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, distanceToCamera, Input.mousePosition.y));

        transform.position = TranslateFromCanvasToWorld(eventData);
        transform.rotation = Quaternion.Euler(90,0,0);
        distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        //transform.position = new Vector3(initialScreenToWorldPoint.x, 0f, initialScreenToWorldPoint.y);
        //Debug.Log("OnBeginDrag" + transform.position);
        ////Destroy(GetComponent<RectTransform>());
        ////transform.position = new Vector3(1.0f, 2.0f, 3.0f);
        //distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

    }

    public Vector3 TranslateFromCanvasToWorld(PointerEventData eventData)
    {
        Vector3 position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform,
            eventData.position, mainCamera, out position);
        Debug.Log(position);
        position = new Vector3(position.x, 0f, position.y) * movementSpeed;
        return position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = TranslateFromCanvasToWorld(eventData);

        currentDragPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, distanceToCamera, eventData.position.y));
        transform.position = currentDragPosition;
        Debug.Log(eventData.position);
        //Vector3 position;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform,
        //    eventData.position, canvas.worldCamera, out position);
        //transform.position = canvas.transform.TransformPoint(position);
        //Input.mousePosition + offset;
    }
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //    offset = transform.position - Input.mousePosition;
    //    //canvasGroup.alpha = 0.5f;
    //    //canvasGroup.blocksRaycasts = false;
    // }
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    RaycastResult raycastResult = eventData.pointerCurrentRaycast;
    //    if(raycastResult.gameObject?.tag == destinationTag)
    //    {
    //        transform.position = raycastResult.gameObject.transform.position;
    //    }
    //   // canvasGroup.alpha = 1;
    //    //canvasGroup.blocksRaycasts = true;
    //}

    public void OnEndDrag(PointerEventData eventData)
    {
       /* gameObject.transform.parent = FindObjectOfType<Canvas>().transform; 
        gameObject.AddComponent<RectTransform>();
        Image img = gameObject.AddComponent<Image>();
        img.sprite = imageFile;
        gameObject.AddComponent<CanvasGroup>();
        rectTransform.position = originalRectPosition;*/
    }
}
