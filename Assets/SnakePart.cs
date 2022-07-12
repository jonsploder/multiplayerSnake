using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Material snakeHeadMat;
    public Material snakeBodyMat;

    bool isHead = false;
    bool animating = false;
    float scale;
    int calls = 0;
    MoveDirection moveDirection;

    public void SetIsHead(bool head, MoveDirection direction)
    {
        isHead = head;

        if (isHead)
        {
            // StartCoroutine(Unscale());
            GetComponent<Renderer>().material = snakeHeadMat;
            animating = true;
            scale = 0;
            moveDirection = direction;
        }
        else
        {
            GetComponent<Renderer>().material = snakeBodyMat;
        }
    }

    private void Update()
    {
        //var delta = Time.deltaTime;
        //if (animating)
        //{
        //    scale += delta / Time.fixedDeltaTime;
        //    calls += 1;
        //    if (scale > 1)
        //    {
        //        animating = false;
        //        scale = 1;
        //        Debug.Log(calls);
        //    }

        //    // transform.localScale = new Vector3(scale, 1, 1);
        //    var startScale = new Vector3(0, 1, 1);
        //    var endScale = new Vector3(1, 1, 1);
        //    transform.localScale = Vector3.Lerp(startScale, endScale, scale);
        //}
    }

    private IEnumerator Unscale()
    {
        float ratio = 0;
        int calls = 0;
        float duration = 0.1f; // let's say we want a 2s animation
        float start_time = Time.time; // time when the animation began
        Vector3 initial_scale_value = transform.localScale;
        do
        {
            yield return new WaitForEndOfFrame(); // wait for next frame
            ratio = (Time.time - start_time) / duration; // update the ratio value at every frame
            transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), initial_scale_value, ratio); // apply the new scale
            calls += 1;
        } while (ratio < 1);
        Debug.Log(calls);
    }
}
