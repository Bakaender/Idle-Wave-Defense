using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessWaveTD
{
    public struct ButtonHighlightHelper
    {
        public int MainIndex;
        public bool StartShown;
        public bool CloseOnClick;
        public float ActiveRed;
        public float ActiveBlue;
        public float ActiveGreen;
        public float ActiveAlpha;
        public float NotActiveRed;
        public float NotActiveBlue;
        public float NotActiveGreen;
        public float NotActiveAlpha;
    }

	public class UIButtonHighlight : MonoBehaviour
	{
        private static List<List<Button>> MainUIButtons = new List<List<Button>>();
        private static List<List<GameObject>> MainUIButtonsPanels = new List<List<GameObject>>();
        private static List<List<bool>> PanelsStartShown = new List<List<bool>>();
        private static List<List<bool>> ButtonsCloseOnClick = new List<List<bool>>();
        private static List<List<Color>> ButtonsActiveColor = new List<List<Color>>();
        private static List<List<Color>> ButtonsNotActiveColor = new List<List<Color>>();

        private static Dictionary<string, int> buttonGroupIndex = new Dictionary<string, int>();

        private static int GetButtonGroup(string groupName)
        {
            int groupIndex;
            if (buttonGroupIndex.TryGetValue(groupName, out groupIndex))
            {
                //Found group. groupIndex now contains its list index.
            }
            else
            {
                //Create list for new group, and add to dictionary with index.
                MainUIButtons.Add(new List<Button>());
                MainUIButtonsPanels.Add(new List<GameObject>());
                PanelsStartShown.Add(new List<bool>());
                ButtonsCloseOnClick.Add(new List<bool>());
                ButtonsActiveColor.Add(new List<Color>());
                ButtonsNotActiveColor.Add(new List<Color>());

                groupIndex = MainUIButtons.Count - 1;
                buttonGroupIndex.Add(groupName, groupIndex);
            }
            return groupIndex;
        }

        public static void AddButtonAndPanelsToLists(Button button, GameObject buttonsPanel, string groupName, Color activeColor, Color notActiveColor, bool startShown, bool closeOnClick)
        {
            int index = GetButtonGroup(groupName);
            MainUIButtons[index].Add(button);
            MainUIButtonsPanels[index].Add(buttonsPanel);
            PanelsStartShown[index].Add(startShown);
            ButtonsCloseOnClick[index].Add(closeOnClick);
            ButtonsActiveColor[index].Add(activeColor);
            ButtonsNotActiveColor[index].Add(notActiveColor);

            if (startShown)
            {
                button.image.color = activeColor;
            }
            else
            {
                buttonsPanel.SetActive(false);
                button.image.color = notActiveColor;
            }
        }

        public static void UpdateHighlightedButton(Button pressedButton, string groupName)
        {
            int index = GetButtonGroup(groupName);
            for (int i = 0; i < MainUIButtons[index].Count; i++)
            {
                if (pressedButton == MainUIButtons[index][i])
                {
                    if (ButtonsCloseOnClick[index][i] && MainUIButtonsPanels[index][i].gameObject.activeSelf) //Disable if already active
                        MainUIButtons[index][i].image.color = ButtonsNotActiveColor[index][i];
                    else
                        MainUIButtons[index][i].image.color = ButtonsActiveColor[index][i];
                }
                else
                {
                    MainUIButtons[index][i].image.color = ButtonsNotActiveColor[index][i];
                }
            }
        }

        public static void UpdateHighlightedButtonPanel(GameObject panel, string groupName)
        {
            int index = GetButtonGroup(groupName);
            for (int i = 0; i < MainUIButtonsPanels[index].Count; i++)
            {
                if (panel == MainUIButtonsPanels[index][i])
                {
                    if (ButtonsCloseOnClick[index][i] && MainUIButtonsPanels[index][i].gameObject.activeSelf) //Disable if already active
                        MainUIButtonsPanels[index][i].gameObject.SetActive(false);
                    else 
                        MainUIButtonsPanels[index][i].gameObject.SetActive(true);
                }
                else
                {
                    MainUIButtonsPanels[index][i].gameObject.SetActive(false);
                }
            }
        }
    }
}