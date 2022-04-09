using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int _posX;
    private int _posY;

    // Reference to the gameObject that gets placed on this cell
    [SerializeField] public GameObject _objectInThisGridSpace = null;

    public bool _isOccupied = false;
    public bool _isOnFire = false;
    [SerializeField] public bool _canBeOnFire;
    [SerializeField] public float _hitPoint;
    [SerializeField] public float _lifetime;

    void Start(){
        if(_objectInThisGridSpace!=null)
            _isOccupied = true;
    }

    void Update()
    {
        if(!_canBeOnFire)
            return;
        if(!_isOnFire){
            if(_hitPoint <= 0)
                SetFire();
            return;
        }
        if(_lifetime <= 0){
            _canBeOnFire = false;
            _isOnFire = false;
            GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            return;
        }
        // If can be on fire, is on fire and lifetime not over, then we propagate
        PropagateFire();
    }

    public bool IsCurrentlyOnFire()
    {
        return _canBeOnFire && _isOnFire;
    }

    public void SetFire(){
        if(!_canBeOnFire)
            return;
        _isOnFire = true;
        _hitPoint = 0;
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        StartCoroutine(FadeFire());
    }
    public void PropagateFire(){
        List<GridCell> flammableNeighbors = this.transform.parent.gameObject.GetComponent<GameGrid>().GetFlammableNeighbors(_posX, _posY);
        foreach(GridCell cell in flammableNeighbors)
            cell._hitPoint--;
    }
    IEnumerator FadeFire()
    {
        yield return new WaitForSeconds(_lifetime);
        _lifetime = 0;
    }

    public void SetPosition(int x, int y)
    {
        _posX = x;
        _posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(_posX, _posY);
    }

}
