using UnityEngine;

public class UpgradeButtonLinker : MonoBehaviour
{
    public int upgradeIndex = 0; // Set this to 0 in the Inspector for card 1
                                 // Set to 1 for card 2
                                 // Set to 2 for card 3

    private UnityEngine.UI.Button button;

    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            // This finds the one-and-only static Instance and tells this
            // button to call its SelectUpgrade function, passing in our index.
            button.onClick.AddListener(() => UpgradeManager.Instance.SelectUpgrade(upgradeIndex));
        }
    }
}
