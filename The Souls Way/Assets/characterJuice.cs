using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles purely aesthetic things like particles, squash & stretch, and tilt

public class characterJuice : MonoBehaviour
{
    [Header("Components")]
    PlayerMovement moveScript;
    characterJump jumpScript;
    [SerializeField] Animator myAnimator;
    [SerializeField] GameObject characterSprite;

    [Header("Components - Particles")]
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;

    [Header("Components - Audio")]
    [SerializeField] AudioSource jumpSFX;
    [SerializeField] AudioSource landSFX;

    [Header("Calculations")]
    public float runningSpeed;
    public float maxSpeed;

    [Header("Current State")]
    public bool squeezing;
    public bool jumpSqueezing;
    public bool landSqueezing;
    public bool playerGrounded;



    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        jumpScript = GetComponent<characterJump>();
    }

    void Update()
    {
        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(moveScript.velocity.x), 0, maxSpeed);
      //  myAnimator.SetFloat("runSpeed", runningSpeed);

        checkForLanding();
    }

    private void checkForLanding()
    {
        if (!playerGrounded && jumpScript.onGround)
        {
            //By checking for this, and then immediately setting playerGrounded to true, we only run this code once when the player hits the ground 
            playerGrounded = true;
            

            //This is related to the "ignore jumps" option on the camera panel.
         //   jumpLine.characterY = transform.position.y;

            //Play an animation, some particles, and a sound effect when the player lands
          //  myAnimator.SetTrigger("Landed");
         //   landParticles.Play();

            if (!landSFX.isPlaying && landSFX.enabled)
            {
                landSFX.Play();
            }

         //   moveParticles.Play();
        }
        else if (playerGrounded && !jumpScript.onGround)
        {
            // Player has left the ground, so stop playing the running particles
            playerGrounded = false;
         //   moveParticles.Stop();
        }
    }
    public void jumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
       // myAnimator.ResetTrigger("Landed");
      //  myAnimator.SetTrigger("Jump");

        if (jumpSFX.enabled)
        {
            jumpSFX.Play();

        }
        //jumpParticles.Play();
    }
}