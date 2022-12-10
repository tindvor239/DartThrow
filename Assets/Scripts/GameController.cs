using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public GameState gameState = GameState.play;
    [SerializeField] private Spiner _spiner;
    [SerializeField] private GameObject _dartPrefab;
    [SerializeField] private Vector2 _spawnPosition;
    [SerializeField] private Quaternion _spawnRotation;
    [SerializeField] private Dart _currentDart;

    [SerializeField] private List<Dart> _lsDart = new List<Dart>();

    [SerializeField] private GameObject _loseObject;

    [SerializeField] private TextMeshProUGUI _txtCounter;
    [SerializeField] private Button _retryButton;

    private float _shootDelayTime = 0.3f;
    private int _counter = 0;

    [SerializeField] private float _shootDelayTimeCounter = 0;

    private void Awake() {
        instance = this;
        _loseObject.SetActive(false);
        _retryButton.onClick.AddListener(Retry);
        Dart.onDartHit += OnDartHit;
    }

    private void Start() {
        _txtCounter.text = _counter.ToString();
        CreateDart();
    }

    private void Update() {
        if (gameState != GameState.play) {
            return;
        }

        if (_shootDelayTimeCounter <= _shootDelayTime) {
            _shootDelayTimeCounter -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && _shootDelayTimeCounter <= 0) {
            _shootDelayTimeCounter = _shootDelayTime;
            ShootDart();
        }
    }

    private void ShootDart() {
        if(_currentDart == null) {
            return;
        }
        _currentDart.Move(_spiner, _lsDart);
    }

    private void CreateDart() {
        GameObject newDart = Instantiate(_dartPrefab);
        newDart.transform.position = _spawnPosition;
        newDart.transform.rotation = _spawnRotation;
       _currentDart = newDart.GetComponent<Dart>();
        _lsDart.Add(_currentDart);
        _currentDart.Stop();
    }

    public void Lose() {
        _loseObject.SetActive(true);
        gameState = GameState.gameover;
    }

    private void Retry() {
        _loseObject.SetActive(false);
        gameState = GameState.play;
        _counter = 0;
        _txtCounter.text = _counter.ToString();
        RemoveAllDart();
        CreateDart();
    }

    private void RemoveAllDart() {
        foreach (Dart dart in _lsDart) {
            Destroy(dart);
        }
        _lsDart.Clear();
    }

    private void OnDartHit() {
        _counter++;
        _txtCounter.text = _counter.ToString();
        CreateDart();
    }
}

public enum GameState { play, gameover}