using UnityEngine;
using System.Collections;
using Huffy.Utilities;
using System.Collections.Generic;

[System.Serializable]
public class NodePair
{
    [SerializeField]
    public int first;
    [SerializeField]
    public int second;

    public LineRenderer line;

    public NodePair()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>();
    }

    public NodePair(int _first, int _second)
    {
        first = _first;
        second = _second;

        line = new GameObject("Line").AddComponent<LineRenderer>();
    }
}

public class GameManager : SingletonBehaviour<GameManager>
{
    public int id = 0;
    private StateMachine<GameManager> sm;

    [SerializeField]
    public List<NodePair> currentPairs = new List<NodePair>();
    
    public Material lineMaterial;

    [HideInInspector]
    public float positionOffset = 4;
    [HideInInspector]
    public Vector3[] positions = {
        new Vector3(-1, 0, 1),  // 0
        new Vector3(0, 0, 1),   // 1
        new Vector3(1, 0, 1),   // 2
        new Vector3(1, 0, 0),   // 3
        new Vector3(1, 0, -1),  // 4
        new Vector3(0, 0, -1),  // 5
        new Vector3(-1, 0, -1), // 6
        new Vector3(-1, 0, 0),  // 7
        new Vector3(0, 0, 0)};  // 8


    private float timeToNext = 2.0f;

    IEnumerator Start ()
    {
        VersionNumber.Initialize();
        Config.Initialize();

        //yield return new WaitForSeconds(0.125f);
        //Container.Load("Container").PrintString();

        sm = new StateMachine<GameManager>(new TestStateOne(this));

        while(true)
        {
            AddRandomPair();
            timeToNext -= 0.02f;

            if (timeToNext < 0.4f)
                timeToNext = 0.4f;

            yield return new WaitForSeconds(timeToNext);
        }
    }

    void Update()
    {
        if (sm != null)
            sm.Update();

        if (Input.GetKeyDown(KeyCode.X))
            AddRandomPair();
    }

    public bool TestPair(int _left, int _right)
    {
        NodePair matchingPair = null;
        foreach (NodePair n in currentPairs)
        {
            if ((n.first == _left || n.second == _left) && (n.first == _right || n.second == _right))
            {
                matchingPair = n;
            }
        }

        if(matchingPair != null)
        {
            currentPairs.Remove(matchingPair);
            Destroy(matchingPair.line.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddRandomPair()
    {
        if (currentPairs.Count < 10)
        {
            NodePair pair = GetRandomPair();
            pair.line.SetPosition(0, positions[pair.first] * positionOffset);
            pair.line.SetPosition(1, positions[pair.second] * positionOffset);
            pair.line.material = lineMaterial;
            currentPairs.Add(pair);
        }
    }

    NodePair GetRandomPair()
    {
        NodePair pair = new NodePair(Random.Range(0, 8), Random.Range(0, 8));

        if (pair.first == pair.second)
            pair = GetRandomPair();
        

        bool foundMatchingPair = false;
        foreach(NodePair n in currentPairs)
        {
            if (n.first == pair.first && n.second == pair.second)
                foundMatchingPair = true;
        }

        if (foundMatchingPair)
            return GetRandomPair();
        else
            return pair;
    }
    
}
