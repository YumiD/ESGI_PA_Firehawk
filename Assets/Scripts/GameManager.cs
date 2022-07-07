using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grid;
using Newtonsoft.Json.Linq;
using Scriptable_Objects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<ItemDictionary> _inventory;
    
    [SerializeField]
    private GameObject gridGenerator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ImportTerrainJson();
        
        _inventory = LevelSelect.LevelToLoad.Objects.Select(item => new ItemDictionary(item.item, item.quantity)).ToList();

        UiManager.Instance.InitializeUi();
    }
    
    public void ImportTerrainJson()
    {
        JObject jsonData = JObject.Parse(LevelSelect.LevelToLoad.JsonData.text);
                
        TerrainGrid terrainGridInstance = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TerrainGrid>();
        terrainGridInstance.Deserialize(jsonData, true);
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