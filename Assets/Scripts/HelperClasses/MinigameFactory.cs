using UnityEngine;

public class MinigameFactory : MonoBehaviour
{
    [SerializeField] public GameObject nodeTransferPrefab;
    [SerializeField] public GameObject attackMinigamePrefab;
    [SerializeField] public GameObject secureLinksPrefab;
    [SerializeField] public GameObject dischargePrefab;
    [SerializeField] public GameObject hellTwistPrefab;
    public static MinigameFactory Instance;
    void Awake()
    {
        Instance = this;
    }
    public IMinigame CreateMinigame(string minigameName)
    {
        switch (minigameName)
        {
            case "NodeTransfer":
                return InstantiateMinigame(minigameName, nodeTransferPrefab);
            case "AttackMinigame":
                return InstantiateMinigame(minigameName, attackMinigamePrefab);
            case "SecureLinks":
                return InstantiateMinigame(minigameName, secureLinksPrefab);
            case "Discharge":
                return InstantiateMinigame(minigameName, dischargePrefab);
            case "HellTwist":
                return InstantiateMinigame(minigameName, hellTwistPrefab);
            default:
                Debug.LogError($"Minigame '{minigameName}' not found!");
                return null;
        }
    }

    IMinigame InstantiateMinigame(string minigameName, GameObject prefab = null)
    {
        if (prefab == null)
        {
            Debug.LogError($"Minigame '{minigameName}' prefab is not assigned on MinigameFactory.");
            return null;
        }

        GameObject minigame = Instantiate(prefab);
        IMinigame minigameComponent = minigame.GetComponent<IMinigame>();
        if (minigameComponent == null)
        {
            Debug.LogError($"Minigame '{minigameName}' prefab does not implement IMinigame.");
            Destroy(minigame);
            return null;
        }

        return minigameComponent;
    }
}
