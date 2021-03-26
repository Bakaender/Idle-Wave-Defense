using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessWaveTD
{
    public class x2Speed : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(MainReferences.uIButtonManager.Times2GameSpeed);
        }
    }
}
