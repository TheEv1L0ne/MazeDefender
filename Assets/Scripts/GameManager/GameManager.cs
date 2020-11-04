using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject playerObject;

    MazeNode startNode = null;
    MazeNode endNode = null;
    Maze maze;

    private GameObject spawnedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
        MazeManager.Instance.GenerateMaze();
        maze = MazeManager.Instance.Maze;
        Vector2 playerPos = SpawnPlayer(maze);
        startNode = maze.mazeMatrix[(int)playerPos.x, (int)playerPos.y];

        CameraManager.Instance.InitCameraAtLocation(startNode.NodePosition, maze.MazeSizeX, maze.MazeSizeY);

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

                        Debug.Log($"Index of tile x = {x}, y = {y}");

                        if (endNode == null || (endNode != maze.mazeMatrix[x, y]))
                        {
                            endNode = maze.mazeMatrix[x, y];

                            AStarPathfinding aStar = new AStarPathfinding();
                            aStar.InitPathfinder(maze, startNode, endNode);
                            aStar.FindPath();
                            aStar.GenerateMazeGraphics(MazeManager.Instance.MazeGraphicsHolder);

                            if (ITrave != null)
                            {
                                StopCoroutine(ITrave);
                                ITrave = null;
                            }

                            ITrave = ITravelToDestination(aStar.GetPath());
                            StartCoroutine(ITrave);
                        }

                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator ITrave = null;

    private IEnumerator ITravelToDestination(List<MazeNode> path)
   {

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 destinationPos = path[i].NodePosition;
            Vector3 currentPos = spawnedPlayer.transform.position;

            startNode = path[i];

            float step = 0;
            float speed = 5f;

            while (spawnedPlayer.transform.position != destinationPos)
            {
                yield return null;
                step += speed * Time.deltaTime;
                spawnedPlayer.transform.position = Vector2.MoveTowards(currentPos, destinationPos, step);
                CameraManager.Instance.UpdateCameraPos(spawnedPlayer.transform.position);
            }
        }

        endNode = null;
    }

    private Vector2 SpawnPlayer(Maze maze)
    {
        bool spawnLocGood = false;

        int x = -1;
        int y = -1;

        while (!spawnLocGood)
        {
            x = Random.Range(0, maze.mazeMatrix.GetLength(0));
            y = Random.Range(0, maze.mazeMatrix.GetLength(1));

            if (maze.mazeMatrix[x,y].Walkable)
            {
                spawnLocGood = true;
            }
        }

        Vector3 position = maze.mazeMatrix[x, y].NodePosition;
        spawnedPlayer = Instantiate(playerObject, position, Quaternion.identity);

        //MazeManager.Instance.PlayerGraphics(x, y);

        return new Vector2(x, y);

    }
}
