using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMoveAnimation : MonoBehaviour
{
    public static PlayerMoveAnimation Instance;

    [SerializeField] private float animationSpeed = .5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }
    public void MoveAnimation(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, animationSpeed);
    }
}
