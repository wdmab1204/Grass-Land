﻿using UnityEngine;
using System.Collections.Generic;

namespace SimpleSpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField]
        private List<SpriteAnimation> spriteAnimations;

        [SerializeField]
        private bool playAutomatically = true;

        private SpriteAnimation DefaultAnimation
        {
            get { return spriteAnimations.Count > 0 ? spriteAnimations[0] : null; }
        }

        private SpriteAnimation CurrentAnimation
        {
            get { return spriteAnimationHelper.CurrentAnimation; }
        }

        public bool Playing
        {
            get { return state == SpriteAnimationState.Playing; }
        }

        public bool Paused
        {
            get { return state == SpriteAnimationState.Paused; }
        }

        private SpriteRenderer spriteRenderer;

        private SpriteAnimationHelper spriteAnimationHelper;

        private SpriteAnimationState state = SpriteAnimationState.Playing;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteAnimationHelper = new SpriteAnimationHelper();
        }

        private void Start()
        {
            if (playAutomatically)
            {
                Play(DefaultAnimation);
            }
        }

        private void LateUpdate()
        {
            if (Playing)
            {
                SpriteAnimationFrame currentFrame = spriteAnimationHelper.UpdateAnimation(Time.deltaTime);

                if (currentFrame != null)
                {
                    spriteRenderer.sprite = currentFrame.Sprite;
                }
            }
        }

        public void Play()
        {
            if (CurrentAnimation == null)
            {
                spriteAnimationHelper.ChangeAnimation(DefaultAnimation);
            }

            Play(CurrentAnimation);
        }

        public void Play(string name)
        {
            Play(GetAnimationByName(name));
        }

        public void Play(SpriteAnimation animation)
        {
            state = SpriteAnimationState.Playing;
            spriteAnimationHelper.ChangeAnimation(animation);
        }
 
        private SpriteAnimation GetAnimationByName(string name)
        {
            for (int i = 0; i < spriteAnimations.Count; i++)
            {
                if (spriteAnimations[i].Name == name)
                {
                    return spriteAnimations[i];
                }
            }

            return null;
        }
    }
}
