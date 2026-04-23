using UnityEngine;

public class MinigameFactory
{
    private GameObject nodeTransferPrefab;
    public static IMinigame CreateMinigame(string minigameName)
    {
        switch (minigameName)
        {
            case "NodeTransfer":
                
                return new NodeTransfer();
            // Add more cases here for additional minigames
            default:
                Debug.LogError($"Minigame '{minigameName}' not found!");
                return null;
        }
    }
}