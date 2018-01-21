using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    public Container TContainer;
    int Height = 30;
    int Width = 10;
    bool _gameover = false;
    public CubeFactory Factory;
    CubeSet CurCube;
    public Transform GameInfo;
    Vector3 OriginPos;

    private void Awake()
    {
        GameInfo.gameObject.SetActive(false);
        InitContainer();
    }

    private void Start()
    {
        Create();
        StartCoroutine(StartAutoMove(0.5f));
    }

    private void InitContainer()
    {
        TContainer.InitContainer(Width, Height);
    }

    void Create()
    {
        var obj = Factory.Create();
        CurCube = obj.GetComponent<CubeSet>();
        CurCube.transform.parent = this.transform;

        int startX = Width / 2 - 2;
        int startY = Height;
        Vect2 startPos = new Vect2(startX, startY);
        CurCube.SetPos(startPos, OriginPos);
        TContainer.SetStates(CurCube.GetCurCubeState(), false);
    }

    private IEnumerator StartAutoMove(float v)
    {
        while (!_gameover)
        {
            yield return new WaitForSeconds(v);
            if (!_gameover) CubeMove(CurCube, Direction.Down);
        }
        print("Stop Move");
    }

    private void CubeMove(CubeSet cube, Direction dir)
    {
        if (TContainer.CanMove(cube, dir))
        {
            TContainer.SetStates(cube.GetCurCubeState(), true);
            cube.MoveTo(dir);
            TContainer.SetStates(cube.GetCurCubeState(), false);
        }
        else
        {
            if (dir == Direction.Down)
            {
                TContainer.TransferCubes(CurCube);
                TContainer.RemoveLineCubes(CurCube);
                if (TContainer.IsGameOver(CurCube))
                {
                    _gameover = true;
                    GameOver();
                }
                else
                {
                    Create();
                }
            }
        }
    }

    void CubeRotate(CubeSet cube)
    {
        if (TContainer.CanRotate(cube))
        {
            TContainer.SetStates(cube.GetCurCubeState(), true);
            cube.Rotate();
            TContainer.SetStates(cube.GetCurCubeState(), false);
        }
        else
        {

        }
    }

    private void GameOver()
    {
        GameInfo.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_gameover)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CubeMove(CurCube, Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CubeMove(CurCube, Direction.Right);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                CubeMove(CurCube, Direction.Down);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CubeRotate(CurCube);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("teris");
        }
    }
}
