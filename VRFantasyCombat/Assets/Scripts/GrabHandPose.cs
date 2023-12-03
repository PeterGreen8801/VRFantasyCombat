using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using Weblication.XR.Grabbable;

public class GrabHandPose : MonoBehaviour
{
    #region Variables

    public float poseTransitionDuration = 0.2f;
    public HandData leftHandPose;
    public HandData rightHandPose;

    private Vector3 _startingHandPosition;
    private Vector3 _finalHandPosition;
    private Quaternion _startingHandRotation;
    private Quaternion _finalHandRotation;

    private Quaternion[] _startingFingerRotations;
    private Quaternion[] _finalFingerRotations;

    #endregion

    #region Overrides


    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(SetupPose);
            grabInteractable.selectExited.AddListener(UnSetPose);
        }

        if (rightHandPose != null)
            rightHandPose.gameObject.SetActive(false);

        if (leftHandPose != null)
            leftHandPose.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredRotation = poseUsedToMirror.root.localRotation;
        mirroredRotation.y *= -1;
        mirroredRotation.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredRotation;

        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        Transform hand = null;

        if (args.interactorObject is XRDirectInteractor)
            hand = args.interactorObject.transform;
        else
        {
            if (args.interactorObject is XRRayInteractor)
                hand = args.interactorObject.transform.parent;
        }

        if (hand != null)
        {
            HandData handData = hand.GetComponentInChildren<HandData>(true);
            handData.animator.enabled = false;

            if (handData.handType == HandData.HandModelType.Right)
                SetHandDataValues(handData, rightHandPose);
            else
                SetHandDataValues(handData, leftHandPose);

            StartCoroutine(SetHandDataRoutine(handData,
                                                    _finalHandPosition,
                                                    _finalHandRotation,
                                                    _finalFingerRotations,
                                                    _startingHandPosition,
                                                    _startingHandRotation,
                                                    _startingFingerRotations));
        }
    }


    public void UnSetPose(BaseInteractionEventArgs args)
    {
        Transform hand = null;

        if (args.interactorObject is XRDirectInteractor)
            hand = args.interactorObject.transform;
        else
        {
            if (args.interactorObject is XRRayInteractor)
                hand = args.interactorObject.transform.parent;
        }

        if (hand != null)
        {
            HandData handData = hand.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            SetHandDataImmediate(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotations);
        }
    }


    public void SetHandDataValues(HandData h1, HandData h2)
    {
        _startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
                                           h1.root.localPosition.y / h1.root.localScale.y,
                                           h1.root.localPosition.z / h1.root.localScale.z);

        _finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
                                           h2.root.localPosition.y / h2.root.localScale.y,
                                           h2.root.localPosition.z / h2.root.localScale.z);

        _startingHandRotation = h1.root.localRotation;
        _finalHandRotation = h2.root.localRotation;

        _startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        _finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            _startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            _finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }


    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < h.fingerBones.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    #endregion

    #region Private Functions


    private IEnumerator SetHandDataRoutine(HandData h,
                                            Vector3 newPosition,
                                            Quaternion newRotation,
                                            Quaternion[] newBonesRotation,
                                            Vector3 startingPosition,
                                            Quaternion startingRotation,
                                            Quaternion[] startingBonesRotation)
    {
        float timer = 0;

        while (timer <= poseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for (int i = 0; i < h.fingerBones.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i],
                                                                    newBonesRotation[i],
                                                                    timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;

            yield return null;

        }
    }

    private void SetHandDataImmediate(HandData h,
                                            Vector3 newPosition,
                                            Quaternion newRotation,
                                            Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < h.fingerBones.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    #endregion
}
