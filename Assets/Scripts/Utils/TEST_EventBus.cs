using UnityEngine;

public class TEST_EventBus : MonoBehaviour
{
    [SerializeField] private Callback data;


    #region Context Menu 

    [ContextMenu("TestPublish")]
    public void TestPublish()
    {
        EventBus.Publish<Callback>(data);
    }
    [ContextMenu("TestListenerAdd")]
    public void TestAddListener()
    {
        EventBus.Subscribe<Callback>(OnCallbackReceived);
    }
    [ContextMenu("TestListenerRemove")]
    public void TestRemoveListener()
    {
        EventBus.Unsubscribe<Callback>(OnCallbackReceived);
    }
    #endregion
    

    private void OnCallbackReceived(Callback ctx)
    {
        print($"id is : {ctx.id} \n" +
              $"name is : {ctx.name} \n" +
              $"color is : {ctx.color}.");
    }
}

[System.Serializable]
public struct Callback
{
    public int id;
    public string name;
    public Color color;
}