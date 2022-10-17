using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_InterfaceDelegate : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float speed;
    [SerializeField] Vector3 direction;
    Sequence _sequence;

    void Start()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMove(_target.position, 3f));
    }

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
