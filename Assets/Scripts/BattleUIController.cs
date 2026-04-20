#nullable enable
using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum SelectionState
{
    None,
    Ally1,
    Ally2,
    Options
}


public class BattleUIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;
    static public Action<Character, Character?>? UpdateAllyUI;
    [Header("UI Elements Ally 1")]
    [SerializeField] private string allyContainer1 = "ally-container-1";
    [SerializeField] private string allyName1 = "ally-name";
    [SerializeField] private string allySprite1 = "ally-sprite";
    [SerializeField] private string allyHP1 = "ally-hp";
    [SerializeField] private string allyHealthBar1 = "hp-bar-fill";
    [SerializeField] private string[] actionButtonNames1 = { "btn-atk", "btn-hack", "btn-item", "btn-boom" };
    private VisualElement allyContainerElement1;
    private Label allyNameElement1;
    private Image allySpriteElement1;
    private Label allyHP1Element;
    private VisualElement allyHealthBarElement1;
    private Button[] actionButtons1;
    private int selectedActionIndex1 = 0;

    [Header("UI Elements Ally 2")]
    [SerializeField] private string allyContainer2 = "ally-container-2";
    [SerializeField] private string allyName2 = "ally-name-2";
    [SerializeField] private string allySprite2 = "ally-sprite-2";
    [SerializeField] private string allyHP2 = "ally-hp-2";
    [SerializeField] private string allyHealthBar2 = "hp-bar-fill-2";
    [SerializeField] private string[] actionButtonNames2 = { "btn-atk-2", "btn-hack-2", "btn-item-2", "btn-boom-2" };
    private VisualElement allyContainerElement2;
    private Label allyNameElement2;
    private Image allySpriteElement2;
    private Label allyHP2Element;
    private VisualElement allyHealthBarElement2;
    private Button[] actionButtons2;
    private int selectedActionIndex2 = 0;

    [Header("UI Elements Options")]
    [SerializeField] private string optionsContainer = "options-1";
    [SerializeField] private string optionsContainer2 = "options-2";
    [SerializeField] private string textBoxContainer = "text-box";
    [SerializeField] private string optionsFullContainer = "dialog-options";
    [SerializeField] private string[] optionButtonNames = { "dialog-option-1", "dialog-option-2", "dialog-option-3", "dialog-option-4", "dialog-option-5", "dialog-option-6" };
    [SerializeField] private string[] textBoxOptionNames = { "dialog-1", "dialog-2", "dialog-3" };
    private VisualElement optionsContainerElement;
    private VisualElement optionsContainerElement2;
    private VisualElement textBoxContainerElement;
    private VisualElement optionsFullContainerElement;
    private Label[] textBoxOptions;
    private Label[] optionButtons;
    private int selectedOptionIndex = 0;

    private SelectionState currentSelectionState = SelectionState.Ally1;
    [SerializeField] private bool hasAlly2 = true;

    void OnEnable()
    {
        // initalize input
        InputController.OnMove += HandleMoveInput;
        InputController.OnActionZ += HandleActionZInput;
        InputController.OnActionX += HandleActionXInput;

        // subscribe to ally UI update event
        UpdateAllyUI += UpdateAllyInfo;
        BattleController.OnBattleStart += InitializeBattleUI;
    }

    void OnDisable()
    {
        // uninitialize input
        InputController.OnMove -= HandleMoveInput;
        InputController.OnActionZ -= HandleActionZInput;
        InputController.OnActionX -= HandleActionXInput;

        // unsubscribe from ally UI update event
        UpdateAllyUI -= UpdateAllyInfo;
        BattleController.OnBattleStart -= InitializeBattleUI;
     }

    void Awake()
    {
        root = uiDocument.rootVisualElement;
        InitializeUI();
    }

    void InitializeUI() {
        // Initialize Ally 1 elements
        allyContainerElement1 = root.Q<VisualElement>(name: allyContainer1);
        allyNameElement1 = root.Q<Label>(name: allyName1);
        allySpriteElement1 = root.Q<Image>(name: allySprite1);
        allyHP1Element = root.Q<Label>(name: allyHP1);
        allyHealthBarElement1 = root.Q<VisualElement>(name: allyHealthBar1);
        actionButtons1 = new Button[actionButtonNames1.Length];
        for (int i = 0; i < actionButtonNames1.Length; i++)
        {
            actionButtons1[i] = root.Q<Button>(name: actionButtonNames1[i]);
        }

        // Initialize Ally 2 elements
        allyContainerElement2 = root.Q<VisualElement>(name: allyContainer2);
        allyNameElement2 = root.Q<Label>(name: allyName2);
        allySpriteElement2 = root.Q<Image>(name: allySprite2);
        allyHP2Element = root.Q<Label>(name: allyHP2);
        allyHealthBarElement2 = root.Q<VisualElement>(name: allyHealthBar2);
        actionButtons2 = new Button[actionButtonNames2.Length];
        for (int i = 0; i < actionButtonNames2.Length; i++)
        {
            actionButtons2[i] = root.Q<Button>(name: actionButtonNames2[i]);
        }
        if (hasAlly2)
        {
            allyContainerElement2.style.display = DisplayStyle.Flex;
        }

        // Initialize Options elements
        optionsContainerElement = root.Q<VisualElement>(name: optionsContainer);
        optionsContainerElement2 = root.Q<VisualElement>(name: optionsContainer2);
        textBoxContainerElement = root.Q<VisualElement>(name: textBoxContainer);
        optionsFullContainerElement = root.Q<VisualElement>(name: optionsFullContainer);
        optionButtons = new Label[optionButtonNames.Length];
        textBoxOptions = new Label[textBoxOptionNames.Length];

        for (int i = 0; i < optionButtonNames.Length; i++)
        {
            optionButtons[i] = root.Q<Label>(name: optionButtonNames[i]);
        }
        for (int i = 0; i < textBoxOptionNames.Length; i++)
        {
            textBoxOptions[i] = root.Q<Label>(name: textBoxOptionNames[i]);
        }
        Debug.Log("UI Elements Initialized");
        optionButtons[selectedOptionIndex].AddToClassList("dialog-option--selected");
        actionButtons1[selectedActionIndex1].AddToClassList("action-btn--selected");
        actionButtons2[selectedActionIndex2].AddToClassList("action-btn--selected");

        SetTextBoxDisplay(false);
        AdjustOptionsVisibility(false);
    }

    void InitializeBattleUI(BattleStartPayload payload)
    {
        if (payload.payloadType == BattleStartPayload.PayloadType.BattleData && payload.battle != null)
        {
            UpdateAllyInfo(payload.battle.player, payload.battle.allies.Length > 1 ? payload.battle.allies[1] : null);
            string[] initialText = payload.battle.battleText.SetText(new BattleTextPayload
            {
                currentEnemyNameIndex = 0,
                currentAllyNameIndex = 0,
                currentMoveText = 0,
                customText = ""
            });
            SetTextBoxText(initialText);
            SetTextBoxDisplay(true);
            AdjustOptionsVisibility(false);
        }
    }

    void HandleMoveInput(Vector2 input)
    {
        Debug.Log($"Move Input Received: {input}");
        if (currentSelectionState == SelectionState.Options)
        {
            int lastSelectedOptionIndex = selectedOptionIndex;

            if (input.x > 0.5f)
            {
                // Move right
                selectedOptionIndex = (selectedOptionIndex + 3) % optionButtons.Length;
            }
            else if (input.x < -0.5f)
            {
                // Move left
                selectedOptionIndex = (selectedOptionIndex - 3 + optionButtons.Length) % optionButtons.Length;
            }
            else if (input.y > 0.5f)
            {
                // Move up
                selectedOptionIndex = (selectedOptionIndex - 1 + optionButtons.Length) % optionButtons.Length;
            }
            else if (input.y < -0.5f)
            {
                // Move down
                selectedOptionIndex = (selectedOptionIndex + 1) % optionButtons.Length;
            }

            optionButtons[lastSelectedOptionIndex].RemoveFromClassList("dialog-option--selected");
            optionButtons[selectedOptionIndex].AddToClassList("dialog-option--selected");
        }
        else if (currentSelectionState == SelectionState.Ally1)
        {
            int lastSelectedActionIndex = selectedActionIndex1;

            if (input.x > 0.5f)
            {
                // Move right
                selectedActionIndex1 = (selectedActionIndex1 + 1) % actionButtons1.Length;
            }
            else if (input.x < -0.5f)
            {
                // Move left
                selectedActionIndex1 = (selectedActionIndex1 - 1 + actionButtons1.Length) % actionButtons1.Length;
            }

            actionButtons1[lastSelectedActionIndex].RemoveFromClassList("action-btn--selected");
            actionButtons1[selectedActionIndex1].AddToClassList("action-btn--selected");
        }
        else if (currentSelectionState == SelectionState.Ally2)
        {
            int lastSelectedActionIndex = selectedActionIndex2;

            if (input.x > 0.5f)
            {
                // Move right
                selectedActionIndex2 = (selectedActionIndex2 + 1) % actionButtons2.Length;
            }
            else if (input.x < -0.5f)
            {
                // Move left
                selectedActionIndex2 = (selectedActionIndex2 - 1 + actionButtons2.Length) % actionButtons2.Length;
            }

            actionButtons2[lastSelectedActionIndex].RemoveFromClassList("action-btn--selected");
            actionButtons2[selectedActionIndex2].AddToClassList("action-btn--selected");
        }
    }

    void HandleActionZInput()
    {
        if (currentSelectionState == SelectionState.Options)
        {
            Debug.Log($"Selected Option: {optionButtons[selectedOptionIndex].name}");
            // TEMP: Just cycle back to Ally 1 for now, eventually this will trigger the selected option's effect and then move to the next state as needed
            currentSelectionState = SelectionState.Ally1; // Go into combat or whatever after selecting an option
            AdjustOptionsVisibility(false);
        }
        else if (currentSelectionState == SelectionState.Ally1)
        {
            Debug.Log($"Selected Action for Ally 1: {actionButtons1[selectedActionIndex1].name}");
            currentSelectionState = hasAlly2 ? SelectionState.Ally2 : SelectionState.Options; // Switch to Ally 2 after selecting action for Ally 1
            AdjustOptionsVisibility(false);
        }
        else if (currentSelectionState == SelectionState.Ally2)
        {
            Debug.Log($"Selected Action for Ally 2: {actionButtons2[selectedActionIndex2].name}");
            currentSelectionState = SelectionState.Options; // Switch to Options after selecting action for Ally 2
            AdjustOptionsVisibility(true);
        }
    }

    void HandleActionXInput()
    {
        if (currentSelectionState == SelectionState.Options)
        {
            currentSelectionState = SelectionState.Ally1;
            AdjustOptionsVisibility(false);
        }
        else if (currentSelectionState == SelectionState.Ally2)
        {
            currentSelectionState = SelectionState.Ally1; // Go back to Ally 1 selection
            AdjustOptionsVisibility(false);
        }
    }

    void AdjustOptionsVisibility(bool Visible = true)
    {
        //optionsFullContainerElement.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
        optionsContainerElement.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
        optionsContainerElement2.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
    }

    void SetTextBoxDisplay(bool options = false) {
        if (options) {
            optionsContainerElement.style.display = DisplayStyle.Flex;
            textBoxContainerElement.style.display = DisplayStyle.None;
        } else {
            optionsContainerElement.style.display = DisplayStyle.None;
            textBoxContainerElement.style.display = DisplayStyle.Flex;
        }
    }

    void SetTextBoxText(string[] textLines) {
        for (int i = 0; i < textBoxOptions.Length; i++) {
            if (i < textLines.Length) {
                textBoxOptions[i].text = textLines[i];
                textBoxOptions[i].style.display = DisplayStyle.Flex;
            } else {
                textBoxOptions[i].style.display = DisplayStyle.None;
            }
        }
    }

    void UpdateAllyInfo(Character ally1, Character? ally2)
    {
        // Update Ally 1 UI
        allyNameElement1.text = ally1.Name;
        allySpriteElement1.sprite = ally1.Sprite[0]; // Assuming Character has a Sprite property
        float hpPercent1 = (float)ally1.CurrentHp / ally1.Hp;
        allyHP1Element.text = $"{ally1.CurrentHp}/{ally1.Hp}";
        allyHealthBarElement1.style.width = Length.Percent(hpPercent1 * 100);

        // Update Ally 2 UI if ally2 is not null
        if (ally2 != null)
        {
            allyNameElement2.text = ally2.Name;
            allySpriteElement2.sprite = ally2.Sprite[0]; // Assuming Character has a Sprite property
            float hpPercent2 = (float)ally2.CurrentHp / ally2.Hp; // Assuming max HP is 100
            allyHP2Element.text = $"{ally2.CurrentHp}/{ally2.Hp}";
            allyHealthBarElement2.style.width = Length.Percent(hpPercent2 * 100);
            allyContainerElement2.style.display = DisplayStyle.Flex;
        }
    }

}
