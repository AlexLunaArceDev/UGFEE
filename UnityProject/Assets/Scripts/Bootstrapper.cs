using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    public enum Cohort
    {
        VariantA,
        VariantB,
        VariantC
    }
    public static Cohort AssignedCohort { get; private set; }
    private const string _cohortKey = "UserCohort";
    private const string _userIDKey = "UserID";
    public static string UserID { get; private set; }

    private void Awake()
    {
        //Simulation of assinging ID to the user
        AssingUserID();

        //Assign our user to a cohort of Variant A, B, or C
        AssignCohort();

        //Binding the Local ConfigService
        ServiceLocator.Bind<IConfigService>(new LocalConfigService());

        //Binding the Local LocalAnalyticsService
        ServiceLocator.Bind<IAnalyticsService>(new LocalAnalyticsService()); //Future TODO: integrate Mixpanel creating a Mixpanel Service that use the IAnalyticsService
    }

    private void AssingUserID()
    {
        if (PlayerPrefs.HasKey(_userIDKey))
        {
            UserID = PlayerPrefs.GetString(_userIDKey);
        }
        else
        {
            UserID = "UnityEditorUser01";
            PlayerPrefs.SetString(_userIDKey, UserID);
        }
        Debug.Log($"User ID: {UserID}");
    }

    private void AssignCohort()
    {
        if (PlayerPrefs.HasKey(_cohortKey))
        {
            AssignedCohort = (Cohort)System.Enum.Parse(typeof(Cohort), PlayerPrefs.GetString(_cohortKey));
            Debug.Log($"User was aleary assigned to cohort: {AssignedCohort}");
        }
        else
        {
            AssingNewCohort();
        }
    }

    [ContextMenu("AssingNewCohort")]
    private void AssingNewCohort()
    {
        int randomIndex = Random.Range(0, 3);
        if (PlayerPrefs.HasKey(_cohortKey))
        {
            Cohort previousCohort = (Cohort)System.Enum.Parse(typeof(Cohort), PlayerPrefs.GetString(_cohortKey));
            while((Cohort)randomIndex == previousCohort)
            {
                randomIndex = Random.Range(0, 3);
            }
        }
        AssignedCohort = (Cohort)randomIndex;
        PlayerPrefs.SetString(_cohortKey, AssignedCohort.ToString());
        Debug.Log($"Assigned new cohort: {AssignedCohort}");
    }
}
