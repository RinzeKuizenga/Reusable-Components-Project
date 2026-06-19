using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MenuStates
{
    Main,
    Action,
    Target
}

public enum Actions
{
    Attack,
    Item,
    Super
}

public class MenuController : MonoBehaviour
{
    // Tracks the currently highlighted option in the active menu state.
    int selectedIndex;

    // Main menu option labels and their arrow positions.
    [SerializeField] private List<string> blocks;
    [SerializeField] private Vector2[] blocksPos;

    // Text fields used to display attacks, items, or super moves.
    [SerializeField] private List<TextMeshPro> optionsText;

    // Available actions that can be selected during a player turn.
    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private List<ItemData> items;
    [SerializeField] private List<SuperData> supers;
    [SerializeField] private List<Enemy> enemies;

    // Positions used by the action menu and ally targeting menu.
    [SerializeField] private Vector2[] menuSlots;
    [SerializeField] private Vector2[] players;

    // References used to control the menu visuals.
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject AKey;
    [SerializeField] private GameObject DKey;
    [SerializeField] private Transform arrowTrans;

    // Stores the positions and option count for the currently active menu state.
    Vector2[] currentPositions;
    private int currentOptionCount;

    // Stores the position and movement speed of the selection arrow.
    Vector2 targetPosition;
    float speed;

    // Stores the current menu flow and selected action information.
    MenuStates currentState;
    Actions selectedAction;
    TargetType currentTargetType;

    // Stores the currently selected battle action and active player.
    public AttackData selectedAttack;
    public ItemData selectedItem;
    public SuperData selectedSuper;
    public PlayerController currentPlayer;

    // Sends the selected command back to the PlayerController.
    public event Action<AttackCommand> onAttackChosen;
    public event Action<ItemCommand> onItemChosen;

    //WARNING DEZE SCRIPT IS NIET FANTASTISCH GESCHREVEN I KNOW, HIJ HEEFT BEST VEEL IF-STATEMENTS ALLEEN IK ZAG GEEN ANDERE MANIER OM HET TE DOEN IK HEB ECHT M'N BEST GEDAAN ZONDER AI DUS PLS GUN ME DIT HET WERKT EN IS MAKKELIJK OM UIT TE BREIDEN :(
    private void Awake()
    {
        // Start the menu at the main action selection screen.
        currentState = MenuStates.Main; ;
        currentPositions = blocksPos;
        currentOptionCount = blocks.Count;
        MoveArrow();
    }

    // Assigns the player who opened this menu.
    public void WhichPlayer(PlayerController player)
    {
        currentPlayer = player;
    }

    // Moves the selection to the next available option.
    void MoveRight()
    {
        selectedIndex++;

        // Return to the first option after reaching the end of the list.
        if (selectedIndex >= currentOptionCount)
        {
            selectedIndex = 0;
        }

        MoveArrow();
        SFXPlayer.Instance.PlaySFX(1, 1f);
    }

    // Moves the selection to the previous available option.
    void MoveLeft()
    {
        selectedIndex--;

        // Return to the final option when moving left from the first option.
        if (selectedIndex < 0)
        {
            selectedIndex = currentOptionCount - 1;
        }

        MoveArrow();
        SFXPlayer.Instance.PlaySFX(1, 1f);
    }

