using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    public GameObject ChaseTrigger;
    [SerializeField] private Transform _start;
    [SerializeField] private float _moveAmount = 1f;
    public float NoHitTimer;
    [SerializeField] private float _getAwayThreshold;
    private bool timerActive = true;
    public int TimesHit;
    public int MaxTimesHit;
    [SerializeField] private float _lerpTimeElapsed;
    [SerializeField] private float _lerpDuration = 1f;
    [SerializeField] public Coroutine _chaseCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        MaxTimesHit = Mathf.RoundToInt(Mathf.Abs(_start.position.z / _moveAmount));
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            NoHitTimer += Time.deltaTime;
        }

        if (NoHitTimer > _getAwayThreshold)
        {
            //Debug.Log("chase: player got away");
            
            ChaseTrigger.transform.position = _start.position;

            TimesHit = 0;
        }
    }

    public IEnumerator GetCloser()
    {
        //Debug.Log("chase: getting closer");
        
        timerActive = false;
        
        var currentPosition = ChaseTrigger.transform.position;
            
        var newPosition = currentPosition + new Vector3(0, 0, _moveAmount);

        _lerpTimeElapsed = 0;
        
        while (_lerpTimeElapsed < _lerpDuration)
        {
            ChaseTrigger.transform.position = Vector3.Lerp(currentPosition, newPosition, _lerpTimeElapsed / _lerpDuration);

            _lerpTimeElapsed += Time.deltaTime;
                
            yield return null;
        }
            
        ChaseTrigger.transform.position = newPosition;
            
        NoHitTimer = 0;
            
        timerActive = true;
    }
}
