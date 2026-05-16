namespace RenData.SaveLoad;
public class PostLoadableClass
{
    public PostLoadableClass()
    {
        IsPostLoadRegistered = false;
    }
    public virtual void OnPostLoad() { }
    public bool Is_Post_Load_Registered() => IsPostLoadRegistered;
    public void Set_Post_Load_Registered(bool onoff) { IsPostLoadRegistered = onoff; }
    private bool IsPostLoadRegistered;
}
