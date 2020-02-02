using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullFlapRoutine : MonoBehaviour
{
    public Animator anim;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(GlideRoutine());
            anim.SetTrigger("flap");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator GlideRoutine()
    {
        var direction = Random.Range(0, 2) * 2 - 1;
        var waitTime = Random.Range(1, 5) * Mathf.PI;
        var timer = 0f;
        while (timer < waitTime)
        {
            var perturbance = Mathf.Sin(timer) * direction;
            var xPos = perturbance * -0.3f;
            var yPos = Mathf.Abs(perturbance) * 0.05f;
            var zPos = transform.localPosition.z;
            transform.localPosition = new Vector3(xPos, yPos, zPos);
            transform.eulerAngles = Vector3.back * perturbance * 10;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
    }
}
