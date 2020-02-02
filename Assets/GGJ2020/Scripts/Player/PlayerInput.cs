using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput 
{
    public const string LEFT_STICK_HORIZONTAL = "HorizontalMovement"; 
    public const string LEFT_STICK_VERTICAL = "VerticalMovement";

    public const string RIGHT_STICK_HORIZONTAL = "CameraHorizontal";
    public const string RIGHT_STICK_VERTICAL = "CameraVertical";

    public const string JUMP = "Jump";
    public const string FIRE = "Fire";
    public const string SPRINT = "Sprint";
    public const string INTERACT = "Interact";

    public const float SPRINT_SENSITIVITY_THRESHOLD = 0.5f;
    public const float STICK_SENSITIVITY_THRESHOLD = 0.5f;
    public const float FIRE_SENSITIVITY_THRESHOLD = 0.5f;

}
