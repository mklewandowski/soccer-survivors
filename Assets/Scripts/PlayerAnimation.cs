using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator playerAnimator;
    bool isMoving;

    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
    }

    public void EndKick()
    {
        if (isMoving)
            playerAnimator.Play("player-walk" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        else
            playerAnimator.Play("player-idle" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
    }

    public void SetIsMoving(bool val)
    {
        isMoving = val;
    }
}
