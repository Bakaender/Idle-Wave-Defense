using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class CloudMover : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            PickRandomCloud(spriteRenderer);
        }

        private void Update()
        {
            if (MainReferences.optionsManager.MoveClouds)
            {
                gameObject.transform.position += new Vector3(MainReferences.cloudManager.CloudMoveSpeed * Time.deltaTime, MainReferences.cloudManager.CloudMoveSpeed * Time.deltaTime, 0);

                if (gameObject.transform.position.x <= MainReferences.cloudManager.CloudResetDistance)
                {
                    gameObject.transform.position += new Vector3(MainReferences.cloudManager.CloudResetMoveDistance, MainReferences.cloudManager.CloudResetMoveDistance, 0);
                    PickRandomCloud(spriteRenderer);
                }
            }
        }

        private void PickRandomCloud(SpriteRenderer cloud)
        {
            cloud.sprite = MainReferences.cloudManager.AllClouds[Random.Range(0, MainReferences.cloudManager.AllClouds.Length)];
        }
    }
}