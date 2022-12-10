using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private DartState _state = DartState.moving;
    [SerializeField] private List<Dart> _lsDart = new List<Dart>();
    [SerializeField] private Spiner _target;

    [SerializeField] private Vector3 colliderSize;

    public delegate void OnDartHit();
    public static event OnDartHit onDartHit;

    private void Update() {
        Moving();
    }

    public void Stop() {
        _state = DartState.stop;
    }

    public void Move(Spiner spiner, List<Dart> lsDart) {
        _target = spiner;
        _lsDart = lsDart;
        _state = DartState.moving;
    }

    private void Moving() {
        if (GameController.instance.gameState != GameState.play) {
            return;
        }

        if (_state.Equals(DartState.moving)) {
            transform.position += transform.right * Time.deltaTime * _speed;
            CheckDartDistance();
            CheckTargetDistance();
        }

    }

    private void CheckDartDistance() {
        float ySize = colliderSize.x / 2;
        Debug.Log("My Y Size: " + ySize);

        float xSize = colliderSize.y / 2;
        Debug.Log("My X Size: " + xSize);
        foreach (Dart dart in _lsDart) {
            if (dart == this) {
                continue;
            }
            bool yHit = false;
            bool xHit = false;
            Vector2 direction = transform.position - dart.transform.position;
            direction.y = Mathf.Abs(direction.y);
            direction.x = Mathf.Abs(direction.x);
            Debug.Log("Direction Vector: " + direction);

            float dartYSize = dart.colliderSize.x / 2;
            Debug.Log("Dart Y Size: " + dartYSize);

            float dartXSize = dart.colliderSize.y / 2;
            Debug.Log("Dart X Size: " + dartXSize);

            if (direction.y <= ySize + dartYSize) {
                yHit = true;
            }

            if (direction.x <= xSize + dartXSize) {
                xHit = true;
            }

            if(yHit && xHit) {
                GameController.instance.Lose();
                return;
            }
        }
    }

    private void CheckTargetDistance() {
        float distance = Vector2.Distance(transform.position, _target.transform.position);
        bool isHit = distance + _target.spinerRadius <= colliderSize.x;
        if (isHit) {
            Debug.Log("Is Hit:" + isHit);
            Hit();
        }
    }

    private void Hit() {
        _state = DartState.stop;
        transform.parent = _target.transform;
        if(_target.gameObject.tag.Equals("Spin")) {
            onDartHit.Invoke();
            return;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, colliderSize);
    }
}

public enum DartState { moving, stop}
