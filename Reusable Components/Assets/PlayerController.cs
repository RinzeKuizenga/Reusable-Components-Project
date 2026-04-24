using UnityEngine;

public class PlayerController : MonoBehaviour
{
    KeyboardInput keyboardInput;
    FreeMovement freeMovement;

    void Awake()
    {
        keyboardInput = GetComponent<KeyboardInput>();
        freeMovement = GetComponent<FreeMovement>();
    }
    void Update()
    {
        freeMovement.Move(keyboardInput.GetInput());
    }
}
