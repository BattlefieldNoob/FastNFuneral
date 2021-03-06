using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Grabber : MonoBehaviour
{
    [SerializeField] private LinkPointCollisionHandler _linkPointCollisionHandler;

    [SerializeField] private Vector3 _handDeltaPosition;

    private Grabbable GrabCandidate;

    private InteractableShelf InteractableShelfSelected;

    private Grabbable Grabbed;

    private LinkPoint LinkCandidate => _linkPointCollisionHandler.LinkCandidate;

    private Collider _collider;

    private Transform cameraTransform;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _linkPointCollisionHandler.Disable();
        cameraTransform = Camera.main.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("LinkPoint") && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Grabbable") && Grabbed == null)
        {
            // Debug.Log($"Collided with {other.attachedRigidbody.name}");
            // Debug.Log($"Model {other.name}");
            var newGrabbable = other.attachedRigidbody.GetComponent<Grabbable>();

            if (newGrabbable == null)
                return;

            if (GrabCandidate != null && GrabCandidate != newGrabbable)
            {
                var cameraPosition = cameraTransform.position;
                var actualCandidateDistance = Vector3.Distance(cameraPosition, GrabCandidate.transform.position);

                var newCandidateDistance = Vector3.Distance(cameraPosition, newGrabbable.transform.position);

                if (actualCandidateDistance < newCandidateDistance)
                {
                    // Debug.Log("ignoring!");
                    //dont change the actual candidate!
                    return;
                }

                GrabCandidate.DoNotHighLight();
                GrabCandidate = null;
            }

            GrabCandidate = newGrabbable;
            GrabCandidate.HighLight();
            //Debug.Log("Collided with grabbable!");
        }

        if (other.CompareTag("InteractableShelf"))
        {
            var interactable = other.GetComponent<InteractableShelf>();

            if (interactable == null)
                return;

            if (InteractableShelfSelected != null)
            {
                InteractableShelfSelected.DoNotHighLight();
            }

            InteractableShelfSelected = interactable;
            InteractableShelfSelected.HighLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("LinkPoint") && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Grabbable"))
        {
            
            Debug.Log($"Removing with {other.attachedRigidbody.name}");
            Debug.Log($"Removing with {other.name}");
            if (GrabCandidate == null)
                return;
            
            var toremove = other.attachedRigidbody.GetComponent<Grabbable>();
            if (toremove != null)
            {
                if (!toremove.Equals(GrabCandidate))
                {
                    Debug.Log("JUST!");
                    //just remove highlight
                    toremove.DoNotHighLight();
                    return;
                }
            }
            
            GrabCandidate.DoNotHighLight();
            GrabCandidate = null;
        }

        if (other.CompareTag("InteractableShelf"))
        {
            if (InteractableShelfSelected == null)
                return;

            InteractableShelfSelected.DoNotHighLight();

            InteractableShelfSelected = null;
        }
    }


    private void Grab()
    {
        var linkable = GrabCandidate.GetComponent<Linkable>();
        if (linkable != null)
        {
            linkable.Unlink();
        }

        Grabbed = GrabCandidate;
        GrabCandidate = null;
        Grabbed.Grabbed();
        Grabbed.transform.SetParent(transform);
        Grabbed.transform.localRotation = Quaternion.identity;
        Grabbed.transform.localPosition = Vector3.zero + _handDeltaPosition;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundFX/SFX_Grab");
        EventManager.Instance.OnGrab.Invoke();
        _collider.enabled = false;
        _linkPointCollisionHandler.Enable();
    }

    private void ReleaseAndLink()
    {
//        Debug.Log("ReleaseAndLink");
        var linkable = Grabbed.GetComponent<Linkable>();
        if (linkable == null)
        {
            Debug.Log("Cannot find linkable!");
            ReleaseAndThrow();
            return;
        }

        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        Grabbed = null;
        EventManager.Instance.OnRelease.Invoke();
        _collider.enabled = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundFX/SFX_Attach");


        linkable.LinkTo(LinkCandidate);
        _linkPointCollisionHandler.Disable();
    }

    private void ReleaseAndThrow()
    {
        var chargeAmount = Time.time - throwChargeStart;

        chargeAmount = Mathf.Clamp(chargeAmount, 0, 2);

        Grabbed.transform.SetParent(null);
        Grabbed.Released();
        _collider.enabled = true;
        EventManager.Instance.OnRelease.Invoke();
        _collider.enabled = true;

        //Debug.Log("Throw distance:"+chargeAmount);
        if (chargeAmount > 0.15f)
        {
            //Throw!!!!
            Grabbed.ApplyForce(transform.forward * chargeAmount);
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundFX/SFX_Throw");

        Grabbed = null;
        throwChargeStart = 0;
    }

    private void Update()
    {
        if (inThrowCharge && Input.GetMouseButtonUp(0))
        {
            ReleaseAndThrow();
            inThrowCharge = false;
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        if (InteractableShelfSelected != null && GrabCandidate == null && Grabbed is null)
        {
            InteractableShelfSelected.Toggle();
        }
        else if (GrabCandidate != null && Grabbed is null)
        {
            Grab();
        }
        else if (Grabbed != null && LinkCandidate != null && LinkCandidate.GetComponentInParent<Grabber>() == null)
        {
            //Debug.Log("Release And Link!");
            ReleaseAndLink();
        }
        else if (Grabbed != null)
        {
            //Debug.Log("Release And Throw!");
            StartThrowCharge();
        }
    }

    private float throwChargeStart = 0;
    private bool inThrowCharge;

    private void StartThrowCharge()
    {
        throwChargeStart = Time.time;
        inThrowCharge = true;
    }
}