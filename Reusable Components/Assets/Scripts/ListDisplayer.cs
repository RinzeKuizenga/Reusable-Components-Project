using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ListDisplayer : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> optionsText;
    MenuController menuController;

    private void Awake()
    {
        menuController = GetComponent<MenuController>();
    }
    void DisplayText()
    {
        
    }
}
