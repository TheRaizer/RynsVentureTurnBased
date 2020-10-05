using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeTextUp : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private float timeBeforeFade = 0.5f;
    private float timerForFade;
    private Vector3 dir;
    private TextMeshProUGUI text;

    private void Awake()
    {
        timerForFade = timeBeforeFade;
        text = GetComponent<TextMeshProUGUI>();
        dir = new Vector3(0, speed, 0);
    }

    void Update()
    {
        if (timerForFade > 0)
        {
            timerForFade -= Time.deltaTime;
        }

        if(text.alpha > 0 && timerForFade <= 0)
        {
            text.alpha -= fadeSpeed * Time.deltaTime;
            transform.Translate(dir);
        }
        if(text.alpha <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        text.alpha = 1f;
        timerForFade = timeBeforeFade;
    }
}
