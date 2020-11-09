using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{
    #region EditorReferences
    [SerializeField] private GameObject _playerObjectPrefab;
    [SerializeField] private GameObject _enemyObjectPrefab;
    [SerializeField] private GameObject _cityObject;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _projectilePlayer;
    #endregion

    #region private variables
    private Maze _maze;
    private Unit _playerUnit;
    private List<Unit> _enemyUnits;
    private PlayerBase _playerBase;
    private MazeNode _cityNode = null;
    private List<Unit> _deadUnits = new List<Unit>();
    private bool _gameInProgress = false;
    #endregion

    #region Getters
    public GameObject Projectile { get => _projectile; }
    public GameObject ProjectilePlayer { get => _projectilePlayer; }
    public Unit PlayerUnit => _playerUnit;
    public List<Unit> EnemyUnits => _enemyUnits;
    public PlayerBase PlayerBase => _playerBase;
    public Vector3 CityPos => _cityNode.NodePosition;
    public Vector3 PlayerPos => _playerUnit.transform.position;
    public MazeNode CityNode => _cityNode;
    #endregion

    #region Unit Mono Functions
    protected override void OnAwake()
    {
        int music = PlayerPrefs.GetInt(StaticPrefStrings.MUSIC, 1);
        if (music == 1)
        {
            AudioManager.Instance.PlaySound(0);
        }
        else
        {
            AudioManager.Instance.StopSound(0);
        }
    }

    private void OnEnable()
    {
        UIManager.onStartPressedDelegate += OnStart;
        UIManager.onQuitPressedDelegate += OnQuit;
    }

    private void OnDisable()
    {
        UIManager.onStartPressedDelegate -= OnStart;
        UIManager.onQuitPressedDelegate -= OnQuit;
    }
    #endregion

    private void OnQuit()
    {
        StopAllCoroutines();

        for (int i = _enemyUnits.Count - 1; i >= 0; i--)
        {
            Destroy(_enemyUnits[i].gameObject);
        }

        Destroy(_playerUnit.gameObject);
        Destroy(_playerBase.gameObject);

        TilePoolManager.Instance.ReturnAllTilesToPool();
    }

    private void OnStart()
    {
        InitGame();
    }

    #region GAME PLAY
    private void InitGame()
    {
        MazeManager.Instance.GenerateMaze();     
        _maze = MazeManager.Instance.Maze;

        _deadUnits = new List<Unit>();

        //Base
        (int, int) baseSpawnIndex = MazeManager.Instance.GetEmptyNodeIndex();
        SpawnBase(baseSpawnIndex);

        //player
        (int, int) playerSpawnIndex = MazeManager.Instance.GetEmptyCloseTo(baseSpawnIndex);
        UnitData data = new UnitData()
        {
            HitPoints = 200,
            AttackDamage = 10,
            MovementSpeed = 5,
            AttackCooldown = .3f,
            AttackDistance = 4,
            AttackRange = 4
        };
        _playerUnit = SpawnUnit(_playerObjectPrefab, playerSpawnIndex, data);
        UIManager.Instance.playUI.SetPlayerHp(_playerUnit.Data.HitPoints, _playerUnit.Data.HitPoints);

        //Enemies
        _enemyUnits = new List<Unit>();

        int diff = PlayerPrefs.GetInt(StaticPrefStrings.DIFFICULTY, 0);

        int numberOfBots = diff == 0 ? 6 : 14;

        for (int i = 0; i < numberOfBots; i++)
        {
            UnitData withData = new UnitData()
            {
                HitPoints = 100,
                AttackDamage = 20,
                MovementSpeed = 2,
                AttackCooldown = 3,
                AttackDistance = 4,
                AttackRange = 3,
            };

            Unit enemy = SpawnUnit(_enemyObjectPrefab, MazeManager.Instance.GetEmptyNodeIndex(), withData);
            _enemyUnits.Add(enemy);
        }
        UIManager.Instance.playUI.SetEnemiesLeft(_enemyUnits.Count);

        //Player camera
        CameraManager.Instance.InitCameraAtLocation(playerSpawnIndex);

        _gameInProgress = true;
        StartCoroutine(GameLoop());
    }


    private IEnumerator GameLoop()
    {
        while (_gameInProgress)
        {
            if (!PlayerBase.IsAlive || !PlayerUnit.IsAlive || _enemyUnits.Count == 0)
            {
                FinishGame(_enemyUnits.Count == 0);
            }

            UIManager.Instance.playUI.SetEnemiesLeft(_enemyUnits.Count);
            foreach (var item in _enemyUnits)
            {

                if (item.IsAlive)
                    item.ExecuteUnitState();
                else
                {
                    _deadUnits.Add(item);
                }
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

;            for (int i = _deadUnits.Count-1; i >= 0; i--)
            {
                Destroy(_deadUnits[i].gameObject);
                _enemyUnits.Remove(_deadUnits[i]);
                _deadUnits.Remove(_deadUnits[i]);
            }
            
            yield return null;
        }
    }

    private void FinishGame(bool won)
    {
        _gameInProgress = false;
        UIManager.Instance.playUI.SetGameEndState(true, won);
    }
    #endregion

    #region SPAWNING
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
    #endregion
}
