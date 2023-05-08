using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BalloonInflator : XRGrabInteractable
{
    [Header("Balloon Data")] 
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Balloon balloonPrefab;

    private XRBaseController m_Controller;
    private Balloon m_BalloonInstance;
    private bool deatched;
    private bool canInstantiate;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);
        var _controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = _controllerInteractor.xrController;
        
        deatched = false;
        canInstantiate = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (isSelected & m_Controller != null)
        {
            if (m_BalloonInstance != null)
            {
                m_BalloonInstance.transform.localScale = (Vector3.one) + m_Controller.activateInteractionState.value * (Vector3.one * 6);
                if (m_BalloonInstance.transform.localScale.x >= 5.9f)
                {
                    m_BalloonInstance.Detach();
                    m_BalloonInstance = null;
                    deatched = true;
                }
            }

            if (m_Controller.activateInteractionState.value > 0.1f)
            {
                if (m_BalloonInstance == null & canInstantiate)
                {
                    m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);
                    canInstantiate = false;
                }
            }  
            
            if (m_Controller.activateInteractionState.value < 0.1f)
            {
                if (deatched)
                {
                    deatched = false;
                    canInstantiate = true;
                }
            }
                
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if(m_BalloonInstance != null)
            Destroy(m_BalloonInstance.gameObject);
    }
}
