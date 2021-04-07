using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    [SerializeField] List<MYthsAndSteel_Enum.EventCard> _cardObtain = new List<MYthsAndSteel_Enum.EventCard>();

    [SerializeField] private Player _redPlayerInfos = new Player();
    public Player RedPlayerInfos => _redPlayerInfos;
    [SerializeField] private Player _bluePlayerInfos = new Player();
    public Player BluePlayerInfos => _bluePlayerInfos;
    [Space]

    [SerializeField] private UnitReference _unitRef = null;
    public UnitReference UnitRef => _unitRef;
    [Space]

    //Liste des unit�s d�sactiv�es
    public List<MYthsAndSteel_Enum.TypeUnite> DisactivateUnitType = new List<MYthsAndSteel_Enum.TypeUnite>();
    
    [SerializeField] bool _ArmyRedWinAtTheEnd;
    public bool ArmyRedWinAtTheEnd => _ArmyRedWinAtTheEnd;

    [Header("Cartes events")]
    [SerializeField] private EventCardList _eventCardList = null;
    public EventCardList EventCardList => _eventCardList;

    private void Start(){
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamBluePlayer, 2);
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamRedPlayer, 1);
    }

    #region DesactivationUnitType
    /// <summary>
    /// D�sactive un type d'unit�
    /// </summary>
    /// <param name="DesactiveUnit"></param>
    public void DesactivateUnitType(MYthsAndSteel_Enum.TypeUnite DesactiveUnit)
    {
        DisactivateUnitType.Add(DesactiveUnit);
    }


    /// <summary>
    /// active tous les types d'unit�s
    /// </summary>
    public void ActivateAllUnitType()
    {
        DisactivateUnitType.Clear();
    }
    #endregion DesactivationUnitType

    #region CarteEvent
    /// <summary>
    /// Ajoute une carte event random au joueur
    /// </summary>
    /// <param name="player"></param>
    [EasyButtons.Button]
    public void GiveEventCard(int player)
    {
        if(_cardObtain.Count < EventCardList._eventSO.NumberOfEventCard){
            int randomCard = UnityEngine.Random.Range(0, EventCardList._eventSO.NumberOfEventCard);

            MYthsAndSteel_Enum.EventCard newCard = EventCardList._eventSO.EventCardList[randomCard]._eventType;

            if(_cardObtain.Contains(newCard))
            {
                GiveEventCard(player);
                return;
            }

            AddEventCard(player, newCard);
            _cardObtain.Add(newCard);
        }
        else{
            Debug.Log("Il n'y a plus de cartes events");
        }
    }

    /// <summary>
    /// Ajoute une carte sp�cifique au joueur
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void AddEventCard(int player, MYthsAndSteel_Enum.EventCard card)
    {
        if(player == 1){
            EventCardList._eventCardRedPlayer.Insert(1, card);
        }
        else if(player == 2){
            EventCardList._eventCardBluePlayer.Insert(1, card);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }

        CreateEventCard(player, card);
    }

    /// <summary>
    /// Ajoute la carte event au canvas
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void CreateEventCard(int player, MYthsAndSteel_Enum.EventCard card){
        GameObject newCard = Instantiate(player == 1? UIInstance.Instance.EventCardObjectRed : UIInstance.Instance.EventCardObjectBlue,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0).transform.position : UIInstance.Instance.BluePlayerEventtransf.GetChild(0).transform.position,
                                         Quaternion.identity,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0) : UIInstance.Instance.BluePlayerEventtransf.GetChild(0));

        EventCard newEventCard = new EventCard();
        foreach(EventCard evC in EventCardList._eventSO.EventCardList){
            if(evC._eventType == card){
                newEventCard = evC;
            }
        }
        newCard.GetComponent<EventCardContainer>().AddEvent(newEventCard);

        AddEventToButton(card, newCard);

        if(player == 1){
            EventCardList._eventGamRedPlayer.Insert(1, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamRedPlayer, 1);
        }
        else if(player == 2){
            EventCardList._eventGamBluePlayer.Insert(1, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamBluePlayer, 2);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }
    }

    public void AddEventToButton(MYthsAndSteel_Enum.EventCard card, GameObject cardGam){
        switch(card)
        {
            case MYthsAndSteel_Enum.EventCard.Activation_de_nodus:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchActivationDeNodus);
                break;

            case MYthsAndSteel_Enum.EventCard.Armes_perforantes:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchArmesPerforantes);
                break;

            case MYthsAndSteel_Enum.EventCard.Arme_�pid�miologique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchArmeEpidemiologique);
                break;

            case MYthsAndSteel_Enum.EventCard.Bombardement_a�rien:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchBombardementAerien);
                break;

            case MYthsAndSteel_Enum.EventCard.Cessez_le_feu:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchCessezLeFeu);
                break;

            case MYthsAndSteel_Enum.EventCard.D�ploiement_acc�l�r�:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchD�ploiementAcc�l�r�);
                break;

            case MYthsAndSteel_Enum.EventCard.D�tonation_d_orgone:
                break;

            case MYthsAndSteel_Enum.EventCard.Entra�nement_rigoureux:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchEntrainementRigoureux);
                break;

            case MYthsAndSteel_Enum.EventCard.Fil_barbel�:
                break;

            case MYthsAndSteel_Enum.EventCard.Illusion_strat�gique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchIllusionStrat�gique);
                break;

            case MYthsAndSteel_Enum.EventCard.Manoeuvre_strat�gique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.ManoeuvreStrat�gique);
                break;

            case MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.OptimisationOrgone);
                break;

            case MYthsAndSteel_Enum.EventCard.Paralysie:
                break;

            case MYthsAndSteel_Enum.EventCard.Pillage_orgone:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchPillageOrgone);
                break;

            case MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimis�s:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchPointeursLaserOptimis�s);
                break;

            case MYthsAndSteel_Enum.EventCard.Reprogrammation:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReproggramation);
                break;

            case MYthsAndSteel_Enum.EventCard.R�approvisionnement:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReapprovisionnement);
                break;

            case MYthsAndSteel_Enum.EventCard.Sabotage:
                break;

            case MYthsAndSteel_Enum.EventCard.S�rum_exp�rimental:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchSerumExperimental);
                break;

            case MYthsAndSteel_Enum.EventCard.Transfusion_d_orgone:
                break;
        }
    }

    #endregion CarteEvent




    /// <summary>
    /// Est ce qu'il reste des unit�s dans l'arm�e du joueur
    /// </summary>
    /// <param name="Joueur"></param>
    /// <returns></returns>
    public bool CheckArmy(UnitScript unit, int Joueur){
        if(Joueur == 1){
            if(unit.UnitSO.IsInRedArmy){
                return true;
            }
            return false;
        }
        else{
            if(unit.UnitSO.IsInRedArmy){
                return false;
            }
            return true;
        }
    }
}

/// <summary>
/// Toutes les infos li�es aux cartes events
/// </summary>
[System.Serializable]
public class EventCardList
{
    public EventCardClass _eventSO = null;

    //Carte Event du Joueur 1
    public List<MYthsAndSteel_Enum.EventCard> _eventCardRedPlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Event du Joueur 2
    public List<MYthsAndSteel_Enum.EventCard> _eventCardBluePlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Gam du Joueur 1
    public List<GameObject> _eventGamRedPlayer = null;

    //Carte Gam du Joueur 2
    public List<GameObject> _eventGamBluePlayer = null;
}