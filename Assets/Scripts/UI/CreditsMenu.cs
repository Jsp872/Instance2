using UnityEngine;

namespace UI
{
    public class CreditsMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup creditsPanel;
        public void Return()
        {
            creditsPanel.Toggle(false);
        }
    }
}