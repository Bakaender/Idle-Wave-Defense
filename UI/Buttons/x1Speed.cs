using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessWaveTD
{
    public class x1Speed : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(MainReferences.uIButtonManager.NormalGameSpeed);
        }
    }
}
