using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class BedPart : MonoBehaviour
{
    [SerializeField] private float growthTime = 10;

    [Header("Stages of growth from min to max")]
    [SerializeField] private List<Transform> growthStages;

    [SerializeField] private Block blockPrefab;

    [SerializeField] private ParticleSystem particle;

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

            CreateBlock();

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

    private void CreateBlock()
    {
        var block = Instantiate(blockPrefab, transform.position, Quaternion.identity);
        block.transform.SetParent(transform);
        block.transform.DOLocalMove(Vector3.up, 0.5f);

        Vector3 position = new Vector3(Random.Range(-1f, 1f), 0.1f, Random.Range(-1f, 1f));
        block.transform.DOLocalMove(position, 0.5f);
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
                StartCoroutine(PlayParticle());
            }
        }
    }

    IEnumerator PlayParticle()
    {
        particle.gameObject.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(0.5f);
        particle.Stop();
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
