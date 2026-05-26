using UnityEngine;


public class MenuController : MonoBehaviour
{
    int selectedIndex;

    string[] pageoptions = { "Attack", "Items", "Supers" };
    string selectedPage = "";
    [SerializeField] private Vector2[] blocks;

    string[] Attackoptions = { "SmallAttack", "BigAttack" };
    string selectedAttack = "";
    [SerializeField] private Vector2[] attacks;

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
        currentPositions = blocks;
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
        //arrowTrans.position = Vector2.MoveTowards(transform.position, blocks[selectedIndex], 8 *
        //Time.deltaTime);
        arrowTrans.position = currentPositions[selectedIndex];
    }

    void Select()
    {
        selectedPage = currentOptions[selectedIndex];

        if(selectedPage == "Attack")
        {
            menuBackground.SetActive(true);
            currentOptions = Attackoptions;
            currentPositions = attacks;

            selectedIndex = 0;
            MoveArrow();

            GameStateManager.Instance.ChangeState(GameState.Player1Action);
        }
    }

    void Back()
    {
        selectedIndex = 0;
        menuBackground.SetActive(false);
        Destroy(gameObject);
        selectedPage = "";
        GameStateManager.Instance.ChangeState(GameState.Player1Turn);
        Debug.Log($"Went back to");
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
        if (Input.GetKeyDown(KeyCode.Z) && GameStateManager.Instance.CurrentState == GameState.Player1Turn)
        {
            Select();
        }
        if (Input.GetKeyDown(KeyCode.X) && GameStateManager.Instance.CurrentState == GameState.Player1Action)
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
