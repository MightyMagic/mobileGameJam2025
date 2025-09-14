using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePhaseManager : MonoBehaviour
{
    public static UpgradePhaseManager Instance;

   private void Awake()
   {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
   }
}
