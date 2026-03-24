using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    [Serializable]
    public class Btn
    {
        public Button btn;
        public Image enabledImg;
    }
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private List<Btn> sectionBtns;

        private void Awake()
        {
            foreach (Btn sectionBtn in sectionBtns)
            {
                sectionBtn.btn.onClick.AddListener(() => OpenSection(sectionBtns.IndexOf(sectionBtn)));
            }
            OpenSection(0);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

        public void OpenSection(int index)
        {
            foreach (Btn btn in sectionBtns)
            {
                Color newColor = btn.enabledImg.color;
                newColor.a = 0;
                btn.enabledImg.color = newColor;
            }
            Color color  = sectionBtns[index].enabledImg.color;
            color.a = .2f;
           sectionBtns[index].enabledImg.color = color;
        }
    }
}
