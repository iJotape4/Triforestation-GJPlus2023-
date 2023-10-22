using TMPro;
using UnityEngine;

namespace LevelSelector
{
    public class LevelButton : MonoBehaviour
    {

        [SerializeField] public LevelData level;
        [SerializeField] SpriteRenderer[] stars;
        [SerializeField] public TextMeshProUGUI text;
        [SerializeField] PregamePopUp pregamePopUP;
        [SerializeField] CircleCollider2D circleCollider;

        private void Awake()
        {
            stars =  transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
            pregamePopUP = FindObjectOfType<PregamePopUp>(); 
            circleCollider= GetComponent<CircleCollider2D>(); 

            pregamePopUP.popUpEnabled+= SwitchButtonsActivation;
        }
        //private void Start()
        //{
        //    for (int i=0; i<stars.Length; i++)
        //    {
        //        if (!level.stars[i])
        //            stars[i].color= Color.black;
        //    }
        //}

        private void SwitchButtonsActivation(bool activated)=>
            circleCollider.enabled= !activated;

        public void OnMouseDown()=>   
            pregamePopUP.EnablePopUP(level) ;
        

        public void OnMouseUp()=>     
            pregamePopUP.EnablePlayButton();
        
    }
}
