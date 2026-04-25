#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SelectionState
{
    None,
    Ally1,
    Ally2,
    Options,
    Confirm
}

public enum SelectionMenuContext
{
    None,
    AttackTarget,
    HackAbility,
    HackTargetEnemy,
    HackTargetAlly,
    ItemSelect,
    ItemTargetAlly,
    BoomTarget
}


public class BattleUIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument = null!;
    [SerializeField] private GameObject battleSquare = null!;
    [SerializeField] private Camera battleCamera = null!;
    private VisualElement root = null!;
    private bool isUIInitialized;
    static public Action<Character, Character?>? UpdateAllyUI;

    [Header("UI Elements Ally 1")]
    [SerializeField] private string allyContainer1 = "ally-container-1";
    [SerializeField] private string allyName1 = "ally-name";
    [SerializeField] private string allySprite1 = "ally-sprite";
    [SerializeField] private string allyHP1 = "ally-hp";
    [SerializeField] private string allyHealthBar1 = "hp-bar-fill";
    [SerializeField] private string[] actionButtonNames1 = { "btn-atk", "btn-hack", "btn-item", "btn-boom" };
    private VisualElement allyContainerElement1 = null!;
    private Label allyNameElement1 = null!;
    private VisualElement allySpriteElement1 = null!;
    private Label allyHP1Element = null!;
    private VisualElement allyHealthBarElement1 = null!;
    private Button[] actionButtons1 = null!;
    private int selectedActionIndex1 = 0;

    [Header("UI Elements Ally 2")]
    [SerializeField] private string allyContainer2 = "ally-container-2";
    [SerializeField] private string allyName2 = "ally-name-2";
    [SerializeField] private string allySprite2 = "ally-sprite-2";
    [SerializeField] private string allyHP2 = "ally-hp-2";
    [SerializeField] private string allyHealthBar2 = "hp-bar-fill-2";
    [SerializeField] private string[] actionButtonNames2 = { "btn-atk-2", "btn-hack-2", "btn-item-2", "btn-boom-2" };
    private VisualElement allyContainerElement2 = null!;
    private Label allyNameElement2 = null!;
    private VisualElement allySpriteElement2 = null!;
    private Label allyHP2Element = null!;
    private VisualElement allyHealthBarElement2 = null!;
    private Button[] actionButtons2 = null!;
    private int selectedActionIndex2 = 0;

    [Header("UI Elements Options")]
    [SerializeField] private string optionsContainer = "options-1";
    [SerializeField] private string optionsContainer2 = "options-2";
    [SerializeField] private string textBoxContainer = "text-box";
    [SerializeField] private string optionsFullContainer = "dialog-options";
    [SerializeField] private string[] optionButtonNames = { "dialog-option-1", "dialog-option-2", "dialog-option-3", "dialog-option-4", "dialog-option-5", "dialog-option-6" };
    [SerializeField] private string[] textBoxOptionNames = { "dialog-1", "dialog-2", "dialog-3" };
    private VisualElement optionsContainerElement = null!;
    private VisualElement optionsContainerElement2 = null!;
    private VisualElement textBoxContainerElement = null!;
    // private VisualElement optionsFullContainerElement;
    private Label[] textBoxOptions = null!;
    private Label[] optionButtons = null!;
    private int selectedOptionIndex = 0;
    private int visibleOptionCount = 0;

    private SelectionState currentSelectionState = SelectionState.Ally1;
    private SelectionMenuContext currentMenuContext = SelectionMenuContext.None;
    private BattleSelectionBatch currentSelectionBatch = new BattleSelectionBatch();
    private BattleActionSelection? activeSelection;
    private Character[]? currentAllies;
    private Enemy[]? currentEnemies;
    private int currentActorIndex = 0;

    [SerializeField] private bool hasAlly2 = false;

    void OnEnable()
    {
        // initalize input
        InputController.OnMove += HandleMoveInput;
        InputController.OnActionZ += HandleActionZInput;
        InputController.OnActionX += HandleActionXInput;
        BattleController.OnAllyDamage += HandleAllyDamaged;
        BattleController.OnEnemyDamage += HandleEnemyDamaged;
    }

    void OnDisable()
    {
        // uninitialize input
        InputController.OnMove -= HandleMoveInput;
        InputController.OnActionZ -= HandleActionZInput;
        InputController.OnActionX -= HandleActionXInput;
        BattleController.OnAllyDamage -= HandleAllyDamaged;
        BattleController.OnEnemyDamage -= HandleEnemyDamaged;
    }

    void OnDestroy()
    {
        UpdateAllyUI -= UpdateAllyInfo;
        BattleController.OnBattleStart -= InitializeBattleUI;
        BattleController.OnBattleEnd -= HandleBattleEnd;
        BattleController.OnMinigameStarted -= HandleMinigameStarted;
        BattleController.OnMinigameEnded -= HandleMinigameEnded;
    }

    void Awake()
    {
        TryInitializeUI();
        // subscribe to ally UI update event
        UpdateAllyUI += UpdateAllyInfo;
        BattleController.OnBattleStart += InitializeBattleUI;
        BattleController.OnBattleEnd += HandleBattleEnd;
        BattleController.OnMinigameStarted += HandleMinigameStarted;
        BattleController.OnMinigameEnded += HandleMinigameEnded;
    }

    private bool TryInitializeUI()
    {
        if (isUIInitialized)
        {
            return true;
        }

        if (uiDocument == null)
        {
            Debug.LogError("BattleUIController is missing UIDocument reference.");
            return false;
        }

        root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogWarning("BattleUIController could not initialize UI because rootVisualElement is null.");
            return false;
        }

        InitializeUI();
        isUIInitialized = true;
        return true;
    }

    void InitializeUI()
    {
        // Initialize Ally 1 elements
        allyContainerElement1 = root.Q<VisualElement>(name: allyContainer1);
        allyNameElement1 = root.Q<Label>(name: allyName1);
        allySpriteElement1 = root.Q<VisualElement>(name: allySprite1);
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
        allySpriteElement2 = root.Q<VisualElement>(name: allySprite2);
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
        // optionsFullContainerElement = root.Q<VisualElement>(name: optionsFullContainer);
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
        SetBattleSquareActive(false, "UI initialization");
        root.style.display = DisplayStyle.None;
    }

    void InitializeBattleUI(BattleStartPayload payload)
    {
        if (payload.payloadType == BattleStartPayload.PayloadType.BattleData && payload.battle != null)
        {
            if (!TryInitializeUI())
            {
                Debug.LogError("BattleUIController failed to initialize UI before battle start.");
                return;
            }

            currentAllies = payload.battle.allies;
            currentEnemies = payload.battle.enemies;
            currentSelectionBatch = new BattleSelectionBatch();
            currentActorIndex = 0;
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
            StartActorSelection();
            // SetBattleSquareActive(false, "Battle UI initialized");
            root.style.display = DisplayStyle.Flex;
            battleCamera.enabled = true;
        }
    }

    private Character? GetCurrentActor()
    {
        if (currentAllies == null || currentActorIndex < 0 || currentActorIndex >= currentAllies.Length)
        {
            return null;
        }

        Character actor = currentAllies[currentActorIndex];
        return actor != null && actor.IsAlive ? actor : null;
    }

    private int FindNextLivingActorIndex(int startIndex)
    {
        if (currentAllies == null || currentAllies.Length == 0)
        {
            return -1;
        }

        for (int i = Mathf.Max(0, startIndex); i < currentAllies.Length; i++)
        {
            Character ally = currentAllies[i];
            if (ally != null && ally.IsAlive)
            {
                return i;
            }
        }

        return -1;
    }

    private Button[] GetCurrentActionButtons()
    {
        return currentActorIndex == 0 ? actionButtons1 : actionButtons2;
    }

    private int GetCurrentActionIndex()
    {
        return currentActorIndex == 0 ? selectedActionIndex1 : selectedActionIndex2;
    }

    private void SetCurrentActionIndex(int newIndex)
    {
        if (currentActorIndex == 0)
        {
            selectedActionIndex1 = newIndex;
        }
        else
        {
            selectedActionIndex2 = newIndex;
        }
    }

    private bool HasSecondActor
    {
        get
        {
            return hasAlly2 && currentAllies != null && currentAllies.Length > 1;
        }
    }

    private void StartActorSelection()
    {
        int nextLivingActorIndex = FindNextLivingActorIndex(currentActorIndex);
        if (nextLivingActorIndex < 0)
        {
            currentSelectionState = SelectionState.None;
            return;
        }

        currentActorIndex = nextLivingActorIndex;
        activeSelection = new BattleActionSelection
        {
            actor = GetCurrentActor()
        };

        currentSelectionState = currentActorIndex == 0 ? SelectionState.Ally1 : SelectionState.Ally2;
        currentMenuContext = SelectionMenuContext.None;
        AdjustOptionsVisibility(false);
    }

    private void HandleBattleEnd()
    {
        SetBattleSquareActive(false, "Battle ended");
        if (root != null)
        {
            root.style.display = DisplayStyle.None;
        }
        battleCamera.enabled = false;
    }

    private void HandleMinigameStarted()
    {
        SetBattleSquareActive(true, "Minigame started");
    }

    private void HandleMinigameEnded()
    {
        SetBattleSquareActive(false, "Minigame ended");
    }

    private void SetBattleSquareActive(bool isActive, string reason)
    {
        Debug.Log($"Setting battle square active: {isActive}. Reason: {reason}");
        if (battleSquare != null)
        {
            battleSquare.SetActive(isActive);
        }
    }

    private void ClearOptionSelection()
    {
        if (optionButtons == null)
        {
            return;
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] != null)
            {
                optionButtons[i].RemoveFromClassList("dialog-option--selected");
            }
        }
    }

    private void ClearActionSelection(Button[] actionButtons, int actionIndex)
    {
        if (actionButtons == null || actionIndex < 0 || actionIndex >= actionButtons.Length)
        {
            return;
        }

        actionButtons[actionIndex].RemoveFromClassList("action-btn--selected");
    }

    private void SelectActionIndex(int newIndex)
    {
        var actionButtons = GetCurrentActionButtons();
        int lastSelectedActionIndex = GetCurrentActionIndex();
        ClearActionSelection(actionButtons, lastSelectedActionIndex);
        SetCurrentActionIndex(newIndex);
        actionButtons[newIndex].AddToClassList("action-btn--selected");
    }

    private void ShowOptions(string[] labels, SelectionMenuContext context)
    {
        currentMenuContext = context;
        currentSelectionState = SelectionState.Options;
        ClearOptionSelection();
        SetOptionButtonText(labels);
        selectedOptionIndex = 0;
        if (visibleOptionCount > 0 && optionButtons[0] != null)
        {
            optionButtons[0].AddToClassList("dialog-option--selected");
        }
        AdjustOptionsVisibility(true);
    }

    private int GetVisibleOptionCount()
    {
        if (optionButtons == null)
        {
            return 0;
        }

        return Mathf.Clamp(visibleOptionCount, 0, optionButtons.Length);
    }

    private string[] BuildEnemyLabels()
    {
        if (currentEnemies == null || currentEnemies.Length == 0)
        {
            return new[] { "No targets" };
        }

        string[] labels = new string[currentEnemies.Length];
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            labels[i] = currentEnemies[i].Name;
        }

        return labels;
    }

    private string[] BuildAllyLabels()
    {
        if (currentAllies == null || currentAllies.Length == 0)
        {
            return new[] { "No targets" };
        }

        string[] labels = new string[currentAllies.Length];
        for (int i = 0; i < currentAllies.Length; i++)
        {
            Character ally = currentAllies[i];
            labels[i] = ally != null && ally.IsAlive ? ally.Name : "Down";
        }

        return labels;
    }

    private string[] BuildAbilityLabels()
    {
        Character? actor = GetCurrentActor();
        if (actor == null || actor.Abilities == null || actor.Abilities.Length == 0)
        {
            return new[] { "No abilities" };
        }

        List<string> labels = new List<string>();
        foreach (Ability ability in actor.Abilities)
        {
            if (ability != null && ability.Unlocked)
            {
                labels.Add(ability.Name);
            }
        }

        return labels.Count > 0 ? labels.ToArray() : new[] { "No abilities" };
    }

    private string[] BuildItemLabels()
    {
        if (Inventory.instance == null || Inventory.instance.items == null || Inventory.instance.items.Count == 0)
        {
            return new[] { "No items" };
        }

        string[] labels = new string[Inventory.instance.items.Count];
        for (int i = 0; i < Inventory.instance.items.Count; i++)
        {
            labels[i] = Inventory.instance.items[i].itemName;
        }

        return labels;
    }

    private void FinalizeCurrentSelection()
    {
        if (activeSelection == null)
        {
            return;
        }

        if (activeSelection.IsComplete)
        {
            currentSelectionBatch.selections.Add(activeSelection);
        }

        activeSelection = null;

        if (currentActorIndex == 0 && HasSecondActor)
        {
            currentActorIndex = FindNextLivingActorIndex(currentActorIndex + 1);
            if (currentActorIndex >= 0)
            {
                StartActorSelection();
                return;
            }

            if (BattleController.Instance != null)
            {
                BattleController.Instance.SubmitSelections(currentSelectionBatch);
            }

            currentSelectionBatch = new BattleSelectionBatch();
            currentSelectionState = SelectionState.None;
            currentMenuContext = SelectionMenuContext.None;
            AdjustOptionsVisibility(false);
            return;
        }

        if (BattleController.Instance != null)
        {
            BattleController.Instance.SubmitSelections(currentSelectionBatch);
        }

        currentSelectionBatch = new BattleSelectionBatch();
        currentSelectionState = SelectionState.None;
        currentMenuContext = SelectionMenuContext.None;
        AdjustOptionsVisibility(false);
    }

    private void BackOutOneStep()
    {
        if (currentSelectionState != SelectionState.Options)
        {
            return;
        }

        if (currentMenuContext == SelectionMenuContext.HackTargetEnemy || currentMenuContext == SelectionMenuContext.HackTargetAlly)
        {
            ShowOptions(BuildAbilityLabels(), SelectionMenuContext.HackAbility);
            return;
        }

        if (currentMenuContext == SelectionMenuContext.ItemTargetAlly)
        {
            ShowOptions(BuildItemLabels(), SelectionMenuContext.ItemSelect);
            return;
        }

        currentSelectionState = currentActorIndex == 0 ? SelectionState.Ally1 : SelectionState.Ally2;
        currentMenuContext = SelectionMenuContext.None;
        AdjustOptionsVisibility(false);
    }

    void HandleMoveInput(Vector2 input)
    {
        if (!isUIInitialized) return;
        // Debug.Log($"Move Input Received: {input}");
        if (currentSelectionState == SelectionState.Options)
        {
            int visibleCount = GetVisibleOptionCount();
            if (visibleCount <= 0)
            {
                return;
            }

            selectedOptionIndex = Mathf.Clamp(selectedOptionIndex, 0, visibleCount - 1);
            int lastSelectedOptionIndex = selectedOptionIndex;

            if (input.x > 0.5f)
            {
                // Move right
                selectedOptionIndex = (selectedOptionIndex + 3) % visibleCount;
            }
            else if (input.x < -0.5f)
            {
                // Move left
                selectedOptionIndex = (selectedOptionIndex - 3 + visibleCount) % visibleCount;
            }
            else if (input.y > 0.5f)
            {
                // Move up
                selectedOptionIndex = (selectedOptionIndex - 1 + visibleCount) % visibleCount;
            }
            else if (input.y < -0.5f)
            {
                // Move down
                selectedOptionIndex = (selectedOptionIndex + 1) % visibleCount;
            }

            if (lastSelectedOptionIndex >= 0 && lastSelectedOptionIndex < visibleCount && optionButtons[lastSelectedOptionIndex] != null)
            {
                optionButtons[lastSelectedOptionIndex].RemoveFromClassList("dialog-option--selected");
            }
            if (selectedOptionIndex >= 0 && selectedOptionIndex < visibleCount && optionButtons[selectedOptionIndex] != null)
            {
                optionButtons[selectedOptionIndex].AddToClassList("dialog-option--selected");
            }
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
            int visibleCount = GetVisibleOptionCount();
            if (visibleCount <= 0)
            {
                return;
            }

            selectedOptionIndex = Mathf.Clamp(selectedOptionIndex, 0, visibleCount - 1);

            if (currentMenuContext == SelectionMenuContext.AttackTarget || currentMenuContext == SelectionMenuContext.BoomTarget)
            {
                if (currentEnemies != null && currentEnemies.Length > 0)
                {
                    activeSelection!.selectedEnemyTarget = currentEnemies[Mathf.Clamp(selectedOptionIndex, 0, currentEnemies.Length - 1)];
                }
                FinalizeCurrentSelection();
                return;
            }

            if (currentMenuContext == SelectionMenuContext.HackAbility)
            {
                Character? actor = GetCurrentActor();
                if (actor == null || actor.Abilities == null || actor.Abilities.Length == 0)
                {
                    return;
                }

                List<Ability> unlockedAbilities = new List<Ability>();
                foreach (Ability ability in actor.Abilities)
                {
                    if (ability != null && ability.Unlocked)
                    {
                        unlockedAbilities.Add(ability);
                    }
                }

                if (unlockedAbilities.Count == 0)
                {
                    return;
                }

                activeSelection!.selectedAbility = unlockedAbilities[Mathf.Clamp(selectedOptionIndex, 0, unlockedAbilities.Count - 1)];
                if (!activeSelection.selectedAbility.NeedsTarget)
                {
                    FinalizeCurrentSelection();
                    return;
                }

                if (activeSelection.selectedAbility.IsOffensive)
                {
                    ShowOptions(BuildEnemyLabels(), SelectionMenuContext.HackTargetEnemy);
                }
                else
                {
                    ShowOptions(BuildAllyLabels(), SelectionMenuContext.HackTargetAlly);
                }

                return;
            }

            if (currentMenuContext == SelectionMenuContext.HackTargetEnemy)
            {
                if (currentEnemies != null && currentEnemies.Length > 0)
                {
                    activeSelection!.selectedEnemyTarget = currentEnemies[Mathf.Clamp(selectedOptionIndex, 0, currentEnemies.Length - 1)];
                }

                FinalizeCurrentSelection();
                return;
            }

            if (currentMenuContext == SelectionMenuContext.HackTargetAlly)
            {
                if (currentAllies != null && currentAllies.Length > 0)
                {
                    Character selectedAlly = currentAllies[Mathf.Clamp(selectedOptionIndex, 0, currentAllies.Length - 1)];
                    if (selectedAlly == null || !selectedAlly.IsAlive)
                    {
                        return;
                    }

                    activeSelection!.selectedAllyTarget = selectedAlly;
                }

                FinalizeCurrentSelection();
                return;
            }

            if (currentMenuContext == SelectionMenuContext.ItemSelect)
            {
                if (Inventory.instance == null || Inventory.instance.items.Count == 0)
                {
                    return;
                }

                activeSelection!.selectedItem = Inventory.instance.items[Mathf.Clamp(selectedOptionIndex, 0, Inventory.instance.items.Count - 1)];
                ShowOptions(BuildAllyLabels(), SelectionMenuContext.ItemTargetAlly);
                return;
            }

            if (currentMenuContext == SelectionMenuContext.ItemTargetAlly)
            {
                if (currentAllies != null && currentAllies.Length > 0)
                {
                    Character selectedAlly = currentAllies[Mathf.Clamp(selectedOptionIndex, 0, currentAllies.Length - 1)];
                    if (selectedAlly == null || !selectedAlly.IsAlive)
                    {
                        return;
                    }

                    activeSelection!.selectedAllyTarget = selectedAlly;
                }

                FinalizeCurrentSelection();
                return;
            }
        }
        else if (currentSelectionState == SelectionState.Ally1 || currentSelectionState == SelectionState.Ally2)
        {
            int selectedActionIndex = GetCurrentActionIndex();
            Character? actor = GetCurrentActor();
            if (actor == null)
            {
                return;
            }

            if (activeSelection == null)
            {
                activeSelection = new BattleActionSelection { actor = actor };
            }

            activeSelection.actor = actor;

            switch (selectedActionIndex)
            {
                case 0:
                    activeSelection.selectionType = BattleSelectionType.Attack;
                    ShowOptions(BuildEnemyLabels(), SelectionMenuContext.AttackTarget);
                    break;
                case 1:
                    activeSelection.selectionType = BattleSelectionType.Hack;
                    ShowOptions(BuildAbilityLabels(), SelectionMenuContext.HackAbility);
                    break;
                case 2:
                    activeSelection.selectionType = BattleSelectionType.Item;
                    ShowOptions(BuildItemLabels(), SelectionMenuContext.ItemSelect);
                    break;
                case 3:
                    activeSelection.selectionType = BattleSelectionType.Boom;
                    ShowOptions(BuildEnemyLabels(), SelectionMenuContext.BoomTarget);
                    break;
            }
        }
    }

    void HandleActionXInput()
    {
        if (currentSelectionState == SelectionState.Options)
        {
            BackOutOneStep();
            return;
        }

        if (currentSelectionState == SelectionState.Ally2 && currentActorIndex == 1)
        {
            currentActorIndex = 0;
            StartActorSelection();
        }
    }

    void AdjustOptionsVisibility(bool Visible = true)
    {
        //optionsFullContainerElement.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
        optionsContainerElement.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
        optionsContainerElement2.style.visibility = Visible ? Visibility.Visible : Visibility.Hidden;
    }

    void SetTextBoxDisplay(bool options = false)
    {
        if (options)
        {
            optionsContainerElement.style.display = DisplayStyle.Flex;
            textBoxContainerElement.style.display = DisplayStyle.None;
        }
        else
        {
            optionsContainerElement.style.display = DisplayStyle.None;
            textBoxContainerElement.style.display = DisplayStyle.Flex;
        }
    }

    void SetTextBoxText(string[] textLines)
    {
        for (int i = 0; i < textBoxOptions.Length; i++)
        {
            if (i < textLines.Length)
            {
                textBoxOptions[i].text = textLines[i];
                textBoxOptions[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                textBoxOptions[i].style.display = DisplayStyle.None;
            }
        }
    }

    void SetOptionButtonText(string[] optionLines)
    {
        visibleOptionCount = Mathf.Clamp(optionLines?.Length ?? 0, 0, optionButtons.Length);
        if (optionLines == null || optionLines.Length == 0)
        {
            visibleOptionCount = 0;
            return;
        }
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < visibleOptionCount)
            {
                optionButtons[i].text = optionLines[i];
                optionButtons[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                optionButtons[i].RemoveFromClassList("dialog-option--selected");
                optionButtons[i].style.display = DisplayStyle.None;
            }
        }
    }

    void UpdateAllyInfo(Character ally1, Character? ally2)
    {
        // Update Ally 1 UI
        allyNameElement1.text = ally1.Name;
        Debug.Log(ally1.Sprite[0]);
        allySpriteElement1.style.backgroundImage = new StyleBackground(ally1.Sprite[0]); // Assuming Character has a Sprite property
        float hpPercent1 = (float)ally1.CurrentHp / ally1.Hp;
        if (ally1.IsDown)
        {
            allyNameElement1.text = "Down";
            allyHP1Element.text = "Down";
            allyHealthBarElement1.style.width = Length.Percent(0);
        }
        else
        {
            allyNameElement1.text = ally1.Name;
            allyHP1Element.text = $"{ally1.CurrentHp}/{ally1.Hp}";
            allyHealthBarElement1.style.width = Length.Percent(hpPercent1 * 100);
        }

        // Update Ally 2 UI if ally2 is not null
        if (ally2 != null)
        {
            allySpriteElement2.style.backgroundImage = new StyleBackground(ally2.Sprite[0]); // Assuming Character has a Sprite property
            float hpPercent2 = (float)ally2.CurrentHp / ally2.Hp; // Assuming max HP is 100
            if (ally2.IsDown)
            {
                allyNameElement2.text = "Down";
                allyHP2Element.text = "Down";
                allyHealthBarElement2.style.width = Length.Percent(0);
            }
            else
            {
                allyNameElement2.text = ally2.Name;
                allyHP2Element.text = $"{ally2.CurrentHp}/{ally2.Hp}";
                allyHealthBarElement2.style.width = Length.Percent(hpPercent2 * 100);
            }
            allyContainerElement2.style.display = DisplayStyle.Flex;
        }
        else
        {
            allyContainerElement2.style.display = DisplayStyle.None;
        }
    }

    private void HandleAllyDamaged(Character _)
    {
        RefreshHealthBarsFromCurrentBattle();
    }

    private void HandleEnemyDamaged(Enemy _)
    {
        RefreshHealthBarsFromCurrentBattle();
    }

    private void RefreshHealthBarsFromCurrentBattle()
    {
        if (!isUIInitialized)
        {
            return;
        }

        Battle? battle = BattleController.CurrentBattle;
        if (battle == null || battle.allies == null || battle.allies.Length == 0 || battle.player == null)
        {
            return;
        }

        Character? ally2 = battle.allies.Length > 1 ? battle.allies[1] : null;
        UpdateAllyInfo(battle.player, ally2);
    }

}
