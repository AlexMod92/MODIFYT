using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
	#region public
	public float normalShakePosX = 1.0f;
	public float normalShakePosY = 1.0f;
	public float normalShakeTime = 0.5f;
	[Space(5.0f)]
	public float goalShakePosX = 2.0f;
	public float goalShakePosY = 2.0f;
	public float goalShakeTime = 1.0f;
	#endregion

	#region private
	private Hashtable normalScreenShakeHt = new Hashtable();
	private Hashtable goalScreenShakeHt = new Hashtable();
	#endregion

	private void Awake()
	{
		// standard screenshake
		normalScreenShakeHt.Add("x", normalShakePosX);
		normalScreenShakeHt.Add("y", normalShakePosY);
		normalScreenShakeHt.Add("time", normalShakeTime);
		normalScreenShakeHt.Add("ignoretimescale", true);

		// goal screenshake
		goalScreenShakeHt.Add("x", goalShakePosX);
		goalScreenShakeHt.Add("y", goalShakePosY);
		goalScreenShakeHt.Add("time", goalShakeTime);
		goalScreenShakeHt.Add("ignoretimescale", true);
	}

	public void NormalScreenShakeCamera(GameObject _mainCamera)
	{
		iTween.ShakePosition(_mainCamera, normalScreenShakeHt);
	}

	public void GoalScreenShakeCamera(GameObject _mainCamera)
	{
		iTween.ShakePosition(_mainCamera, goalScreenShakeHt);
	}
}