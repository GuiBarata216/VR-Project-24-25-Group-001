using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BUttonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;

    private Vector3 offset;
    private Transform pokeAttachTransform;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool isFollowing = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor interactor = (UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)hover.interactorObject;
            isFollowing = true;

            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isFollowing) 
        {
            visualTarget.position = pokeAttachTransform.position + offset;
        }
    }
}
