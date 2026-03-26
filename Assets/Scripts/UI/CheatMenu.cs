using TMPro;
using UnityEngine;

public class CheatMenu : UI_Basic_Functions
{
    [SerializeField] private GameObject cheatPanel;

    [Header("Enable Visual Helper")]
    private bool enableHelper;
    [SerializeField] private TextMeshProUGUI enableTextMesh1;
    [SerializeField] private TextMeshProUGUI enableTextMesh2;

    [Header("Change Note Order")]
    private bool isInitialNoteOrder;
    [SerializeField] private TextMeshProUGUI changeNotetext1;
    [SerializeField] private TextMeshProUGUI changeNotetext2;

    public void Start()
    {
        print("Initialize cheat menu");
        enableHelper = PlayerBlackboard.Instance.enableVisualHelper;
        isInitialNoteOrder = PlayerBlackboard.Instance.isInitialNoteOrder;
        EnableHelperApplyVisualState();
        ChangeNoteApplyVisualState();
    }

    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }

    public void ActiveVisibleNoteHelper()
    {
        enableHelper = !enableHelper;

        PlayerBlackboard.Instance.enableVisualHelper = enableHelper;

        EnableHelperApplyVisualState();
    }

    public void ChangeNoteOrder()
    {
        isInitialNoteOrder = !isInitialNoteOrder;
        PlayerBlackboard.Instance.isInitialNoteOrder = isInitialNoteOrder;
        EventBus.Publish(new OnChangeNoteOrder());
        ChangeNoteApplyVisualState();
    }

    private void ChangeNoteApplyVisualState()
    {
        changeNotetext1.enabled = !isInitialNoteOrder;
        changeNotetext2.enabled = isInitialNoteOrder;
    }
    private void EnableHelperApplyVisualState()
    {
        EventBus.Publish(new ActiveVisibleNoteHelper(enableHelper));

        enableTextMesh1.enabled = !enableHelper;
        enableTextMesh2.enabled = enableHelper;
    }
}

public struct ActiveVisibleNoteHelper
{
    public bool isActive;

    public ActiveVisibleNoteHelper(bool value)
    {
        isActive = value;
    }
}
public struct OnChangeNoteOrder { }

