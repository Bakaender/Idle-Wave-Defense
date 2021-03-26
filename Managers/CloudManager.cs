using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class CloudManager : MonoBehaviour
    {
        public Vector2 BackgroundMoveVelocity;

        public Sprite[] AllClouds = new Sprite[5];

        public float CloudMoveSpeed = -1f;
        public float CloudResetDistance = -20.48f;
        public float CloudResetMoveDistance = 40.96f;

        private void Awake()
        {
            MainReferences.cloudManager = this;
        }
    }
}