using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{

    public float Delay = 5.0f;

    public delegate void ExitAction();

    public static event ExitAction OnTileExited;

    private bool _exited = false;

    private void OnTriggerExit(Collider other)
    {
        CarTag carTag = other.GetComponent<CarTag>();
        if(carTag != null )
        {
            if (!_exited)
            {
                _exited = true;
                OnTileExited();
                StartCoroutine(WaitAndDeactivate());
            }
        }
    }

    IEnumerator WaitAndDeactivate()
    {
        yield return new WaitForSeconds(Delay);

        transform.root.gameObject.SetActive(false);
    }
}
