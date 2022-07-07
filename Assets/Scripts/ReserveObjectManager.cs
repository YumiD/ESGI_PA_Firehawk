using System.Collections.Generic;
using UnityEngine;

public class ReserveObjectManager : MonoBehaviour
{
    public static ReserveObjectManager Instance;
    private readonly List<KeyValuePair<FireObjectScriptableObject, GameObject>> _instantiateObject = new List<KeyValuePair<FireObjectScriptableObject, GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddInReserve(FireObjectScriptableObject item, Transform itemTransform)
    {
        GameObject obj = Instantiate(item.Prefab, itemTransform.position, itemTransform.rotation);
        obj.SetActive(false);
        _instantiateObject.Add(new KeyValuePair<FireObjectScriptableObject, GameObject>(item, obj));
    }

    public void RemoveInReserve(Transform item)
    {
        KeyValuePair<FireObjectScriptableObject, GameObject> found = _instantiateObject.Find(it => it.Value.transform.position == item.transform.position);
        int index = _instantiateObject.FindIndex(it => it.Value.transform.position == item.transform.position);
        Destroy(found.Value);
        _instantiateObject.RemoveAt(index);
    }

    public void InstantiateReserve()
    {
        foreach (KeyValuePair<FireObjectScriptableObject, GameObject> item in _instantiateObject)
        {
            item.Value.SetActive(true);
        }
    }

    public void HideDuplicate()
    {
        for (int i = 0; i < _instantiateObject.Count; i++)
        {
            GameObject newCopy = Instantiate(_instantiateObject[i].Key.Prefab, _instantiateObject[i].Value.transform.position, _instantiateObject[i].Value.transform.rotation);
            Destroy(_instantiateObject[i].Value);
            _instantiateObject[i] = new KeyValuePair<FireObjectScriptableObject, GameObject>(_instantiateObject[i].Key, newCopy);
            _instantiateObject[i].Value.SetActive(false);
        }
    }
}