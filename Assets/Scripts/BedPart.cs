using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BedPart : MonoBehaviour
{
    [SerializeField] private float growthTime = 10;

    [Header("Stages of growth from min to max")]
    [SerializeField] private List<Transform> growthStages;

    private List<Transform> _partsOfFinalStage;
    private bool _isGrown;
    private float _timeForOnePartGrow;

    private void Start()
    {
        _partsOfFinalStage = GetChildrenFromStage(growthStages.Last());
        _timeForOnePartGrow = growthTime / growthStages.Count;

        Initialize();
    }

    private void Update()
    {
        if (_partsOfFinalStage.Where(x => x.gameObject.activeSelf).Count() == 0 && _isGrown)
        {
            _isGrown = false;
            StartCoroutine(Grow());
        }
    }

    private void Initialize()
    {
        growthStages.ForEach(x => x.gameObject.SetActive(false));
        growthStages.Last().gameObject.SetActive(true);
        _isGrown = true;

        _partsOfFinalStage.ForEach(x => x.gameObject.SetActive(true));
    }

    private List<Transform> GetChildrenFromStage(Transform stage)
    {
        var childrens = new List<Transform>();
        for (int i = 0; i < stage.childCount; i++)
        {
            childrens.Add(stage.GetChild(i));
        }

        return childrens;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Scythe")
        {
            if (_partsOfFinalStage.Any(x => x.gameObject.activeSelf))
            {
                _partsOfFinalStage.First(x => x.gameObject.activeSelf).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Grow()
    {
        for (var i = 0; i < growthStages.Count; i++)
        {
            yield return new WaitForSeconds(_timeForOnePartGrow);

            growthStages[i].gameObject.SetActive(true);
            if (i != 0) growthStages[i - 1].gameObject.SetActive(false);
        }

        _partsOfFinalStage.ForEach(x => x.gameObject.SetActive(true));
        _isGrown = true;
    }
}
