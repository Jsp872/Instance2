using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Basic_Functions : MonoBehaviour
{
    private ScreenFade _screenFade;
    private PlayerBlackboard bb;

    private const string mainMenuSceneName = "MainMenu";
    private PlayerBlackboard Blackboard
    {
        get
        {
            if (bb == null)
                bb = PlayerBlackboard.Instance;
            return bb;
        }
    }
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
        if (ScreenFade == null) return;
        if (SceneManager.GetActiveScene().name != mainMenuSceneName && Blackboard == null) return;

        if (sceneName == mainMenuSceneName)
        {
            Blackboard.ResetAll();
        }

        ScreenFade.FadeOut(() => SceneManager.LoadScene(sceneName));
    }

    public void Open(UI_Basic_Functions UI) => UI.gameObject.SetActive(true);
    public void Close(UI_Basic_Functions UI) => UI.gameObject.SetActive(false);
    public void Quit() => Application.Quit();
}