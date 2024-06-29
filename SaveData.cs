using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sauvegarde_donnée;

public class SaveData
{
    //une reference pour avoir accès au position d'un Joueur
    [Header("References")]
    [SerializeField]
    // pour avoir les axes d'un joueur sur x,y,z;
    private Transform player;
    
    [SerializeField]
    private Equipement equipement;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Buildsystem buidSystem;

    public void save(){
        if(Input.GetKeyDown(KeyCode.F1))
        {
            dataSave();
        }

        if(Input.GetKeyDown(KeyCode.F9)){
            loadGame();
        }
    }
    public void dataSave(){
        GameData save=new GameData{
            Playerposition=player.position,
            nombreOutils=Inventory.instance.GetContent(),
            equipedHeadItem=equipement.equipedHeadItem,
            equipedHandsItem=equipement.equipedHandsItem,
            equipedChestItem=equipement.equipedChestItem,
            equipedFeetItem=equipement.equipedFeetItem,
            equipedLegsItem=equipement.equipedLegsItem,
            equipedWeaponItem=equipement.equipedWeaponItem,
            currentHealth=playerStats.currentHealth,
            currentHunger=playerStats.currentHunger,
            currentThirst=playerStats.currentThirst,
            placedStructures=buidSystem.placedStructures.ToArray(),
        };
 
        string jsonData=JsonUtility.ToJson(save);

        //sur ce script il cree l'emplacement du fichier,peu importe la version du jeux ce fichier sera compatible
        string file=Application.persistentDataPath + "/DataSave.json";
        // Debug.Log(file); 
        System.IO.WriteAllText(file,jsonData);
        Debug.Log("Sauvegarde effectuée");
    }

    public void loadGame(){
          string file=Application.persistentDataPath + "/DataSave.json";
          string json=System.IO.ReadAllText(file);
          GameData save=JsonUtility.FromJson<GameData>(json);

          Debug.Log("chargement des données");

          player.position=save.Playerposition;

          equipement.LoadEquipements(new ItemData[] {
          save.equipedHeadItem,
          save.equipedChestItem,
          save.equipedHandsItem,
          save.equipedLegsItem,
          save.equipedWeaponItem,
          save.equipedFeetItem
          });

          Inventory.instance.LoadData(save.nombreOutils);

          playerStats.currentHealth=save.currentHealth;
          playerStats.currentHunger=save.currentHunger;
          playerStats.currentThirst=save.currentThirst;
          //Qui est public->
          playerStats.UpdateHealthBarFill();

          buidSystem.LoadStructures(save.placedStructures);

          Debug.Log("chargement Terminés");
    }
}

public class GameData{ 
    public Vecteur3 Playerposition;
    public List<ItemInInventory> nombreOutils;
    public ItemData equipedHeadItem;
    public ItemData equipedChestItem;
    public ItemData equipedHandsItem;
    public ItemData equipedLegsItem;
    public ItemData equipedFeetItem;
    public ItemData equipedWeaponItem;
    public float currentHealth;
    public float currentHunger;
    public float currentThirst;
    public PlacedStructure[] placedStructures;
}