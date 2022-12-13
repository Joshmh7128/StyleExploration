using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssetRotator : MonoBehaviour
{
    [SerializeField] float time;

    // Start is called before the first frame update
    void Start()
    {
       transform.DOLocalRotate(new Vector3(0, 360, 0), time, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
