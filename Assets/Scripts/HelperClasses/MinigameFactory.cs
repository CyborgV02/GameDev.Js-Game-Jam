using UnityEngine;

public class MinigameFactory : MonoBehaviour
{
    [SerializeField] public GameObject nodeTransferPrefab;
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
                if (nodeTransferPrefab == null)
                {
                    Debug.LogError("NodeTransfer prefab is not assigned on MinigameFactory.");
                    return null;
                }

                GameObject minigame = Instantiate(nodeTransferPrefab);
                IMinigame minigameComponent = minigame.GetComponent<IMinigame>();
                if (minigameComponent == null)
                {
                    Debug.LogError("NodeTransfer prefab does not implement IMinigame.");
                    Destroy(minigame);
                    return null;
                }

                return minigameComponent;
            default:
                Debug.LogError($"Minigame '{minigameName}' not found!");
                return null;
        }
    }
}