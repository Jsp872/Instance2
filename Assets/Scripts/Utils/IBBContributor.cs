using UnityEngine;

public interface IBBContributor
{
    void ReadFromBB(PlayerBlackboard bb);
    void WriteToBB(PlayerBlackboard bb);
    bool CanWriteOnUpdate() => true;

    sealed void Register()
    {
        PlayerBlackboard bb = PlayerBlackboard.Instance;

        if (bb is null)
            return;

        bb.Register(this);
        ReadFromBB(bb);
    }
    sealed void Unregister() => PlayerBlackboard.Instance.Unregister(this);
}