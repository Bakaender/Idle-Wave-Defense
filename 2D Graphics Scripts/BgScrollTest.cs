using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class BgScrollTest : MonoBehaviour
    {
        private Material material;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            if (MainReferences.optionsManager.MoveStarBackground)
            {
                material.mainTextureOffset += MainReferences.cloudManager.BackgroundMoveVelocity * Time.deltaTime;

                if (material.mainTextureOffset.x > 1)
                {
                    material.mainTextureOffset -= Vector2.right;
                }
                if (material.mainTextureOffset.y > 1)
                {
                    material.mainTextureOffset -= Vector2.up;
                }
            }
        }
    }
}