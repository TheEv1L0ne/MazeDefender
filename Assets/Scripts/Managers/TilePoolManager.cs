using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePoolManager : Singleton<TilePoolManager>
{

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _poolHolder;
    [SerializeField] private int _initPoolSize;

    private List<GameObject> _poolObjectList = null;

    protected override void OnAwake()
    {
        base.OnAwake();

        InitPool();
    }

    private void InitPool()
    {
        if (_poolObjectList == null)
        {
            _poolObjectList = new List<GameObject>();
            for (int p = 0; p < _initPoolSize; p++)
            {
                AddAndReturnNewTileToPool();
            }
        }
    }

    private void AddNewTileToPool()
    {
        GameObject poolObj = Instantiate(_tilePrefab, Vector3.zero, Quaternion.identity);
        poolObj.SetActive(false);
        poolObj.transform.parent = _poolHolder;

        _poolObjectList.Add(poolObj);
    }

    private GameObject AddAndReturnNewTileToPool()
    {
        GameObject poolObj = Instantiate(_tilePrefab, Vector3.zero, Quaternion.identity);
        poolObj.SetActive(false);
        poolObj.transform.parent = _poolHolder;

        _poolObjectList.Add(poolObj);

        return poolObj;
    }

    public GameObject GetTileFromPool()
    {
        foreach (var tile in _poolObjectList)
        {
            if(!tile.activeInHierarchy)
            {
                tile.SetActive(true);
                return tile;
            }
        }

        GameObject newTile = AddAndReturnNewTileToPool();
        newTile.SetActive(true);
        return newTile;
    }


    public void ReturnTileToPool(GameObject tile)
    {
        tile.transform.parent = _poolHolder;
        tile.transform.position = Vector3.zero;
        tile.SetActive(false);
    }

    public void ReturnAllTilesToPool()
    {
        foreach (var tile in _poolObjectList)
        {
            tile.transform.parent = _poolHolder;
            tile.transform.position = Vector3.zero;
            tile.SetActive(false);
        }
    }
}
