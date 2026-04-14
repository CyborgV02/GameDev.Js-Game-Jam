using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;

    void Awake()
    {
        root = uiDocument.rootVisualElement;
        // Example: Set up health bar colors
        var playerHealthBar = root.Q<ProgressBar>(className: "Bar");
        SetProgressBarColor(playerHealthBar, Color.green);
    }

    void SetProgressBarColor(ProgressBar bar, Color color)
    {
        try
        {
            var fill = bar.Q<VisualElement>(className: "unity-progress-bar__progress");
            fill.style.backgroundColor = color;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to set progress bar color: " + e.Message);
        }
    }

    /* Test loop to simulate health changes
    async void Loop()
    {
        while (true)
        {
            var playerHealthBar = root.Q<ProgressBar>(className: "Bar");
            playerHealthBar.value = UnityEngine.Random.Range(0f, 100f); // Simulate health changes
            await Task.Delay(1000); // Update every second
        }
    } */


}
