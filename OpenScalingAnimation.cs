using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiftMenu
{

    public class OpenScalingAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            Debug.Log("PrintOnEnable: script was enabled");

            StartCoroutine("ScaleUpAndDown");
        }


        IEnumerator ScaleUpAndDown()
        {
            Vector3 initialScale = gameObject.transform.localScale;
            Vector3 upScale = new Vector3(initialScale.x + 0.1f, initialScale.y + 0.1f, initialScale.z + 0.1f);
            float duration = 0.1f;

            for (float time = 0; time < duration * 2; time += Time.deltaTime)
            {
                  float progress = Mathf.PingPong(time, duration) / duration;

                //float progress = time / duration;

                gameObject.transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
                yield return null;
            }
            transform.localScale = initialScale;
        }

    } // class ends here


} // name space ends here