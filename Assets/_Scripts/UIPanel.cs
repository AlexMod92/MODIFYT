using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UIPanel : MonoBehaviour
{
	private RectTransform rectTransform = null;

    #region int
    public int childHeight = 64;
    public int spaceHeight = 32;

    private int childCount = 0;
    #endregion

    private float newRectHeight;

    /// <summary>
    /// Set the height of a rect transform based on the child count
    /// </summary>
    private void Start()
	{
		rectTransform = GetComponent<RectTransform>();

        childCount = transform.childCount;

        // height of all childs + spaces between the child objects
        newRectHeight = (childHeight * childCount) + (spaceHeight * (childCount - 1));

        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, newRectHeight);
    }
}