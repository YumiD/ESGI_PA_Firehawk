using System.Collections.Generic;
using Grid;
using Scriptable_Objects;
using UI.Models;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField] private List<AIcon> icons;
    [SerializeField] private InputManagerBuild input;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitializeUi()
    {
        InGameUpdateUi();
    }

    public void InGameUpdateUi()
    {
        List<ItemDictionary> inventory = GameManager.Instance.GetInventory();
        
        for (int i = 0; i < input.ChoicesPrefab.Count; i++)
        {
            ItemDictionary it = inventory.Find(item => item.item == input.ChoicesPrefab[i].scriptableObject);

            if (it != null)
            {
                AIcon icon = icons.Find(ico => ico.Prop == it.item);
                icon.UpdateQuantity(it.quantity);
            }
            else
            {
                icons[i].UpdateQuantity(0);
            }
        }
    }
}