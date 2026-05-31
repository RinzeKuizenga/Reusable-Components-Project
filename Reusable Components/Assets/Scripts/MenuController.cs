using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private List<ItemData> items;
    [SerializeField] private List<SuperData> supers;


    [SerializeField] private Vector2[] menuSlots;
    [SerializeField] private Vector2[] enemies;
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

    private void Awake()
    {
        currentState = MenuStates.Main;;
        currentPositions = blocksPos;
        currentOptionCount = blocks.Count;
        MoveArrow();
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
        targetPosition = currentPositions[selectedIndex];

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
                menuBackground.SetActive(true);
                break;

            case MenuStates.Target:
                if (currentTargetType == TargetType.Enemy)
                {
                    currentPositions = enemies; 
                   currentOptionCount = enemies.Length;

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
