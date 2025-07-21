using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //TODO: Assign our user to a cohort of Variant A, B, or C
        
        //TODO:
        //ServiceLocator.Bind<IConfigService>(new LocalConfigService());
        
        //TODO:
        //ServiceLocator.Bind<IAnalyticsService>(new LocalAnalyticsService());
    }

    private void Start()
    {
        //TODO: Show the appropriate UI based on our user's cohort
    }
}
