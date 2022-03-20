using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class TowerSelection : MonoBehaviour
{

    public GameObject resource_window;
    public GameObject base_tower_select_UI;
    public ThirdPersonController player_controller;
    public StarterAssetsInputs _input;
    public TowerPlacement tower_placement;

    private void Start()
    {
        _input = GameObject.FindObjectOfType<StarterAssetsInputs>();
    }

    public void Toggle_Tower_Select()
    {
        player_controller.enabled = !player_controller.enabled;
        base_tower_select_UI.SetActive(!base_tower_select_UI.activeSelf);
    }

    public void UI_Event_ToggleWindow()
    {
        resource_window.SetActive(!resource_window.activeSelf);
    } 

    public void UI_Event_SelectTower()
    {
        Toggle_Tower_Select();
        resource_window.SetActive(false);
        BuyTower();
    }

    public void Toggle_Workshop()
    {
        Toggle_Tower_Select();
    }

    public void BuyTower()
    {
        _input.fire = false;
        _input.SetCursorState(true);
        player_controller.enabled = true;
        base_tower_select_UI.SetActive(false);
        tower_placement.gameObject.SetActive(true);
        resource_window.SetActive(false);
    }

    public void Update()
    {
        if(_input.workshop)
        {
            Toggle_Tower_Select();
            _input.workshop = false;
            _input.SetCursorState(false);
        }        
    }
}
