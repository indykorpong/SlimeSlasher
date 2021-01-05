using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierText : MonoBehaviour {

    public Text textUI;
    public string preText = "";

    private RectTransform rekt;

    private void Awake() {
        rekt = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        GameManager.currMultiplier.AddListener(TextUpdate);
        GameManager.CurrentGameState += GameState;
        ResetLocalScale();
    }

    private void OnDisable() {
        GameManager.currMultiplier.RemoveListener(TextUpdate);
        GameManager.CurrentGameState -= GameState;
        ResetLocalScale();
    }

    private void GameState(GameManager.GameState g) {
        textUI.enabled = (GameManager.GameState.gameStarted == g);
    }

    private int previousVal;
    private bool isFirstCalled = false;
    private void TextUpdate(int val) {
        textUI.text = preText + val.ToString();

        if (!isFirstCalled) {
            isFirstCalled = true;
            return;
        }

        if (val > previousVal) {
            if (OnMultiplierIncreaseRef != null) {
                StopCoroutine(OnMultiplierIncreaseRef);
                ResetLocalScale();
            }
            OnMultiplierIncreaseRef = OnMultiplierIncrease();
            StartCoroutine(OnMultiplierIncreaseRef);
        }
        previousVal = val;
    }

    public float maxSize;
    public float reduceDuration;
    private float _rDuration;
    private Vector3 originalScale = Vector3.one;
    private Vector3 currentVec = Vector3.one;

    private void ResetLocalScale() {
        rekt.localScale = originalScale;
        currentVec = originalScale;
        _rDuration = 0f;
    }

    private IEnumerator OnMultiplierIncreaseRef;
    private IEnumerator OnMultiplierIncrease() {

        originalScale = rekt.localScale;
        currentVec = rekt.localScale;
        _rDuration = reduceDuration;

        float xMax = originalScale.x * maxSize;
        float yMax = originalScale.y * maxSize;
        float zMax = originalScale.z * maxSize;

        float xMin = originalScale.x;
        float yMin = originalScale.y;
        float zMin = originalScale.z;

        while (_rDuration > 0) {
            _rDuration -= Time.unscaledDeltaTime;

            currentVec.x = Mathf.Lerp(xMin, xMax, _rDuration / reduceDuration);
            currentVec.y = Mathf.Lerp(yMin, yMax, _rDuration / reduceDuration);
            currentVec.z = Mathf.Lerp(zMin, zMax , _rDuration / reduceDuration);

            rekt.localScale = currentVec;

            yield return null;
        }

        ResetLocalScale();
    }

}
