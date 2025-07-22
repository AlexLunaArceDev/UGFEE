using UnityEngine;

public abstract class UIScreenBase : MonoBehaviour
{
    public string ScreenName => name;
    public abstract void Next();
}
