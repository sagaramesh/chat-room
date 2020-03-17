using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceInput : MonoBehaviour {

    // Mesh with blendshapes:

    // [SerializeField]
    // private GameObject puppet;

    // Sprite Mouth:

    [SerializeField]
    private GameObject mouthMask, mouthOutline;
    private float mouthSize;
    [SerializeField]
    private int micNumber = 0;


    // Capture realtime audio:

    AudioSource audioSource;
    AudioClip audioClip;
    public bool useMic = true;
    string selectedMic;
    [Range (0, 1000)]
    public int micSensitivity = 650;
    private float level;
    public AudioMixerGroup micMixerGroup, masterMixerGroup;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        if (useMic)
        {
            if (Microphone.devices.Length > 0)
            {
                // Choose correct mic input:
                selectedMic = Microphone.devices[micNumber].ToString();
                // To take mic input without echo: 
                audioSource.outputAudioMixerGroup = micMixerGroup;
                audioSource.clip = Microphone.Start(selectedMic, true, 10, AudioSettings.outputSampleRate);
            }
            else {
                useMic = false;
            }
        }
        else {
            audioSource.outputAudioMixerGroup = masterMixerGroup;
            audioSource.clip = audioClip;
        }
    }
	
	void Update () {

        // Get mic volume:

        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
        audioSource.clip.GetData(waveData, micPosition);

        // Peak of the last 128 samples:

        float levelMax = 0;
        for (int i = 0; i < dec; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        level = Mathf.Sqrt(Mathf.Sqrt(levelMax));

        // Function for current implementation (blendshape / sprite mask):

        MicToSpriteMask();
    }

    void MicToBlendshape() {
        // For blendshape: set blendshape value (0-100) according to level and sensitivity:
        //puppet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.RoundToInt(level * micSensitivity));
    }

    void MicToSpriteMask() {
        // Use the current voice volume (a value between 0 - 1) to calculate the target mouth size (between 0.05 and 1.0)
        float targetMouthSize = Mathf.Lerp(0.05f, 1.0f, (level * micSensitivity * 0.1f));

        // Animate the mouth size towards the target mouth size to keep the open / close animation smooth
        mouthSize = Mathf.Lerp(mouthSize, targetMouthSize, 30.0f * Time.deltaTime);

        // Apply the mouth size to the scale of the mouth geometry
        Vector3 localScale = mouthMask.transform.localScale;
        localScale.y = mouthSize;
        mouthMask.transform.localScale = localScale;
        if (mouthSize <= 0.127f) {
            mouthOutline.transform.localScale = localScale;
        }
       
    }
}
