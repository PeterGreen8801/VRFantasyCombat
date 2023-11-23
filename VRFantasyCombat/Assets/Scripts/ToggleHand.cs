using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleHand : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideHand);
        grabInteractable.selectExited.AddListener(ShowHand);
    }

    public void HideHand(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Left Hand")
        {
            leftHand.SetActive(false);
        }
        else if (args.interactorObject.transform.tag == "Right Hand")
        {
            rightHand.SetActive(false);
        }
    }

    public void ShowHand(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Left Hand")
        {
            leftHand.SetActive(false);
        }
        else if (args.interactorObject.transform.tag == "Right Hand")
        {
            rightHand.SetActive(true);
        }


    }

}
