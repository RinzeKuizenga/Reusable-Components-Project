using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MenuStates
{
    Main,
    Action,
    Target
}

public class MenuController : MonoBehaviour
{
    int selectedIndex;

    [SerializeField] private List<string> blocks;
    [SerializeField] private Vector2[] blocksPos;

    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private Vector2[] attacksPos;

    [SerializeField] private Vector2[] enemies;

    Vector2[] currentPositions;
    MenuStates currentState;

    AttackData selectedAttack;

    public Transform arrowTrans;
    Vector2 targetPosition;
    float speed;
    public GameObject menuBackground;

    private void Awake()
    {
        currentState = MenuStates.Main;
        //currentOptions = pageoptions;
        currentPositions = blocksPos;
        MoveArrow();
    }
    void MoveRight()
    {
        selectedIndex++;
        if (selectedIndex >= currentPositions.Length)
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
            selectedIndex = currentPositions.Length - 1;
        }
        MoveArrow();
    }


    void MoveArrow()
    {
        targetPosition = currentPositions[selectedIndex];
        //arrowTrans.position = Vector2.MoveTowards(arrowTrans.position, currentPositions[selectedIndex], 0.5f * Time.deltaTime);
        //arrowTrans.position = currentPositions[selectedIndex];

        switch (currentState)
        {
            case MenuStates.Main:
                speed = 16f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case MenuStates.Action:
                speed = 22f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case MenuStates.Target:
                speed = 28f;
                arrowTrans.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    void Select()
    {
        switch (currentState)
        {
            case MenuStates.Main:
                SwitchState(MenuStates.Action);
                break;
            case MenuStates.Action:
                 selectedAttack = attacks[selectedIndex];
                SwitchState(MenuStates.Target);
                break;
            case MenuStates.Target:
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

    private void Update()
    {
        arrowTrans.position = Vector2.MoveTowards(arrowTrans.position, targetPosition, speed * Time.deltaTime);
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
        }

        if (Input.GetKeyDown(KeyCode.Z))
            Select();

        if (Input.GetKeyDown(KeyCode.X))
            Back();
    }

    void SwitchState(MenuStates newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case MenuStates.Main:
                currentPositions = blocksPos;
                menuBackground.SetActive(false);
                break;
            case MenuStates.Action:
                currentPositions = attacksPos;
                menuBackground.SetActive(true);
                break;
            case MenuStates.Target:
                currentPositions = enemies;
                menuBackground.SetActive(false);
                break;
        }

        MoveArrow();
    }
}
