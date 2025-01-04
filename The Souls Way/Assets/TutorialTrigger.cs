using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private bool used = false;
    public AudioClip Clip;
    public string subtitles;

    private TutorialManager tutorialManager;
    private void Awake()
    {
        tutorialManager = gameObject.GetComponentInParent<TutorialManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !used)
        {
            used = true;
            print("Tutorial Triggered");
            TriggerTutorial();
        }
    }
    private void TriggerTutorial()
    {
        tutorialManager.AddTutorialToQueue(Clip, subtitles);
    }
}
