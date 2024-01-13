using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    [RequireComponent(typeof(Button))]
    public class SoundOnButtonClick : MonoBehaviour
    {
        [SerializeField] private string sound = "event:/SFX/UI/Click button";
        private Button button;
        private FMOD.Studio.EventInstance buttonSound;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayButtonClickSound);
            buttonSound = FMODUnity.RuntimeManager.CreateInstance(sound);
        }

        private void OnDestroy() =>
            button.onClick.RemoveListener(PlayButtonClickSound);

        public void PlayButtonClickSound() =>
            buttonSound.start();
    }
}
