using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float _length, _startPos;
    private Camera _cam;
    public float parallaxEffect;
    
    private void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _cam = Camera.main;
    }
    private void Update()
    {
        var cameraPosition = _cam.transform.position;
        var temp = cameraPosition.x * (1 - parallaxEffect);
        var dist = (cameraPosition.x * parallaxEffect);

        var parallaxPosition = transform.position;
        parallaxPosition = new Vector3(_startPos + dist, parallaxPosition.y, parallaxPosition.z);
        transform.position = parallaxPosition;

        if(temp > _startPos + _length) _startPos += _length;
        else if(temp < _startPos - _length) _startPos -= _length;
    }
}
