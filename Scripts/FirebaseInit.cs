using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseAuth Auth;
    public static FirebaseUser User;
    public static DatabaseReference DB;

    async void Awake()
    {
        var dependency = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependency == DependencyStatus.Available)
        {
            Auth = FirebaseAuth.DefaultInstance;
            DB = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase ready ✅");
        }
        else
        {
            Debug.LogError("Firebase init error: " + dependency);
        }
    }
}
