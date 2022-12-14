using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssetRotator : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] Vector3 rotationDirection;

    // Start is called before the first frame update
    void Start()
    {
       //transform.DOLocalRotate(rotationDirection, time, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
       DoRotate();
    }

    void DoRotate()
    {
        //transform.DOLocalRotate(rotationDirection, time, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).OnComplete(DoRotate);
        transform.DORotate(rotationDirection, time, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
