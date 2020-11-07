using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject _playerObjectPrefab;
    [SerializeField] private GameObject _enemyObjectPrefab;
    [SerializeField] private GameObject _cityObject;

    [SerializeField] private GameObject _projectile;

    private Maze _maze;
    private Unit _playerUnit;

    private List<Unit> _enemyUnits;

    MazeNode _cityNode = null;
    public Vector3 CityPos => _cityNode.NodePosition;
    public Vector3 PlayerPos => _playerUnit.transform.position;
    public MazeNode CityNode => _cityNode;

    float time = 10 * 60;

    int cityHp = 200;

    public GameObject Projectile { get => _projectile;}

    // Start is called before the first frame update
    void Start()
    {

        
    }

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

        (int, int) emptyTileIndex = MazeManager.Instance.GetEmptyNodeIndex();

        UnitData data = new UnitData()
        {
            HitPoints = 200,
            Damage = 10,
            MovementSpeed = 5,
        };

        SpawnBase(MazeManager.Instance.GetEmptyNodeIndex());

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
            foreach (var item in _enemyUnits)
            {
                item.ExecuteUnitState();
            }

            if (Input.GetMouseButtonDown(0))
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

            yield return null;
        }
    }

    private Unit SpawnUnit(GameObject fromPrefab, (int, int) atIndex, UnitData withData = null)
    {

        if(withData == null)
        {
            withData = new UnitData()
            {
                HitPoints = 100,
                Damage = 10,
                MovementSpeed = 2,
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
        Debug.Log($"SPAWNING AT LOCATION {atIndex.Item1} {atIndex.Item2}");

        MazeManager.Instance.InitBase(atIndex);

        _cityNode = _maze.mazeMatrix[atIndex.Item1, atIndex.Item2];
        Vector3 atPosition = _cityNode.NodePosition;
        GameObject unitObject = Instantiate(_cityObject, atPosition, Quaternion.identity);
    }

    public void SpawnProjectile(Vector3 atLocation)
    {
        Debug.Log($"atLocation --->> {atLocation}");

        GameObject projectile = Instantiate(_projectile);
        projectile.transform.position = atLocation;
        Projectile p= projectile.GetComponent<Projectile>();
        p.Fly(_cityNode.NodePosition);

    }

    public void TimerCountDown(float timeInSeconds)
    {
        float iniTime = timeInSeconds;
    }
}
