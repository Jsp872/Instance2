using TMPro;
using UnityEngine;

namespace Lang
{
    public class TextKey : MonoBehaviour
    {
        [SerializeField] private string key;
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            Localization.OnLanguageChanged += UpdateText;
            UpdateText();
        }

        private void OnDestroy()
        {
            Localization.OnLanguageChanged -= UpdateText;
        }

        private void UpdateText()
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Localization key is null or empty on " + gameObject.name);
                return;
            }
            
            text.text = Localization.Read(key);
        }
    }
}