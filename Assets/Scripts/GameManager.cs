using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelInventory levelInventory;
    private readonly List<ItemDictionary> _inventory = new List<ItemDictionary>();
    public bool IsEditMode { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (ItemDictionary obj in levelInventory.inventory.Select(item =>
                     new ItemDictionary(item.item, item.quantity)))
        {
            _inventory.Add(obj);
        }

        UiManager.Instance.InitializeUi();
    }

    public List<ItemDictionary> GetInventory()
    {
        return _inventory;
    }

    public void AddInInventory(GameObject obj)
    {
        foreach (ItemDictionary item in _inventory.Where(item => item.item.name == obj.name))
        {
            item.AddItem();
            return;
        }
    }

    public int VerifyInInventory(GameObject obj)
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i].item.name == obj.name)
            {
                bool result = _inventory[i].VerifyItem();
                if (result)
                {
                    return i;
                }

                return -1;
            }
        }

        return -1;
    }

    public void RemoveInInventory(int index)
    {
        _inventory[index].RemoveItem();
    }

    public void SetGameState(bool isEditMode)
    {
        IsEditMode = isEditMode;
    }
}