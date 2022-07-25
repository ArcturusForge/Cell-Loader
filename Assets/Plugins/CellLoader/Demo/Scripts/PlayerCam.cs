using UnityEngine;

public enum CamState { Menu, Player }

public class PlayerCam : MonoBehaviour
{
    public Transform playerTarget;

    public void SwitchState(CamState state)
    {
        switch (state)
        {
            case CamState.Player:
                transform.SetParent(playerTarget);
                break;

            case CamState.Menu:
                transform.SetParent(null);
                break;
            default:
                break;
        }
    }

    public void SetToPlayer()
    {
        SwitchState(CamState.Player);
    }
}
