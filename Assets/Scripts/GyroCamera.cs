using UnityEngine;
using System.Collections;

public class GyroCamera : MonoBehaviour
{
    // STATE
    private float _initialYAngle = 0f;
    private float _appliedGyroYAngle = 0f;
    private float _calibrationYAngle = 0f;
    private Transform _rawGyroRotation;
    private float _tempSmoothing;

    // SETTINGS
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float minX = -16;
    [SerializeField] private float maxX = 16;
    [SerializeField] private float minY = -13;
    [SerializeField] private float maxY = 13;
    [SerializeField] private float flickerFilter = 0.05f;

    private IEnumerator Start()
    {
        Input.gyro.enabled = true;
        Application.targetFrameRate = 60;
        _initialYAngle = transform.eulerAngles.y;

        _rawGyroRotation = new GameObject("GyroRaw").transform;
        _rawGyroRotation.position = transform.position;
        _rawGyroRotation.rotation = transform.rotation;

        // Wait until gyro is active, then calibrate to reset starting rotation.
        yield return new WaitForSeconds(1);

        StartCoroutine(CalibrateYAngle());
    }

    private void Update()
    {
        ApplyGyroRotation();
        ApplyCalibration();

        var rate = Input.gyro.rotationRate;
        if (!(rate.x <= -flickerFilter || rate.x >= flickerFilter))
        {
            rate.x = 0;
        }
        if (!(rate.y <= -flickerFilter || rate.y >= flickerFilter))
        {
            rate.y = 0;
        }
        rate.z = 0;
        rate *= _speed;

        var tempY = rate.y;
        rate.y = rate.x;
        rate.x = -tempY;

        var newPosition = transform.position + rate;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;

        if (Mathf.Abs(RenderSettings.skybox.GetFloat("_Rotation") - _appliedGyroYAngle) >= 0.5f)
        {
            RenderSettings.skybox.SetFloat("_Rotation", _appliedGyroYAngle);
        }
    }

    private IEnumerator CalibrateYAngle()
    {
        _tempSmoothing = _speed;
        _speed = 1;
        _calibrationYAngle = _appliedGyroYAngle - _initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
        yield return null;
        _speed = _tempSmoothing;
    }

    private void ApplyGyroRotation()
    {
        _rawGyroRotation.rotation = Input.gyro.attitude;
        _rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
        _rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
        _appliedGyroYAngle = _rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
    }

    private void ApplyCalibration()
    {
        _rawGyroRotation.Rotate(0f, -_calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
    }

    public void SetEnabled(bool value)
    {
        enabled = true;
        StartCoroutine(CalibrateYAngle());
    }
}