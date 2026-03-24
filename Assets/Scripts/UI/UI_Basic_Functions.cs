using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Basic_Functions : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
