public interface IBBContributor
{
    void ReadFromBB(PlayerBlackboard bb);
    void WriteToBB(PlayerBlackboard bb);
    bool CanWriteOnUpdate() => true;

    sealed void Register()
    {
        PlayerBlackboard.Instance.Register(this);
        ReadFromBB(PlayerBlackboard.Instance);
    }
    sealed void Unregister() => PlayerBlackboard.Instance.Unregister(this);
}