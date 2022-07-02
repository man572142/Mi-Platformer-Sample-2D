using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    enum Mode { UnityDefault,DotweenVelocity,LerpVelocity}

    [SerializeField] Player[] players;

    [System.Serializable]
    struct Player
    {
        public GameObject player;
        public Mode mode;
    }


    public void SwitchToUnityDefault() => SwitchPlayer(Mode.UnityDefault);


    public void SwitchToDotweenVelocity() => SwitchPlayer(Mode.DotweenVelocity);

    public void SwitchToLerpVelocity() => SwitchPlayer(Mode.LerpVelocity);

    private void SwitchPlayer(Mode targetMode)
    {
        foreach(Player p in players)
        {
            p.player.SetActive(p.mode == targetMode);
        }
    }


}
