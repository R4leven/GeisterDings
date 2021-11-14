using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject root;
    public int HorizontalSpeed = 100;
    public int VerticalSpeed = 100;

    public double SecondsGoVertical = 0.5;
    public double SecondsGoHorizontal = 0.5;

    private bool goesUp = false;
    private bool goesRight = false;

    void Update()
    {
        transform.RotateAround(root.transform.position, root.transform.up, HorizontalSpeed * Time.deltaTime);
    }
}
