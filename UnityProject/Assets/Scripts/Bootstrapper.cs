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

    private void Awake()
    {
        //Assign our user to a cohort of Variant A, B, or C
        AssignCohort();
        
        //Binding the Local ConfigService
        ServiceLocator.Bind<IConfigService>(new LocalConfigService());

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
            Debug.Log($"Assigned new cohort: {AssignedCohort}");
        }
    }

    [ContextMenu("AssingNewCohort")]
    private void AssingNewCohort()
    {
        int randomIndex = Random.Range(0, 3);
        AssignedCohort = (Cohort)randomIndex;
        PlayerPrefs.SetString(_cohortKey, AssignedCohort.ToString());
    }
}