    // Updates the target position, speed, and rotation of the selection arrow.
    void MoveArrow()
    {
        // Debug.Log($"Index: {selectedIndex}");
        // Debug.Log($"Positions Length: {currentPositions.Length}");
        // Debug.Log($"Option Count: {currentOptionCount}");

        // Target either an enemy, an ally, or a menu option depending on the current state.
        if (currentState == MenuStates.Target)
        {
            if (currentTargetType == TargetType.Enemy) targetPosition = (Vector2)enemies[selectedIndex].transform.position + new Vector2(-1f, 0f);
            else targetPosition = players[selectedIndex];
        }
        else
        {
            targetPosition = currentPositions[selectedIndex];
        }

        // Change arrow movement and appearance depending on the menu layout.
        switch (currentState)
        {
            case MenuStates.Main:
                speed = 16f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            case MenuStates.Action:
                speed = 40f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            case MenuStates.Target:
                speed = 40f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    // Confirms the currently selected option in the active menu state.
    void Select()
    {
        SFXPlayer.Instance.PlaySFX(0, 1f);

        switch (currentState)
        {
            case MenuStates.Main:
                // Choose which action category the player wants to open.
                if (selectedIndex == 0) selectedAction = Actions.Attack;
                if (selectedIndex == 1) selectedAction = Actions.Item;
                if (selectedIndex == 2) selectedAction = Actions.Super;

                SwitchState(MenuStates.Action);
                break;

            case MenuStates.Action:
                // Store the selected action data and determine its valid target type.
                switch (selectedAction)
                {
                    case Actions.Attack:
                        selectedAttack = attacks[selectedIndex];
                        currentTargetType = selectedAttack.target;
                        break;

                    case Actions.Item:
                        selectedItem = items[selectedIndex];
                        currentTargetType = selectedItem.target;
                        break;

                    case Actions.Super:
                        selectedSuper = supers[selectedIndex];
                        currentTargetType = selectedSuper.target;
                        break;
                }

                SwitchState(MenuStates.Target);
                break;

            case MenuStates.Target:
                // Create a command containing the selected action and enemy target.
                if (currentTargetType == TargetType.Enemy)
                {
                    AttackCommand command = new AttackCommand();

                    command.attack = selectedAttack;
                    command.enemy = enemies[selectedIndex];
                    command.attacker = currentPlayer;

                    onAttackChosen?.Invoke(command);
                }
                // Create a command containing the selected item and ally target.
                else if (currentTargetType == TargetType.Ally)
                {
                    ItemCommand command = new ItemCommand();

                    command.item = selectedItem;
                    command.target = GetTargetPlayer(selectedIndex);

                    // Prevent the player from wasting a healing item on a full-health target.
                    if (command.target.health >= command.target.MaxHealth)
                    {
                        SFXPlayer.Instance.PlaySFX(4, 1f);
                        return;
                    }

                    command.user = currentPlayer;

                    onItemChosen?.Invoke(command);
                }

                // Close the menu after a valid action has been selected.
                Destroy(gameObject);
                break;
        }

        // Reset the selection for the next menu state.
        selectedIndex = 0;
    }

    // Returns to the previous menu state when the player presses the back button.
    void Back()
    {
        SFXPlayer.Instance.PlaySFX(3, 1f);

        switch (currentState)
        {
            case MenuStates.Action:
                SwitchState(MenuStates.Main);
                break;

            case MenuStates.Target:
                SwitchState(MenuStates.Action);
                break;
        }

        selectedIndex = 0;
    }

    // Configures the menu visuals and selectable options for a new menu state.
    void SwitchState(MenuStates newState)
    {
        currentState = newState;

        // Start each state with the first available option selected.
        selectedIndex = 0;

        switch (currentState)
        {
            case MenuStates.Main:
                // Restore the main action buttons and horizontal navigation controls.
                currentPositions = blocksPos;
                currentOptionCount = blocks.Count;

                menuBackground.SetActive(false);
                AKey.SetActive(true);
                DKey.SetActive(true);
                break;

            case MenuStates.Action:
                // Show the list of actions belonging to the selected action category.
                currentPositions = menuSlots;

                switch (selectedAction)
                {
                    case Actions.Attack:
                        currentOptionCount = attacks.Count;
                        break;

                    case Actions.Item:
                        currentOptionCount = items.Count;
                        break;

                    case Actions.Super:
                        currentOptionCount = supers.Count;
                        break;
                }

                DisplayText();

                menuBackground.SetActive(true);
                AKey.SetActive(false);
                DKey.SetActive(false);
                break;

            case MenuStates.Target:
                // Refresh enemy targets when selecting an enemy-targeting action.
                if (currentTargetType == TargetType.Enemy)
                {
                    enemies = FindObjectsOfType<Enemy>().ToList();
                    currentOptionCount = enemies.Count;
                }
                // Set up ally targeting using the configured player target positions.
                else if (currentTargetType == TargetType.Ally)
                {
                    Debug.Log($"{currentTargetType}");
                    currentPositions = players;
                    currentOptionCount = players.Length;
                }

                menuBackground.SetActive(false);
                break;
        }

        MoveArrow();
    }

    // Displays the available attacks, items, or super moves in the action menu.
    void DisplayText()
    {
        for (int i = 0; i < optionsText.Count; i++)
        {
            string text = "";

            // Select the correct display name based on the active action category.
            switch (selectedAction)
            {
                case Actions.Attack:
                    if (i < attacks.Count)
                        text = attacks[i].attackName;
                    break;

                case Actions.Item:
                    if (i < items.Count)
                        text = items[i].itemName;
                    break;

                case Actions.Super:
                    if (i < supers.Count)
                        text = supers[i].superName;
                    break;
            }

            // Only show text fields that are used by the current action category.
            if (i < currentOptionCount)
            {
                optionsText[i].gameObject.SetActive(true);
                optionsText[i].text = text;
            }
            else
            {
                optionsText[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Smoothly move the selection arrow toward its currently selected target.
        arrowTrans.position = Vector2.Lerp(arrowTrans.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(arrowTrans.position, targetPosition) < 0.01f)
        {
            arrowTrans.position = targetPosition;
        }

        // Use different navigation keys depending on the current menu layout.
        switch (currentState)
        {
            case MenuStates.Main:
                if (Input.GetKeyDown(KeyCode.A))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.D))
                    MoveRight();

                break;

            case MenuStates.Action:
                if (Input.GetKeyDown(KeyCode.W))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.S))
                    MoveRight();

                break;

            case MenuStates.Target:
                if (Input.GetKeyDown(KeyCode.W))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.S))
                    MoveRight();

                break;
        }

        // Confirm or cancel the active selection.
        if (Input.GetKeyDown(KeyCode.Z))
            Select();

        if (Input.GetKeyDown(KeyCode.X))
            Back();
    }

    // Finds the selected player and returns their healable component.
    IHealable GetTargetPlayer(int index)
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

        // Prevent invalid list access when the target index does not exist.
        if (index < 0 || index >= playerControllers.Length)
            return null;

        Player player = playerControllers[index].GetComponent<Player>();

        return player;
    }
}