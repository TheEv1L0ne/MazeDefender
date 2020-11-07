using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject _playerObjectPrefab;
    [SerializeField] private GameObject _enemyObjectPrefab;
    [SerializeField] private GameObject _cityObject;

    [SerializeField] private GameObject _projectile;
    public GameObject Projectile { get => _projectile; }

    private Maze _maze;
    private Unit _playerUnit;
    public Unit PlayerUnit => _playerUnit;

    private List<Unit> _enemyUnits;
    public List<Unit> EnemyUnits => _enemyUnits;

    private PlayerBase _playerBase;
    public PlayerBase PlayerBase => _playerBase;

    MazeNode _cityNode = null;
    public Vector3 CityPos => _cityNode.NodePosition;
    public Vector3 PlayerPos => _playerUnit.transform.position;
    public MazeNode CityNode => _cityNode;

    private void OnEnable()
    {
        UIManager.onStartPressedkDelegate += OnStart;
    }

    private void OnDisable()
    {
        UIManager.onStartPressedkDelegate -= OnStart;
    }

    private void OnStart()
    {
        InitGame();
    }

    private void InitGame()
    {
        MazeManager.Instance.GenerateMaze();     
        _maze = MazeManager.Instance.Maze;

        SpawnBase(MazeManager.Instance.GetEmptyNodeIndex());

        (int, int) emptyTileIndex = MazeManager.Instance.GetEmptyNodeIndex();

        UnitData data = new UnitData()
        {
            HitPoints = 200,
            AttackDamage = 10,
            MovementSpeed = 5,
            AttackCooldown = 2,
            AttackDistance = 4,
            AttackRange = 4
        };

        _playerUnit = SpawnUnit(_playerObjectPrefab, emptyTileIndex, data);

        _enemyUnits = new List<Unit>();
        for (int i = 0; i < 1; i++)
        {
            Unit enemy = SpawnUnit(_enemyObjectPrefab, MazeManager.Instance.GetEmptyNodeIndex());
            _enemyUnits.Add(enemy);
        }

        CameraManager.Instance.InitCameraAtLocation(emptyTileIndex);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while(true)
        {
            if (!PlayerBase.IsAlive)
            {
                FinishGame();
            }

            foreach (var item in _enemyUnits)
            {
                item.ExecuteUnitState();
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {

                Vector3 mousePos = Input.mousePosition;
                mousePos = CameraManager.Instance.MainCamera.ScreenToWorldPoint(mousePos);
                mousePos.z = 0f;

                RaycastHit2D hit = Physics2D.Raycast(mousePos, -Vector2.up, 0f);
                {
                    if (hit.collider != null)
                    {
                        int arrayIndex = hit.transform.GetSiblingIndex();
                        int x = arrayIndex / MazeManager.Instance.Maze.MazeSizeY;
                        int y = arrayIndex % MazeManager.Instance.Maze.MazeSizeY;

                        MazeNode clickedNode = MazeManager.Instance.GetNode(x, y);

                        if(clickedNode.Type == MazeNode.TileType.Ground
                            || clickedNode.Type == MazeNode.TileType.City)
                        {
                            _playerUnit.Move(clickedNode);
                        }

                    }
                }

            }

            _playerUnit.ExecuteUnitState();

            yield return null;
        }
    }

    private void FinishGame()
    {
        throw new NotImplementedException();
    }

    private Unit SpawnUnit(GameObject fromPrefab, (int, int) atIndex, UnitData withData = null)
    {

        if(withData == null)
        {
            withData = new UnitData()
            {
                HitPoints = 100,
                AttackDamage = 10,
                MovementSpeed = 2,
                AttackCooldown = 3,
                AttackDistance = 4,
                AttackRange = 3,
            };
        }

        Vector3 atPosition = _maze.mazeMatrix[atIndex.Item1, atIndex.Item2].NodePosition;
        GameObject unitObject = Instantiate(fromPrefab, atPosition, Quaternion.identity);

        Unit unit = unitObject.GetComponent<Unit>();
        unit.Init(atIndex, withData);

        return unitObject.GetComponent<Unit>();
    }

    private void SpawnBase((int, int) atIndex)
    {
        _cityNode = _maze.mazeMatrix[atIndex.Item1, atIndex.Item2];
        Vector3 atPosition = _cityNode.NodePosition;

        GameObject baseObject = Instantiate(_cityObject, atPosition, Quaternion.identity);
        _playerBase = baseObject.GetComponent<PlayerBase>();
        _playerBase.Init(200, atIndex);
    }
}
