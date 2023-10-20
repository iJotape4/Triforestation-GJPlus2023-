using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ButtonAudioVolumeView : MonoBehaviour, IAudioVolumeView
{
    public event Action<float> VolumeChanged;

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite mutedSprite;

    private Button button;
    private Image buttonImage;
    AudioVolumeController audioVolumeController;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(OnClicked);
        audioVolumeController = GetComponentInParent<AudioVolumeController>();
    }

    private void OnDestroy() =>
        button.onClick.RemoveListener(OnClicked);

    public void SetVolumeWithoutNotify(float volume) =>
        buttonImage.sprite = volume > 0 ? activeSprite : mutedSprite;

    private void OnClicked()
    {
        bool muted = buttonImage.sprite == mutedSprite;
        VolumeChanged?.Invoke(muted ? audioVolumeController.previousVolume : 0);
    }
}
