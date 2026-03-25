using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Basic_Functions : MonoBehaviour
{
    private ScreenFade screenFade;
    private void Start()
    {
        screenFade = FindFirstObjectByType<ScreenFade>();
    }

    public virtual void OpenScene(string sceneName)
    {
        screenFade.FadeOut(() => SceneManager.LoadScene(sceneName));
    }

    public void Open(UI_Basic_Functions UI)
    {
        UI.gameObject.SetActive(true);
    }

    public void Close(UI_Basic_Functions UI)
    {
        UI.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
