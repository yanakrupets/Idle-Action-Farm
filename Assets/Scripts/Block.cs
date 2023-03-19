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
    private bool isInBarn = false;

    private Transform _bedPart;

    private void Update()
    {
        if (isInBarn && transform.localPosition == Vector3.zero)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void ShowBlock()
    {
        gameObject.SetActive(true);

        transform.DOLocalMove(Vector3.up, 1f);
    }

    public void MoveToStack(Vector3 destination)
    {
        transform.DOScale(new Vector3(0.25f, 0.16f, 0.2f), 1f);
        transform.DOLocalMove(destination, 1f);
        transform.DOLocalRotate(Vector3.zero, 1f);

        isAvailable = false;
    }

    public void MoveToBarn(Transform barn)
    {
        isInBarn = true;

        transform.SetParent(barn);

        transform.DOLocalMove(Vector3.zero, 1f);
        transform.DOLocalRotate(Vector3.zero, 1f);

        isAvailable = true;
    }

    public void InitParentBed(Transform parent)
    {
        _bedPart = parent;
    }
}
