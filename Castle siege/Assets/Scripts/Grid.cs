﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public float size;
    public float borderTop;
    public float borderBottom;
    public float borderLeft;
    public float borderRight;

    private bool isOutOfBounds;

	// Use this for initialization
	void Start () {
        isOutOfBounds = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            0,
            (float)zCount * size);

        result += transform.position;

        if (result.x > borderTop)
        {
            result.x = borderTop;
        }
        if (result.x < borderBottom)
        {
            result.x = borderBottom;
        }
        if (result.z > borderRight)
        {
            result.z = borderRight;
        }
        if (result.z < borderLeft)
        {
            result.z = borderLeft;
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 40 ; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
