using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EndlessWaveTD
{
    public class TooltipManager : MonoBehaviour
    {
        public GameObject TooltipObject;
        public float HeightPerLine = 24f;
        public float HeightSpacing = 20f;

        private RectTransform tooltipTransform;
        private Vector3 tooltipOffset;
        private TMP_Text tooltipText;
        private bool tooltipActive;

        private void Awake()
        {
            MainReferences.tooltipManager = this;

            tooltipText = TooltipObject.GetComponentInChildren<TMP_Text>();
            tooltipTransform = TooltipObject.GetComponent<RectTransform>();
            TooltipObject.SetActive(false);
            tooltipActive = false;
        }

        public void SetTooltipText(string text)
        {
            //Figure out how many lines of text to set height.
            string[] lines = text.Split(new string[] { "<br>" }, System.StringSplitOptions.None);
            tooltipTransform.sizeDelta = new Vector2(tooltipTransform.sizeDelta.x, HeightPerLine * lines.Length + HeightSpacing);
            text.Replace("<br>", "\n");
            tooltipText.SetText(text);
        }

        public void ActivateToolTip(Vector2 pos)
        {
            TooltipObject.SetActive(true);
            tooltipActive = true;
        }

        public void DisableTooltip()
        {
            tooltipActive = false;
            TooltipObject.SetActive(false);
        }

        private void Update()
        {
            if (tooltipActive)
            {
                if (Input.mousePosition.x < Screen.width / 2) //Left Half
                {
                    if (Input.mousePosition.y > Screen.height / 2) //Top Half
                    {
                        tooltipTransform.pivot = Vector2.up;
                        tooltipOffset = new Vector3(10, -10, 0);
                    }
                    else //Bottom Half
                    {
                        tooltipTransform.pivot = Vector2.zero;
                        tooltipOffset = new Vector3(3, 3, 0);
                    }
                }
                else //Right Half
                {
                    if (Input.mousePosition.y > Screen.height / 2) //Top Half
                    {
                        tooltipTransform.pivot = Vector2.one;
                        tooltipOffset = new Vector3(-3, -3, 0);
                    }
                    else //Bottom Half
                    {
                        tooltipTransform.pivot = Vector2.right;
                        tooltipOffset = new Vector3(-3, 3, 0);
                    }
                }

                tooltipTransform.position = Input.mousePosition + tooltipOffset;
            }
        }
    }
}
