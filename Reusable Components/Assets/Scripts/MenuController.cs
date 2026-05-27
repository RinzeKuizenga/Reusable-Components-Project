using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    int selectedIndex;

    string[] pageoptions = { "Attack", "Items", "Supers" };
    [SerializeField] private Vector2[] blocksPos;

    [SerializeField] private List<AttackData> attacks;
    [SerializeField] private Vector2[] attacksPos;

    string[] Enemyoptions = {};
    string selectedEnemy = "";
    [SerializeField] private Vector2[] enemies;

    string[] currentOptions;
    Vector2[] currentPositions;

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

        if(currentSelection == "Attack")
        {
            menuBackground.SetActive(true);
            //currentOptions = Attackoptions;
            //currentPositions = attacks;

            selectedIndex = 0;
            MoveArrow();

            GameStateManager.Instance.ChangeState(GameState.Player1Action);
        }
        if(currentSelection == "SmallAttack")
        {
            menuBackground.SetActive(false);
            currentOptions = Enemyoptions;
            currentPositions = enemies;

            selectedIndex = 0;
            MoveArrow();

            GameStateManager.Instance.ChangeState(GameState.Player1Target);
        }
    }

    void Back()
    {
        selectedIndex = 0;
        menuBackground.SetActive(false);
        Destroy(gameObject);
        if(GameStateManager.Instance.CurrentState == GameState.Player1Action) GameStateManager.Instance.ChangeState(GameState.Player1Turn);
        if (GameStateManager.Instance.CurrentState == GameState.Player1Target) GameStateManager.Instance.ChangeState(GameState.Player1Action);

        Debug.Log($"Went back to {GameStateManager.Instance.CurrentState}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && GameStateManager.Instance.CurrentState == GameState.Player1Turn)
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D) && GameStateManager.Instance.CurrentState == GameState.Player1Turn)
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Z) )
        {
            Select();
        }
        if (Input.GetKeyDown(KeyCode.X) )
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.W) && GameStateManager.Instance.CurrentState == GameState.Player1Action)
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.S) && GameStateManager.Instance.CurrentState == GameState.Player1Action) 
        {
            MoveRight(); 
        }
    }
}
