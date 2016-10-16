using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public GameObject leftPlayer;
    public GameObject rightPlayer;

    private float minDist = 0.2f;
    private float minDist2 = 0.65f;
    private float negativeOne { get { return -1 + minDist; } }
    private float positiveOne { get { return 1 - minDist; } }
    private float negativeOne2 { get { return -1 + minDist2; } }
    private float positiveOne2 { get { return 1 - minDist2; } }

    Vector2 leftStick;
    Vector2 rightStick;
    int leftPosition;
    int rightPosition;
    
    void Update ()
    {
        #region Input
        leftStick = new Vector2(Input.GetAxis("LeftX"), Input.GetAxis("LeftY"));
        rightStick = new Vector2(Input.GetAxis("RightX"), Input.GetAxis("RightY"));
        leftPosition = GetPlayerPosition(leftStick);
        rightPosition = GetPlayerPosition(rightStick);
        //Debug.Log("lx:" + leftStick.x + " ly:" + leftStick.y + " rx:" + rightStick.x + " ry:" + rightStick.y);
        //Debug.Log(GetPlayerPosition(leftStick));

        if(Input.GetAxis("Trigger") > 0.5f || Input.GetAxis("Trigger") < -0.5f || Input.GetKeyDown(KeyCode.Space))
        {
            if(GameManager.Instance.TestPair(leftPosition, rightPosition))
            {
                // good stuff
            }
            else
            {
                // bad stuff
            }
        }

        #endregion

        leftPlayer.transform.position = GameManager.Instance.positions[leftPosition] * GameManager.Instance.positionOffset;
        rightPlayer.transform.position = GameManager.Instance.positions[rightPosition] * GameManager.Instance.positionOffset;
    }

    private int GetPlayerPosition(Vector2 _stick)
    {
        if (_stick.x < negativeOne2 && _stick.y > positiveOne2)
            return 0;
        if (_stick.x > positiveOne2 && _stick.y > positiveOne2)
            return 2;
        if (_stick.x > positiveOne2 && _stick.y < negativeOne2)
            return 4;
        if (_stick.x < negativeOne2 && _stick.y < negativeOne2)
            return 6;

        if (_stick.y > positiveOne)
            return 1;
        if (_stick.x > positiveOne)
            return 3;
        if (_stick.y < negativeOne)
            return 5;
        if (_stick.x < negativeOne)
            return 7;
        
        return 8;
    }
}
