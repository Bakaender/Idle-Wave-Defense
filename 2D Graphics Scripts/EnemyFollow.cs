using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace EndlessWaveTD
{
    public class EnemyFollow : MonoBehaviour
    {
        private static Color32 startColor = new Color32(0, 132, 5, 255);
        private Entity EnemyToFollow;
        private EnemyData EnemyToFollowData;

        private Color32 StartHpColor;
        private SpriteRenderer rend;

        public bool following;

        // Start is called before the first frame update
        void Awake()
        {
            rend = gameObject.GetComponent<SpriteRenderer>();
            //Debug.Log(rend.name);
            following = false;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (following)
            {
                if (GameDataManager.instance.Manager.Exists(EnemyToFollow))
                {
                    EnemyToFollowData = GameDataManager.instance.Manager.GetComponentData<EnemyData>(EnemyToFollow);
                    gameObject.transform.position = EnemyToFollowData.Position;
                    StartHpColor = Color.Lerp(startColor, Color.red, 1f - EnemyToFollowData.HpPercent);
                    rend.material.SetColor("_FillColor_Color_1", StartHpColor);
                    rend.material.SetFloat("_ThresholdSmooth_Value_1", 0.97f - EnemyToFollowData.HpPercent);
                }
                else
                {
                    gameObject.transform.position = new Vector3(100, 100, 100);
                    following = false;
                }
            }
        }

        public void NewEnemy(Entity enemy)
        {
            EnemyToFollow = enemy;

            if (rend != null)
            {
                //Reset health bar color.
                StartHpColor = new Color32(0, 132, 5, 255);
                rend.material.SetColor("_FillColor_Color_1", StartHpColor);

                //Reset health bar fill
                rend.material.SetFloat("_ThresholdSmooth_Value_1", -0.03f);
            }
            else
                Debug.LogError("No Renderer");

            following = true;
        }
    }
}