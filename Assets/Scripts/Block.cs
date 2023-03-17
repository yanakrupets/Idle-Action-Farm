using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Block : MonoBehaviour
{
    public abstract int Cost { get; set; }

    public bool isAvailable = true;

    [SerializeField] private Vector3 _scaleForStack;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // перегрузить метод MoveTo для barn и stack
    public void MoveToStack(Vector3 destination)
    {
        transform.DOScale(new Vector3(0.3f, 0.16f, 0.2f), 1f);
        transform.DOLocalMove(destination, 1f);
        transform.DOLocalRotate(Vector3.zero, 1f);

        isAvailable = false;
    }
}
