using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class HarvestScript : MonoBehaviour
{
    UnityEvent onGrab;
    UnityEvent onRelease;

    void Start()
    {
        // Check if we're in VR mode (AutoHand available)
        var autohandGrabbable = gameObject.GetComponent<Autohand.Grabbable>();
        
        if (autohandGrabbable != null)
        {
            // VR Mode - use AutoHand events
            onGrab = autohandGrabbable.onGrab;
            onGrab.AddListener(OnGrab);

            onRelease = autohandGrabbable.onRelease;
            onRelease.AddListener(OnRelease);
        }
        else
        {
            // PC Mode - use PCGrabbableAdapter or create events
            var pcGrabbable = gameObject.GetComponent<PCGrabbableAdapter>();
            if (pcGrabbable == null)
            {
                pcGrabbable = gameObject.AddComponent<PCGrabbableAdapter>();
            }

            onGrab = pcGrabbable.onGrab;
            if (onGrab == null)
            {
                onGrab = new UnityEvent();
                pcGrabbable.onGrab = onGrab;
            }
            onGrab.AddListener(OnGrab);

            onRelease = pcGrabbable.onRelease;
            if (onRelease == null)
            {
                onRelease = new UnityEvent();
                pcGrabbable.onRelease = onRelease;
            }
            onRelease.AddListener(OnRelease);
        }
    }

    private void OnGrab()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnRelease()
    {
        this.gameObject.transform.parent = null;
    }

}
