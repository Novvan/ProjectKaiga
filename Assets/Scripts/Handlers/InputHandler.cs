using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Variables
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public int shoot = 0;
    public int aim = 1;
    public KeyCode reload = KeyCode.R;
    public KeyCode firstWeapon = KeyCode.Alpha1;
    public KeyCode secondWeapon = KeyCode.Alpha2;

    [Header("Camera")]
    public bool inverted = false;
    [Range(1f, 10f)] public int sensitivityY = 1;
    [Range(1f, 10f)] public int sensitivityX = 1;

    #endregion
}
