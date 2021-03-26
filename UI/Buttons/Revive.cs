using UnityEngine;
using UnityEngine.UI;

namespace EndlessWaveTD
{
    public class Revive : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(MainReferences.waveSpawner.Revive);
        }
    }
}
