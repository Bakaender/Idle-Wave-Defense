using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EndlessWaveTD
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("Use <br> for new line.")]
        public string TooltipText = "Please set this tooltip text";

        public void OnPointerEnter(PointerEventData eventData)
        {
            MainReferences.tooltipManager.SetTooltipText(TooltipText);
            MainReferences.tooltipManager.ActivateToolTip(eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MainReferences.tooltipManager.DisableTooltip();
        }
    }
}
