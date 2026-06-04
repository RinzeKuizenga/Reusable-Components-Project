
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

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
    int selectedIndex;

    [SerializeField] private List<string> blocks;
    [SerializeField] private Vector2[] blocksPos;

    [SerializeField] private List<TextMeshPro> optionsText;

    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private List<ItemData> items;
    [SerializeField] private List<SuperData> supers;
    [SerializeField] private List<Enemy> enemies;


    [SerializeField] private Vector2[] menuSlots;
    [SerializeField] private Vector2[] players;

    [SerializeField] private GameObject menuBackground;
    [SerializeField] private Transform arrowTrans;

    Vector2[] currentPositions;
    private int currentOptionCount;

    Vector2 targetPosition;
    float speed;

    MenuStates currentState;
    Actions selectedAction;
    TargetType currentTargetType;

    public AttackData selectedAttack;
    public ItemData selectedItem;
    public SuperData selectedSuper;
    public PlayerController currentPlayer;

    public event Action<AttackCommand> onAttackChosen;

    private void Awake()
    {
        currentState = MenuStates.Main; ;
        currentPositions = blocksPos;
        currentOptionCount = blocks.Count;
        MoveArrow();
    }

    public void WhichPlayer(PlayerController player)
    {
        currentPlayer = player;
    }
    void MoveRight()
    {
        selectedIndex++;
        if (selectedIndex >= currentOptionCount)
        {
            selectedIndex = 0;
        }
        MoveArrow();
    }
    void MoveLeft()
    {
        selectedIndex--;

        if (selectedIndex < 0)
        {
            selectedIndex = currentOptionCount - 1;
        }
        MoveArrow();
    }


    void MoveArrow()
    {
        Debug.Log($"Index: {selectedIndex}");
        Debug.Log($"Positions Length: {currentPositions.Length}");
        Debug.Log($"Option Count: {currentOptionCount}");
        if (currentState == MenuStates.Target)
        {
            targetPosition = (Vector2)enemies[selectedIndex].transform.position + new Vector2(-1f, 0f);
        }
        else
        {
            targetPosition = currentPositions[selectedIndex];
        }

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

    void Select()
    {
        switch (currentState)
        {
            case MenuStates.Main:
                if (selectedIndex == 0) selectedAction = Actions.Attack;
                if (selectedIndex == 1) selectedAction = Actions.Item;
                if (selectedIndex == 2) selectedAction = Actions.Super;
                SwitchState(MenuStates.Action);
                break;

            case MenuStates.Action:
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
                if (currentTargetType == TargetType.Enemy)
                {
                    AttackCommand command = new AttackCommand();

                    command.attack = selectedAttack;
                    command.target = enemies[selectedIndex];
                    command.attacker = currentPlayer;
                    onAttackChosen?.Invoke(command);
                }
                Destroy(gameObject);
                break;
        }
        selectedIndex = 0;
    }

    void Back()
    {
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


    void SwitchState(MenuStates newState)
    {
        currentState = newState;

        selectedIndex = 0;

        switch (currentState)
        {
            case MenuStates.Main:
                currentPositions = blocksPos;
                currentOptionCount = blocks.Count;
                menuBackground.SetActive(false);
                break;

            case MenuStates.Action:
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
                break;

            case MenuStates.Target:
                if (currentTargetType == TargetType.Enemy)
                {
                    enemies = FindObjectsOfType<Enemy>().ToList();
                    currentOptionCount = enemies.Count;

                }
                else if (currentTargetType == TargetType.Ally)
                {
                    currentPositions = players;
                    currentOptionCount = players.Length;
                }

                menuBackground.SetActive(false);
                break;
        }

        MoveArrow();
    }

    void DisplayText()
    {
        Debug.Log("Text displayed");
        for (int i = 0; i < optionsText.Count; i++)
        {
            string text = "";

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
        arrowTrans.position = Vector2.Lerp(arrowTrans.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(arrowTrans.position, targetPosition) < 0.01f)
        {
            arrowTrans.position = targetPosition;
        }
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

        if (Input.GetKeyDown(KeyCode.Z))
            Select();

        if (Input.GetKeyDown(KeyCode.X))
            Back();
    }
}
