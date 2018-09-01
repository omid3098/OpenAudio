using OpenAudio;
using UnityEngine;

public class AudioSampleUsage : MonoBehaviour
{
    private AudioManager<SampleAudioType> audioManager;

    private void Awake()
    {
        audioManager = new AudioManager<SampleAudioType>("audiofiles");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q pressed");
            audioManager.Play(SampleAudioType.sample_audio_01, true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W pressed");
            audioManager.Play(SampleAudioType.sample_audio_02);
        }
    }
}
