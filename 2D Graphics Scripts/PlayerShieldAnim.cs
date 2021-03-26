using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class PlayerShieldAnim : MonoBehaviour
    {
        Animator animatorController;

        private void Start()
        {
            animatorController = gameObject.GetComponent<Animator>();
            MainReferences.optionsManager.playerShieldAnimator = this;
        }

        public void ContinueAnimate()
        {
            animatorController.speed = 1f;
        }

        public void PauseAnimate()
        {
            animatorController.speed = 0f;
        }
    }
}
