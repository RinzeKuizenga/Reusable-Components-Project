using UnityEngine;


public class MenuController : MonoBehaviour
{
    string[] options = { "Attack", "Items", "Supers" };
    int selectedIndex = 0;

    private void Start()
    {
         
    }
    void MoveRight()
    {
        options[selectedIndex] += 1;
    }
    void MoveLeft()
    {
        options[selectedIndex] += 1;
    }

    private void Update()
    {
        Debug.Log($"{options[selectedIndex]}");
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveRight();
        }
    }
}
