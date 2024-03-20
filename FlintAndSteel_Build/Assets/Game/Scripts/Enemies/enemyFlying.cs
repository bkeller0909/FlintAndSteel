using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFlying : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 3f;
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float detectionRange = 5f;
    [SerializeField]
    private bool detectionEnabled = true;

    [SerializeField]
    private Transform start, end;
}
