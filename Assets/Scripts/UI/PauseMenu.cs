using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : UI_Basic_Functions
{

    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject helpsPanel;
    private void OnEnable()
    {
        SetTimePause();
    }

    private void OnDisable()
    {
        SetTimeUnPause();
        settingPanel.SetActive(false);
        helpsPanel.SetActive(false);
    }

    public override void OpenScene(string sceneName)
    {
        SetTimeUnPause();
        base.OpenScene(sceneName);
    }

    public void SetTimePause()
    {
        Time.timeScale = 0;
    }

    public void SetTimeUnPause()
    {
        Time.timeScale = 1;
    }


    public void ResetBBVariables()
    {
        PlayerBlackboard.Instance.ResetAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
