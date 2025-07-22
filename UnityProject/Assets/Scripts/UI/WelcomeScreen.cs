using UnityEngine;

public class WelcomeScreen : UIScreenBase
{
    [SerializeField] private GameObject _nextUI;
    
    public override void Next()
    {
        Instantiate(_nextUI, transform.parent);
        DestroyImmediate(gameObject);
    }
}
