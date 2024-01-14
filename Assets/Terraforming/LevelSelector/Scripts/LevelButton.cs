using Events;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSelector
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] public LevelData level;
        [SerializeField] PregamePopUp pregamePopUP;
        [SerializeField] Collider circleCollider;
        [SerializeField] ParticleSystem ps;
        Coroutine openPopUpRoutine;
        protected virtual string sceneToLoad => "TriangularGridWithTiles"; 
        const string levelSelectorScene = "LevelSelector";

        private void Awake()
        {
            pregamePopUP = FindObjectOfType<PregamePopUp>(); 
            circleCollider= GetComponent<Collider>(); 
            ps = GetComponentInChildren<ParticleSystem>();

            //pregamePopUP.popUpEnabled+= SwitchButtonsActivation;

            EventManager.AddListener(ENUM_LevelSelectorEvent.LevelSelected, UnselectNode);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(ENUM_LevelSelectorEvent.LevelSelected, UnselectNode);           
        }

        private void UnselectNode()
        {
            ps.Stop();
            if (openPopUpRoutine != null)
                StopCoroutine(openPopUpRoutine);
        }

        private void SwitchButtonsActivation(bool activated)=>
            circleCollider.enabled= !activated;

        public void OnMouseDown()=>
           openPopUpRoutine= StartCoroutine(LevelSelected()) ;     

        IEnumerator LevelSelected()
        {
            EventManager.Dispatch(ENUM_LevelSelectorEvent.LevelSelected);
            EventManager.Dispatch(ENUM_LevelSelectorEvent.Play);
            ps.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(LoadSceneAndExecuteScript());
        }

        IEnumerator LoadSceneAndExecuteScript()
        {
            // Load the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            // Wait until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            StartCoroutine(SetLevelData());          
        }

        IEnumerator SetLevelData()
        {
            DominoPooler pooler = FindObjectOfType<DominoPooler>();
            pooler.SetLevel(level);
            SceneManager.UnloadSceneAsync(levelSelectorScene);
            yield return null;
        }


        public void OnMouseUp()=>
            pregamePopUP?.EnablePlayButton();       
    }
}
