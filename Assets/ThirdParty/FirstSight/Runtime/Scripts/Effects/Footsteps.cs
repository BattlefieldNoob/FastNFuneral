﻿using UnityEngine;
using FMODUnity;

#pragma warning disable CS0649

namespace WraithavenGames.FirstSight
{
    public class Footsteps : MonoBehaviour
    {
        [Header("Footstep Settings")]
        [SerializeField] private float walkingStride = 2.5f;
        [SerializeField] private float runningStride = 4.5f;
        [SerializeField] private float strideAdjustTime = 0.1f;
        [SerializeField, Range(0f, 2f)] private float minFootstepPitch = 0.75f;
        [SerializeField, Range(0f, 2f)] private float maxFootstepPitch = 1.25f;
        [SerializeField, Range(0f, 1f)] private float steroBalance = 0.5f;
        [SerializeField, Range(0f, 1f)] private float walkingVolume = 0.25f;
        [SerializeField, Range(0f, 1f)] private float runningVolume = 0.5f;

        [Header("Landing Settings")]
        [SerializeField, Range(0f, 2f)] private float minLandingPitch = 0.75f;
        [SerializeField, Range(0f, 2f)] private float maxLandingPitch = 1.25f;
        [SerializeField, Range(0f, 1f)] private float landingVolume = 1f;

        [Header("Jumping Settings")]
        [SerializeField, Range(0f, 2f)] private float minJumpingPitch = 1.25f;
        [SerializeField, Range(0f, 2f)] private float maxJumpingPitch = 1.75f;
        [SerializeField, Range(0f, 1f)] private float jumpingVolume = 0.65f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip[] landingSounds;
        [SerializeField] private AudioClip[] jumpingSounds;

        [Header("Dependencies")]
        [SerializeField] [EventRef] private string _stepSound;
        [SerializeField] [EventRef] private string _landingSound;
        [SerializeField] [EventRef] private string _jumpSound;

        [SerializeField] private PlayerController controller;

        private bool leftFootNext;
        private Vector3 lastPosition;
        private float sinceLastFootstep;
        private bool wasGrounded;
        private bool onLeftFoot;
        private float stride;
        private float lastLandingTime;
        private float lastJumpingTime;

        private void Start()
        {
            stride = walkingStride;
            wasGrounded = true;
            lastLandingTime = float.MinValue;
            lastJumpingTime = float.MinValue;
            lastPosition = transform.position;
        }

        private void Update()
        {
            float goalStride = controller.IsRunning ? runningStride : walkingStride;
            stride = Mathf.MoveTowards(stride, goalStride, Time.deltaTime / strideAdjustTime);

            if (controller.IsGrounded)
            {
                if (wasGrounded)
                {
                    Vector3 pos = transform.position;
                    Vector3 posDelta = pos - lastPosition;
                    posDelta.y = 0f;

                    sinceLastFootstep += posDelta.magnitude;
                    lastPosition = pos;

                    if (sinceLastFootstep >= stride)
                    {
                        sinceLastFootstep -= stride;
                        PlayFootstepSound();
                    }
                }
                else
                    PlayLandingSound();
                wasGrounded = true;
            }
            else if (controller.IsAboutToLand)
            {
                sinceLastFootstep = 0f;
                lastPosition = transform.position;

                PlayLandingSound();
            }
            else
            {
                if (wasGrounded && controller.IsCurrentlyJumping)
                    PlayJumpingSound();

                sinceLastFootstep = 0f;
                wasGrounded = false;
            }
        }

        private void PlayFootstepSound()
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            AudioClip sound = footstepSounds[randomIndex];

            onLeftFoot = !onLeftFoot;

            float volume = controller.IsRunning ? runningVolume : walkingVolume;

            RuntimeManager.PlayOneShotAttached(_stepSound, gameObject);
        }

        private void PlayLandingSound()
        {
            if (Time.time - lastLandingTime < 0.1f)
                return;

            lastLandingTime = Time.time;

            int randomIndex = Random.Range(0, landingSounds.Length);
            if(landingSounds==null || landingSounds.Length==0)
                return;
            AudioClip sound = landingSounds[randomIndex];

            //Aggiungere landing volume
            RuntimeManager.PlayOneShotAttached(_stepSound, gameObject);

        }

        private void PlayJumpingSound()
        {
            if (Time.time - lastJumpingTime < 0.1f)
                return;

            lastJumpingTime = Time.time;

            int randomIndex = Random.Range(0, jumpingSounds.Length);
            AudioClip sound = jumpingSounds[randomIndex];

            //Aggiungere jump volume
            RuntimeManager.PlayOneShotAttached(_jumpSound, gameObject);

        }
    }
}
