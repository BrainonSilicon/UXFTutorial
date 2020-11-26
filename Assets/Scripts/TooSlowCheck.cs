using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class TooSlowCheck : MonoBehaviour
{
    public AudioClip failSound;
    public Session session;

    public void BeginCountdown()
    {
        StartCoroutine(Countdown());
    }

    public void StopCountdown()
    {
        StopAllCoroutines();
    }

    // timeout_period is from part 9 and 10 and needs to be added AFTER the .json file

    IEnumerator Countdown()
    {   
        float timeoutPeriod = session.CurrentTrial.settings.GetFloat("timeout_period");
        yield return new WaitForSeconds(timeoutPeriod);

        // if we got to this stage, that means we moved too slow
        session.CurrentTrial.result["outcome"] = "tooslow";
        session.EndCurrentTrial();

        // we will play a clip at position above origin, 100% volume
        AudioSource.PlayClipAtPoint(failSound, new Vector3(0, 1.3f, 0), 1.0f);
    }
}
