using System.Collections.Generic;
using Scriptable_Objects;
using UI;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField] private List<UiIcon> icons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitializeUi()
    {
        UpdateUi();
    }

    public void UpdateUi()
    {
        List<ItemDictionary> item = GameManager.Instance.GetInventory();

        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].UpdateQuantity(item[i].quantity);
        }
    }
}