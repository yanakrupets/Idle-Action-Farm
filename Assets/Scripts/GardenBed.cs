using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] private List<BedPart> _bedParts;

    public bool CultureGrown { 
        get
        {
            return _bedParts.Any(x => x.IsGrown());
        }
        private set { } 
    }
}
