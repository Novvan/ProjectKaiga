using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    #region Variables
    public float intensity = 1;
    public float smooth = 10;
    private Quaternion _origin;
    #endregion

    #region MonoBehaviour callbacks
    private void Start()
    {
        _origin = transform.localRotation;
    }

    private void Update()
    {
        _applySway();
    }
    #endregion

    #region Custom callbacks
    private void _applySway()
    {
        float _mouseY = Input.GetAxisRaw("Mouse Y");
        float _mouseX = Input.GetAxisRaw("Mouse X");

        Quaternion _xAdjustment = Quaternion.AngleAxis(-intensity * _mouseX, Vector3.up);
        Quaternion _yAdjustment = Quaternion.AngleAxis(intensity * _mouseY, Vector3.right);
        Quaternion _finalRotation = _origin * _xAdjustment * _yAdjustment;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, _finalRotation, Time.deltaTime * smooth);

    }
    #endregion
}
