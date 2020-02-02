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
            yield return null;
            var waitTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(waitTime);
            anim.SetTrigger("flap");
        }
        
    }
}
