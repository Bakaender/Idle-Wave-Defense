using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class OptionsManager : MonoBehaviour
    {
        public bool ShowPhysicalProjectiles;
        public bool ShowPhysicalBouncers;

        public bool ShowLightningProjectiles;
        public bool ShowLightningChains;

        public bool ShowFireProjectiles;
        public bool ShowFireExplosions;

        public bool ShowPoisonProjectiles;
        public bool ShowPoisonExplosions;

        public bool ShowIceProjectiles;

        //Background Graphics
        public bool MoveClouds;
        public bool MoveLargeStars;
        public bool MoveStarBackground;
        public bool AnimatePlayer;
        [HideInInspector] public PlayerShieldAnim playerShieldAnimator;

        private void Awake()
        {
            MainReferences.optionsManager = this;
        }

        public void TogglePhysicalProjectiles()
        {
            ShowPhysicalProjectiles = !ShowPhysicalProjectiles;
        }

        public void TogglePhysicalBouncers()
        {
            ShowPhysicalBouncers = !ShowPhysicalBouncers;
        }

        public void ToggleLightningProjectiles()
        {
            ShowLightningProjectiles = !ShowLightningProjectiles;
        }

        public void ToggleLightningChains()
        {
            ShowLightningChains = !ShowLightningChains;
        }

        public void ToggleFireProjectiles()
        {
            ShowFireProjectiles = !ShowFireProjectiles;
        }

        public void ToggleFireExplosions()
        {
            ShowFireExplosions = !ShowFireExplosions;
        }

        public void TogglePoisonProjectiles()
        {
            ShowPoisonProjectiles = !ShowPoisonProjectiles;
        }

        public void TogglePoisonExplosions()
        {
            ShowPoisonExplosions = !ShowPoisonExplosions;
        }

        public void ToggleIceProjectiles()
        {
            ShowIceProjectiles = !ShowIceProjectiles;
        }

        public void ToggleMoveClouds()
        {
            MoveClouds = !MoveClouds;
        }

        public void ToggleMoveLargeStar()
        {
            MoveLargeStars = !MoveLargeStars;
        }

        public void ToggleMoveBackground()
        {
            MoveStarBackground = !MoveStarBackground;
        }

        public void ToggleAnimatePlayer()
        {
            AnimatePlayer = !AnimatePlayer;
            if (AnimatePlayer)
                playerShieldAnimator.ContinueAnimate();
            else
                playerShieldAnimator.PauseAnimate();
        }
    }
}
