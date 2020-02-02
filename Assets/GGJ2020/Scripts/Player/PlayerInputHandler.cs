using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputHandler : MonoBehaviour
{
    public int playerId = 0;

    private Player rewiredPlayer;

    private void Awake()
    {
        rewiredPlayer = ReInput.players.GetPlayer(playerId);
    }

    public Vector2 GetLeftStickAxis()
    {
        float horizontal = rewiredPlayer.GetAxis(PlayerInput.LEFT_STICK_HORIZONTAL);
        float vertical = rewiredPlayer.GetAxis(PlayerInput.LEFT_STICK_VERTICAL);

        //vertical = Mathf.Abs(vertical) > PlayerInput.STICK_SENSITIVITY_THRESHOLD ? vertical : 0f;
        //horizontal = Mathf.Abs(horizontal) > PlayerInput.STICK_SENSITIVITY_THRESHOLD ? horizontal : 0f;

        return new Vector2(horizontal, vertical);
    }

    public Vector2 GetRightStickAxis()
    {
        float horizontal = rewiredPlayer.GetAxis(PlayerInput.RIGHT_STICK_HORIZONTAL);
        float vertical = rewiredPlayer.GetAxis(PlayerInput.RIGHT_STICK_VERTICAL);
       
        return new Vector2(-vertical, horizontal);
    }

    public bool GetJumpInput()
    {
        return rewiredPlayer.GetButton(PlayerInput.JUMP);
    }

    public bool GetInteractInput()
    {
        return rewiredPlayer.GetButton(PlayerInput.INTERACT);
    }

    public bool GetSprintInput()
    {
        return rewiredPlayer.GetAxis(PlayerInput.SPRINT) > PlayerInput.SPRINT_SENSITIVITY_THRESHOLD;
    }
    
    public bool GetFireInput()
    {
        return rewiredPlayer.GetAxis(PlayerInput.FIRE) > PlayerInput.FIRE_SENSITIVITY_THRESHOLD;
    }
    
}
