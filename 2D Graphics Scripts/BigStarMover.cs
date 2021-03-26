using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class BigStarMover : MonoBehaviour
    {
        private void Update()
        {
            if (MainReferences.optionsManager.MoveLargeStars)
            {
                gameObject.transform.position += MainReferences.starSpawner.BigStarMoveSpeed * Time.deltaTime;
                if (gameObject.transform.position.x <= MainReferences.starSpawner.BigStartDestroyDistance)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}