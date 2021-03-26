using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class RotateCenter : MonoBehaviour
    {
        public GameObject RotateObject;
        public float RotateSpeed = 1f;

        private void Update()
        {
            if (MainReferences.optionsManager.AnimatePlayer)
            {
                RotateObject.transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));
            }
        }
    }
}