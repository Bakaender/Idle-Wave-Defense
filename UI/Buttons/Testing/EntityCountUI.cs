using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using TMPro;
using System.Linq;

namespace EndlessWaveTD
{
    public class EntityCountUI : MonoBehaviour
    {
        public TMP_Text EntityCountText;
        public float UpdateDelay;

        private float nextUpdateTime;

        private void Update()
        {
            nextUpdateTime -= Time.unscaledDeltaTime;

            if (nextUpdateTime <= 0)
            {
                EntityCountText.text = "Entity Count: " + World.DefaultGameObjectInjectionWorld.EntityManager.GetAllEntities().Count();
                nextUpdateTime = UpdateDelay;
            }
        }
    }
}
