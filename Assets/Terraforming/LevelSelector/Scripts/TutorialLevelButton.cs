using LevelSelector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelButton : LevelButton
{
    protected override string sceneToLoad => "TutorialSOS";


    protected override IEnumerator SetLevelData()
    {
        DominoPooler pooler = FindObjectOfType<TutorialDominoPooler>();
        pooler.SetLevel(level);
        SceneManager.UnloadSceneAsync(levelSelectorScene);
        yield return null;
    }

    public override void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
