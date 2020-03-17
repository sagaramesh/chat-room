using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{

    private float nextActionTime = 0.0f;

    [SerializeField]
    private GameObject leftEye, rightEye;

    [SerializeField]
    private Sprite eyeRest, eyeBlink;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextActionTime){
            // Blink randomly in intervals of 1-5 seconds:
            nextActionTime = Time.time + (Random.Range(1, 5));
            StartCoroutine("BlinkAnim");
        }
    }

    IEnumerator BlinkAnim() {
        leftEye.GetComponent<SpriteRenderer>().sprite = eyeBlink;
        rightEye.GetComponent<SpriteRenderer>().sprite = eyeBlink;
        yield return new WaitForSeconds(0.1f);
        leftEye.GetComponent<SpriteRenderer>().sprite = eyeRest;
        rightEye.GetComponent<SpriteRenderer>().sprite = eyeRest;
    }
}
