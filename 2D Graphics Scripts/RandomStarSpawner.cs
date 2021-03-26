using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class RandomStarSpawner : MonoBehaviour
    {
        public GameObject[] BigStarPrefabs = new GameObject[1];

        public float MinAngleSpawn = 25f;
        public float MaxAngleSpawn = 65f;
        public float RadiusSpawnFromCenter = 20.48f;

        public float RandomStarMinSpawnTime = 5f;
        public float RandomStarMaxSpawnTime = 10f;
        public Vector3 BigStarMoveSpeed = new Vector3(-0.5f, -0.5f, 0);
        public float BigStartDestroyDistance = -15f;

        private float nextSpawnTime;

        private void Awake()
        {
            MainReferences.starSpawner = this;

            nextSpawnTime = Random.Range(RandomStarMinSpawnTime, RandomStarMaxSpawnTime);
        }

        private void Update()
        {
            if (MainReferences.optionsManager.MoveLargeStars)
            {
                nextSpawnTime -= Time.deltaTime;

                if (nextSpawnTime <= 0)
                {
                    SpawnStar();
                    nextSpawnTime = Random.Range(RandomStarMinSpawnTime, RandomStarMaxSpawnTime);
                }
            }
        }

        private void SpawnStar()
        {
            Instantiate(BigStarPrefabs[Random.Range(0, BigStarPrefabs.Length)], MathFormulas.RandomCircle(Vector3.zero, RadiusSpawnFromCenter, Random.Range(MinAngleSpawn, MaxAngleSpawn)), Quaternion.identity);
        }
    }
}