using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : XRGrabInteractable
{

    [Header("Scanner Data")] 
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private GameObject highlight;
    [Space]
    [SerializeField] private TextMeshProUGUI targetName;
    [SerializeField] private TextMeshProUGUI targetPosition;
    
    private AudioSource sound;
    private Animator animator;
    private bool canCast;
    

    
    protected override void Awake()
    {
        base.Awake();
        sound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        ScunnerActivated(false);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (canCast)
        {
            RaycastHit hit;
            if (Physics.Raycast(laserLine.transform.position, laserLine.transform.TransformDirection(Vector3.forward), out hit))
            {
                if (hit.collider != null)
                {
                    targetName.text = "Name: "+ hit.collider.name; 
                    targetPosition.text =  "Position: "+ hit.collider.transform.position;
                    laserLine.SetPosition(1,new Vector3(0,0,hit.distance));
                    laserLine.endColor = Color.green;
                    laserLine.startColor = Color.green;
                }
            }
            else
            {
                laserLine.SetPosition(1,new Vector3(0,0,50));
                laserLine.endColor = Color.red;
                laserLine.startColor = Color.red;
                targetName.text = "";
                targetPosition.text = "";
            }
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        sound.Play();
        base.OnHoverEntered(args);
        highlight.SetActive(true);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        highlight.SetActive(false);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        sound.Play();
        base.OnSelectEntered(args);
        animator.SetBool("Opened",true);
        highlight.SetActive(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        animator.SetBool("Opened",false);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        ScunnerActivated(true);
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        ScunnerActivated(false);
    }

    private void ScunnerActivated(bool _isActive)
    {
        laserLine.enabled = _isActive;
        targetName.gameObject.SetActive(_isActive);
        targetPosition.gameObject.SetActive(_isActive);
        canCast = _isActive;
    }
}
