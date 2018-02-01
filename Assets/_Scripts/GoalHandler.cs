using UnityEngine;
using System.Collections;

public class GoalHandler : MonoBehaviour
{
    #region public
    [Header("Goals")]
    public GameObject[] goalsBlue = new GameObject[7];
    public GameObject[] goalsOrange = new GameObject[7];
    #endregion

    #region private
    private bool[] goalStates = new bool[7];
    #endregion

    private void Update()
    {
        SetGoal();
    }

    private void SetGoal()
    {
        for (int i = 0; i < goalStates.Length; i++)
        {
            // set all goal states to false
            goalStates[i] = false;

            // set selected goal state to active
            goalStates[(int)GameHandler.Instance.goalTypes] = true;
        }

        SetActiveGoal(goalStates[0], goalStates[1], goalStates[2], goalStates[3], goalStates[4], goalStates[5], goalStates[6]);
    }

    private void SetActiveGoal(bool goalDefault, bool goalDouble, bool goalFull, bool goalHalfTopBottom, bool goalHalfBottomTop, bool goalMoving, bool goalScaling)
    {
        // Blue
        goalsBlue[0].SetActive(goalDefault);
        goalsBlue[1].SetActive(goalDouble);
        goalsBlue[2].SetActive(goalFull);
        goalsBlue[3].SetActive(goalHalfTopBottom);
        goalsBlue[4].SetActive(goalHalfBottomTop);
        goalsBlue[5].SetActive(goalMoving);
        goalsBlue[6].SetActive(goalScaling);

        // Orange
        goalsOrange[0].SetActive(goalDefault);
        goalsOrange[1].SetActive(goalDouble);
        goalsOrange[2].SetActive(goalFull);
        goalsOrange[3].SetActive(goalHalfBottomTop);
        goalsOrange[4].SetActive(goalHalfTopBottom);
        goalsOrange[5].SetActive(goalMoving);
        goalsOrange[6].SetActive(goalScaling);
    }
}