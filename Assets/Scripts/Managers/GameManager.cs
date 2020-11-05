using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject playerObject;

    Maze maze;

    private Unit playerUnit;

    // Start is called before the first frame update
    void Start()
    {

        InitGame();
    }

    private void InitGame()
    {
        MazeManager.Instance.GenerateMaze();     
        maze = MazeManager.Instance.Maze;

        (int, int) emptyTileIndex = MazeManager.Instance.GetEmptyNodeIndex();

        UnitData data = new UnitData()
        {
            HitPoints = 200,
            Damage = 10,
            MovementSpeed = 5,
            Position = Vector3.zero
        };

        playerUnit = SpawnUnit(playerObject, emptyTileIndex, data);

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
                        Debug.Log($"Index of tile x = {x}, y = {y}");

                        if(clickedNode.Walkable)
                        {
                            playerUnit.Move(clickedNode);
                        }

                    }
                }
            }

            yield return null;
        }
    }

    private Unit SpawnUnit(GameObject fromPrefab, (int, int) atIndex, UnitData withData = null)
    {
        Vector3 atPosition = maze.mazeMatrix[atIndex.Item1, atIndex.Item2].NodePosition;
        GameObject unitObject = Instantiate(fromPrefab, atPosition, Quaternion.identity);

        Unit unit = unitObject.GetComponent<Unit>();
        unit.Init(atIndex, withData);

        return unitObject.GetComponent<Unit>();
    }
}
