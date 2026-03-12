using UnityEngine;

public class UI_Basic_Functions : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
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
