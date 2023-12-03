using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    #region Constants

    const string TAG_LEFTHAND = "Left Hand";
    const string TAG_RIGHTHAND = "Right Hand";

    #endregion

    #region Variables

    [Header("Left Hand")]
    public Transform leftAttachTransform;

    [Header("Right Hand")]
    public Transform rightAttachTransform;

    private Vector3 _initialLocalPos;
    private Quaternion _initialLocalRot;

    #endregion

    #region Overrides


    void Start()
    {
        if (!attachTransform)
        {
            GameObject attachPoint = new("Offset Grab Pivot");

            attachPoint.transform.SetParent(transform, false);
            attachTransform = attachPoint.transform;
        }
        else
        {
            _initialLocalPos = attachTransform.localPosition;
            _initialLocalRot = attachTransform.localRotation;
        }
    }

    #endregion

    #region Event Handlers


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject.transform.CompareTag(TAG_LEFTHAND))
        {
            if (leftAttachTransform != null)
                attachTransform = leftAttachTransform;
        }
        else
        {
            if (args.interactorObject.transform.CompareTag(TAG_RIGHTHAND))
            {
                if (rightAttachTransform != null)
                    attachTransform = rightAttachTransform;
            }
        }

        if ((leftAttachTransform == null) || (rightAttachTransform == null))
        {
            if (args.interactorObject is XRDirectInteractor)
            {
                attachTransform.position = args.interactorObject.transform.position;
                attachTransform.rotation = args.interactorObject.transform.rotation;
            }
            else
            {
                attachTransform.localPosition = _initialLocalPos;
                attachTransform.localRotation = _initialLocalRot;
            }
        }

        selectEntered?.Invoke(args);
    }

    #endregion

    #region Public Properties

    public bool BeingHeld
    {
        get { return isSelected; }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Functions

    #endregion
}

