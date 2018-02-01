using UnityEngine;
using System.Collections;

public class PaddlePunch : MonoBehaviour
{
	#region public
	public float punchPosX = 1.0f;
	public float punchTime = 0.5f;
	[Space(5.0f)]
	public bool isPunching = false;
	#endregion

	#region private
	private Hashtable paddlePunchHt = new Hashtable();
	#endregion

	private void Awake()
	{
		paddlePunchHt.Add("x", punchPosX);
		paddlePunchHt.Add("time", punchTime);
		/*paddlePunchHt.Add("onstart", "SetPunchingTrue");
		paddlePunchHt.Add("oncomplete", "SetPunchingFalse");*/
	}

	public void goalkeeperPunch(GameObject _goalkeeperPaddle)
	{
		iTween.PunchPosition(_goalkeeperPaddle, paddlePunchHt);
	}

	private void SetPunchingTrue()
	{
		isPunching = true;
	}

	private void SetPunchingFalse()
	{
		isPunching = false;
	}
}