using UnityEngine;

public class HandData : MonoBehaviour
{
    #region Variables

    public enum HandModelType { Left, Right };

    public HandModelType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;

    #endregion

    #region Overrides


    #endregion
}
