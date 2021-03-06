using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    #region Variables
    [Header("ARMY INFO")]
    //nom de l'arm?e
    public string ArmyName;
    public string ArmyNameNomMasc;
    public string ArmyNameNomFem;

    [Header("ACTIVATION")]
    //Nombre d'activation restante
    public int ActivationLeft; 
    //Valeur de la carte activation pos?e
    public int ActivationCardValue; 

    [Header("ORGONE")]
    //Nombre de charges d'orgone actuel
    public int OrgoneValue; 
    //Nombre de pouvoirs d'orgone encore activable
    public int OrgonePowerLeft; 
    //Permet de se souvenir de la derni?re valeur d'orgone avant Update
    public int LastKnownOrgoneValue; 
    //Tile qui correspond au centre de la zone d'Orgone
    public GameObject TileCentreZoneOrgone;
    //Save Le joueur dont la jauge explose
    int PLayerOrgoneExplose; 

    [Header("RESSOURCE")]
    //Nombre de Ressources actuel
    [SerializeField] private int _Ressource;
    public int Ressource
    {
        get
        {
            return _Ressource;
        }
        set
        {
            _Ressource = value;            
            UIInstance.Instance.UpdateRessourceLeft();
        }
    }

    [Header("OBJECTIF")]
    //Nombre d'objectif actuellement captur?
    public int GoalCapturePointsNumber; 

    public bool HasCreateUnit; //est ce que le joueur a cr?er une unit? durant sont tour
    #endregion Variables

    /// <summary>
    /// Check si l'orgone va exploser
    /// </summary>
    /// <returns></returns>
    public bool OrgoneExplose(){
      
        
         
      
        return OrgoneValue > 5 ? true : false;
    }
    
    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou n?gative.</param>
    public void ChangeOrgone(int Value, int player){
        OrgoneValue += Value;
        UpdateOrgoneUI(player);
    }

    public void CheckOrgone(int player){
 
        GameManager.Instance.IsCheckingOrgone = true;
      
        if (OrgoneExplose() && !GameManager.Instance.ChooseUnitForEvent)
        {
            
            GameManager.Instance.DoingEpxlosionOrgone = true;
            List<GameObject> unitList = player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer;
            GameManager.Instance.StartEventModeUnit(4, player == 1 ? true : false, unitList, "Explosion d'orgone", "?tes-vous sur de vouloir infliger des d?g?ts ? ces unit?s?", true);
            GameManager.Instance._eventCall += GiveDamageToUnitForOrgone;
            if(player == 1) GameManager.Instance._eventCallCancel += CancelOrgoneP1;
            else GameManager.Instance._eventCallCancel += CancelOrgoneP2;
            PLayerOrgoneExplose = player;
          
            
        }
        else
        {
            UpdateOrgoneUI(player);


            GameManager.Instance.IsCheckingOrgone = false;
            if(GameManager.Instance._waitToCheckOrgone != null)
            {
                GameManager.Instance._waitToCheckOrgone();
            }
        }
    }

    /// <summary>
    /// When Orgone explode
    /// </summary>
    void GiveDamageToUnitForOrgone(){
        UIInstance.Instance.ActivateNextPhaseButton();

        i = 0;

        GameManager.Instance._waitEvent += DealOrgoneDamageToUnit;
        GameManager.Instance.WaitToMove(0);

        OrgoneValue -= 6;
        UpdateOrgoneUI(PLayerOrgoneExplose);

    }


    int i = 0;
    public void DealDamageToUnit(){
        if(GameManager.Instance.UnitChooseList.Count > i)
        {
            GameManager.Instance.UnitChooseList[i].GetComponent<UnitScript>().TakeDamage(1);
            i++;
            GameManager.Instance._waitEvent -= DealDamageToUnit;
            GameManager.Instance._waitEvent += DealDamageToUnit;
            GameManager.Instance.WaitToMove(.035f);
        }
        else
        {
            GameManager.Instance._waitEvent -= DealDamageToUnit;

            GameManager.Instance.UnitChooseList.Clear();

            GameManager.Instance.IsCheckingOrgone = false;

            if(GameManager.Instance._waitToCheckOrgone != null)
            {
                GameManager.Instance._waitToCheckOrgone();
                Debug.Log("mais non");
            }
        }
    }

    public void DealOrgoneDamageToUnit()
    {
        if (GameManager.Instance.UnitChooseList.Count > i)
        {
            GameManager.Instance.UnitChooseList[i].GetComponent<UnitScript>().TakeDamage(1, true);
            i++;
            GameManager.Instance._waitEvent -= DealOrgoneDamageToUnit;
            GameManager.Instance._waitEvent += DealOrgoneDamageToUnit;
            GameManager.Instance.WaitToMove(.035f);
        }
        else
        {
            GameManager.Instance._waitEvent -= DealOrgoneDamageToUnit;

            GameManager.Instance.UnitChooseList.Clear();

            GameManager.Instance.IsCheckingOrgone = false;

            if (GameManager.Instance._waitToCheckOrgone != null)
            {
                GameManager.Instance._waitToCheckOrgone();
                Debug.Log("mais non");
            }
        }
    }

    /// <summary>
    /// Si le joueur appuie sur le bouton annuler 
    /// </summary>
    void CancelOrgoneP1(){
        CheckOrgone(1);
        UpdateOrgoneUI(1);
    }

    /// <summary>
    /// Si le joueur appuie sur le bouton annuler 
    /// </summary>
    void CancelOrgoneP2()
    {
        CheckOrgone(2);
        UpdateOrgoneUI(2);
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(int player){
        if(player == 1){
            foreach(Image img in OrgoneManager.Instance.RedPlayerCharge){
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++){
                if(i < 5)
                {
                    OrgoneManager.Instance.RedPlayerCharge[i].enabled = true;
                }
            }
        }
        else{
            foreach(Image img in OrgoneManager.Instance.BluePlayerCharge)
            {
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++)
            {
                if(i < 5)
                {
                    OrgoneManager.Instance.BluePlayerCharge[i].enabled = true;
                }
            }
        }
        Debug.Log("Update UI Orgone : " + OrgoneValue);
    }

    //AV
    [HideInInspector] public bool dontTouchThis = false;
}
