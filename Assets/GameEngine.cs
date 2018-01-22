using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    public Container TContainer;
    int Height = 20;
    int Width = 10;
    bool _gameover = false;
    public CubeFactory Factory;
    CubeSet CurCube;
    public Transform GameInfo;
    public Transform PreShow;
    Vector3 OriginPos;

    public float DropSpeed = 0.5f;

    public int PreShowCount = 5;

    private void Awake()
    {
        GameInfo.gameObject.SetActive(false);
        InitContainer();
        Factory.InitFactory(PreShowCount);
    }

    private void Start()
    {
        Create();
        StartCoroutine(StartAutoMove(DropSpeed));
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

        ShowPreCubes(5);
        print("Create");
    }

    private void ShowPreCubes(int count)
    {
        for (int i = 0; i < PreShow.childCount; i++)
        {
            Destroy(PreShow.GetChild(i).gameObject);
        }

        int sideLength = 1;
        Vector3 oriPos = Vector3.zero + new Vector3(0, sideLength / 2);
        int del = sideLength * 4;
        List<GameObject> objs = Factory.PreCreate(count);
        for (int i = 0; i < objs.Count; i++)
        {
            var obj = objs[i];
            obj.transform.parent = PreShow;
            obj.transform.localPosition = oriPos + new Vector3(0, i * del, 0);
            var cube = obj.GetComponent<CubeSet>();
            cube.SetPos(new Vect2(0,0), OriginPos);
        }

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
