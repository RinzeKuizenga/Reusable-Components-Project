using NUnit.Framework;
using System.Collections.Generic;
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

    string[] pageoptions = { "Attack", "Items", "Supers" };
    [SerializeField] private Vector2[] blocksPos;

    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private Vector2[] attacksPos;

    [SerializeField] private Vector2[] enemies;

    string[] currentOptions;
    Vector2[] currentPositions;
    MenuStates currentState;

    public Transform arrowTrans;
    public GameObject menuBackground;

    private void Awake()
    {
        currentOptions = pageoptions;
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
        arrowTrans.position = currentPositions[selectedIndex];
    }

    void Select()
    {
        string currentSelection = currentOptions[selectedIndex];
        Debug.Log($"currentOptions: {currentOptions[selectedIndex]}");


    }

    void Back()
    {
        selectedIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) )
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Select();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            MoveRight(); 
        }
    }

    void switchstate(MenuStates newState)
    {
        newState = currentState;
    }
}
