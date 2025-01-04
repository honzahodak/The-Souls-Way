using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private AudioSource tutorialAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    private List<AudioClip> tutorialClips = new List<AudioClip>();
    private List<string> subtitlesText = new List<string>();
    [SerializeField] private TMP_Text subtitles;
    void Awake()
    {
        tutorialAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!tutorialAudioSource.isPlaying && tutorialClips.Count > 0)
        {
            PlayTutorial(tutorialClips[0], subtitlesText[0]);
            tutorialClips.RemoveAt(0);
            subtitlesText.RemoveAt(0);
        }
        if (!tutorialAudioSource.isPlaying)
        {
            musicAudioSource.volume = 0.15f;
            subtitles.text = "";
        }
    }

    public void PlayTutorial(AudioClip clip, string subtitlesText)
    {
        tutorialAudioSource.clip = clip;
        tutorialAudioSource.Play();
        musicAudioSource.volume = 0.05f;
        subtitles.text = subtitlesText;
    }
    public void AddTutorialToQueue(AudioClip clip, string subtitles)
    {
        tutorialClips.Add(clip);
        subtitlesText.Add(subtitles);
    }
}
