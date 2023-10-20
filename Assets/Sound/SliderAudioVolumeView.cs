using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderAudioVolumeView : MonoBehaviour, IAudioVolumeView
{
    public event Action<float> VolumeChanged;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDestroy() =>
        slider.onValueChanged.RemoveListener(OnSliderChanged);

    public void SetVolumeWithoutNotify(float volume) =>
        slider.SetValueWithoutNotify(volume);

    private void OnSliderChanged(float value) =>
        VolumeChanged?.Invoke(value);
}
