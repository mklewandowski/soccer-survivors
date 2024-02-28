using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Vector2 inNormal;

    public Vector2 GetInNormal()
    {
        return inNormal;
    }
}
