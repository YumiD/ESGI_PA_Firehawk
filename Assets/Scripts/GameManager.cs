using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelInventory levelInventory;
    private readonly List<ItemDictionary> _inventory = new List<ItemDictionary>();

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

    public void AddInInventory(FireObjectScriptableObject obj)
    {
        _inventory.Find(item => obj == item.item)?.AddItem();
    }

    public bool VerifyInInventory(FireObjectScriptableObject obj)
    {
        return _inventory.Find(item => obj == item.item).VerifyItem();
    }

    public void RemoveInInventory(FireObjectScriptableObject obj)
    {
        _inventory.Find(item => obj == item.item)?.RemoveItem();
    }
}