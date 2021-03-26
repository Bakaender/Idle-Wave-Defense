using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessWaveTD
{
	public class UIShowButtonPanel : MonoBehaviour
	{
        public string ButtonGroupName = "New Group";
        public GameObject PanelToShow;
        public bool StartActive = false;
        public bool CanCloseOnClick = false;
        public Color ActiveColor = Color.white;
        public Color NotActiveColor = Color.gray;

        private Button thisButton;

        private void Awake()
        {
            thisButton = gameObject.GetComponent<Button>();
            thisButton.onClick.AddListener(ButtonClicked);
            UIButtonHighlight.AddButtonAndPanelsToLists(thisButton, PanelToShow, ButtonGroupName, ActiveColor, NotActiveColor, StartActive, CanCloseOnClick);
        }

        private void ButtonClicked()
        {
            UIButtonHighlight.UpdateHighlightedButton(thisButton, ButtonGroupName);
            UIButtonHighlight.UpdateHighlightedButtonPanel(PanelToShow, ButtonGroupName);
        }
    }
}