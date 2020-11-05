using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject _playerObjectPrefab;
    [SerializeField] private GameObject _enemyObjectPrefab;

    private Maze _maze;
    private Unit _playerUnit;

    private List<Unit> _enemyUnits;

    // Start is called before the first frame update
    void Start()
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
            Position = Vector3.zero
        };

        _playerUnit = SpawnUnit(_playerObjectPrefab, emptyTileIndex, data);

        _enemyUnits = new List<Unit>();
        for (int i = 0; i < 10; i++)
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

            if(Input.GetMouseButtonDown(0))
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

                        if(clickedNode.Walkable)
                        {
                            _playerUnit.Move(clickedNode);
                            foreach (var item in _enemyUnits)
                            {
                                item.Move(clickedNode);
                            }
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
                MovementSpeed = 1,
                Position = Vector3.zero
            };
        }

        Vector3 atPosition = _maze.mazeMatrix[atIndex.Item1, atIndex.Item2].NodePosition;
        GameObject unitObject = Instantiate(fromPrefab, atPosition, Quaternion.identity);

        Unit unit = unitObject.GetComponent<Unit>();
        unit.Init(atIndex, withData);

        return unitObject.GetComponent<Unit>();
    }
}
