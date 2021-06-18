using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class PlayerTouchController : MonoBehaviour
{
    private Touch _touch;
    private SpriteRenderer _indicator;
    private PlayerShootBehavior _playerShootBehavior;

    private LineRenderer _bounceLineRenderer;
    private float _distanceBounceLine = 10f;
    private int _currentReflectionsBounceLine = 0;
    private int _maxReflectionsBounceLine = 1;
    // private Vector3 _pos = new Vector3();
    private List<Vector3> _points;

    private bool _isAiming;

    void Start()
    {
        _isAiming = false;

        _playerShootBehavior = GetComponent<PlayerShootBehavior>();

        _bounceLineRenderer = GetComponentInChildren<LineRenderer>();
        _bounceLineRenderer.widthMultiplier = 0.05f;
        ShowBounceLine(false);
        _bounceLineRenderer.positionCount = 2;
        _bounceLineRenderer.SetPosition(0, transform.position);

        _points = new List<Vector3>();
    }

    private void ShowBounceLine(bool doShow)
    {
        if (doShow)
        {
            _bounceLineRenderer.startColor = Color.red;
            _bounceLineRenderer.endColor = Color.red;
        }
        else
        {
            _bounceLineRenderer.startColor = new Color(0, 0, 0, 0);
            _bounceLineRenderer.endColor = new Color(0, 0, 0, 0);
        }
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        _touch = Input.GetTouch(0);

        var touchPos = _touch.position;
        var touchVector = new Vector3(touchPos.x, touchPos.y, 0);
        var createPos = (Vector2) Camera.main.ScreenToWorldPoint(touchPos);

        var cursorPositionOnCam = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var startPoint = (Vector2) transform.position;
        // var direction = (cursorPositionOnCam - startPoint).normalized;
        var direction = (createPos - startPoint).normalized;

        if (!Input.GetMouseButton(0))
        {
            ShowBounceLine(false);

            if (_isAiming)
            {
                _isAiming = false;
                StartCoroutine(_playerShootBehavior.Shoot(direction));
            }

            return;
        }

        _isAiming = true;

        _currentReflectionsBounceLine = 0;
        // var hit = Physics2D.Raycast(startPoint, (cursorPositionOnCam - startPoint).normalized, _distanceBounceLine);
        var hit = Physics2D.Raycast(startPoint, (createPos - startPoint).normalized, _distanceBounceLine);

        _points.Clear();
        _points.Add(startPoint);
        
        if (hit && (hit.transform.CompareTag("blocks") || hit.transform.CompareTag("walls")))
        {
            ReflectFurther(startPoint, hit);
        }
        else
        {
            _points.Add(startPoint + direction.normalized * 1000);
        }

        _bounceLineRenderer.positionCount = _points.Count;
        _bounceLineRenderer.SetPositions(_points.ToArray());
        ShowBounceLine(true);


        /*for (int i = 0; i < _maxReflectionsBounceLine; i++)
        {
            Ray2D ray = new Ray2D(startPoint, direction);

            Debug.DrawRay(startPoint, hit.point - startPoint, Color.green);
            if (hit.collider)
            {
                startPoint = hit.point;

                var rotation = Quaternion.FromToRotation(-ray.direction, hit.normal);
                direction = rotation * hit.normal;

                Debug.DrawRay(hit.point, direction, Color.magenta);
            }
        }*/
    }

    private void ReflectFurther(Vector2 origin, RaycastHit2D hit)
    {
        if (_currentReflectionsBounceLine > _maxReflectionsBounceLine) return;

        _points.Add(hit.point);
        _currentReflectionsBounceLine++;

        Vector2 inDir = (hit.point - origin).normalized;
        Vector2 newDir = Vector2.Reflect(inDir, hit.normal);

        var newHit = Physics2D.Raycast(hit.point + (newDir * 0.0001f), newDir * 100, _distanceBounceLine);
        if (newHit)
        {
            ReflectFurther(hit.point, newHit);
        }
        else
        {
            _points.Add(hit.point + newDir * _distanceBounceLine);
        }
    }
}