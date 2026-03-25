using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Basic_Functions : MonoBehaviour
{
    private ScreenFade _screenFade;

    private ScreenFade ScreenFade
    {
        get
        {
            if (_screenFade == null)
                _screenFade = FindFirstObjectByType<ScreenFade>();
            return _screenFade;
        }
    }

    public virtual void OpenScene(string sceneName)
    {
        if (ScreenFade == null)
        {
            Debug.LogError("ScreenFade introuvable dans la scène !", this);
            return;
        }

        ScreenFade.FadeOut(() => SceneManager.LoadScene(sceneName));
    }

    public void Open(UI_Basic_Functions UI) => UI.gameObject.SetActive(true);
    public void Close(UI_Basic_Functions UI) => UI.gameObject.SetActive(false);
    public void Quit() => Application.Quit();
}