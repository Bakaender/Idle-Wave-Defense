using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class FireExplosionScaler : MonoBehaviour
    {
        private float finalSize;
        private float totalTime;
        private float elapsedTime;

        private void Awake()
        {
            finalSize = MainReferences.fireTowerManager.ExplosionRange * 2;
            totalTime = MainReferences.fireTowerManager.ExplosionTime;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / totalTime;
            float scale = Mathf.Lerp(0.2f, finalSize, ratio);
            transform.localScale = new Vector2(scale, scale);

            if (elapsedTime >= totalTime)
            {
                //TODO pool/queue
                Destroy(gameObject);
            }
        }
    }
}
