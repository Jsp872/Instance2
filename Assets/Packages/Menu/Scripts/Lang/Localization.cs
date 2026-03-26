using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Lang
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        public static event Action OnLanguageChanged;

        private const string languageFolderPath = "Lang";
        private static Dictionary<string, string> texts = new();
        private readonly Dictionary<string, string> languageNames = new()
        {
            {"fr", "Français"},
            {"en", "English"},
        };

        private void Awake()
        {
            LoadLanguage(PlayerPrefs.GetInt("Language", 0));

            if (dropdown)
            {
                dropdown.ClearOptions();
                List<string> options = languageNames.Select(code => code.Value).ToList();
                dropdown.AddOptions(options);
                dropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("Language", 0));
                dropdown.onValueChanged.AddListener(ChangeLanguage);
            }
        }

        private void ChangeLanguage(int index)
        {
            if (index < 0 || index >= languageNames.Count)
            {
                Debug.LogError("Invalid language index");
                return;
            }

            LoadLanguage(index);

            PlayerPrefs.SetInt("Language", index);
            OnLanguageChanged?.Invoke();
        }

        private void LoadLanguage(int index)
        {
            string languageCode = languageNames.ElementAt(index).Key;
            string fullPath = $"{languageFolderPath}/{languageCode}";

            TextAsset textAsset = Resources.Load<TextAsset>(fullPath);

            if (textAsset == null)
            {
                Debug.LogError($"Language file not found in Resources at path: {fullPath}");
                return;
            }

            texts = ConvertJsonToDictionary(textAsset.text);

            Resources.UnloadAsset(textAsset);
        }

        public static string Read(string key)
        {
            return texts.TryGetValue(key, out string value) ? value : $"[MISSING:{key}]";
        }

        private static Dictionary<string, string> ConvertJsonToDictionary(string json)
        {
            Dictionary<string, string> textDict = new();
            json = json.Trim().TrimStart('{').TrimEnd('}');
            string[] pairs = json.Split(',');

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().Trim('"');
                    string value = keyValue[1].Trim().Trim('"');
                    textDict[key] = value;
                }
            }
            return textDict;
        }
    }
}