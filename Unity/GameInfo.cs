/************************************************************************************
 * 								GAME INFO + SAVE                        			*
 * This script will have all the important information on the game, shared 			*
 * variables between scenes, scores, etc... Also There are implemented methods 		*
 * that will allow developers to save safely games.  								*
 *																					*
 *                                													*
 *  All the variables that should be saved must be declared on both GameInfo and	*
 * PlayerData Classes.                                                              *
 * 																					*
 * 																					*
 * 		   		Script created on September 2014 by Aram Rodriguez Gonzalez			*
 * 								               mail: aram_rodriguez@msn.com			*
 * 																					*
 * **********************************************************************************/




using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour {
	// Static Variable that contains an object of this class, accessible from anywhere from the code in the whole project
	public static GameInfo WordInfo;

	// Two arrays that will contain all the infos about the variables on this class and PlayerData ones.
	FieldInfo[] propertyInfos,propertyInfosAux;

	// With this byte you could manage several save slots.
	public byte CURRENT_GAME_ID = 1;



	#region Declare your shared Variables here ( they will be accessible from the whole project)

	#endregion

	#region Variables that should be saved ( duplicate this ones on the PlayerPrefs Class)

	#endregion

	// function awake is call before the game is launched
	void Awake()
	{
		// If there is not one previous object of this class
		if(WordInfo == null)
		{
			// I ask the engine not to destroy this object between scenes
			DontDestroyOnLoad(gameObject);
			// And also I say my script this will be my WorldInfo Object.
			WordInfo = this;
		}
		// If there one previous object of this class, and is not the same of this
		else if(WordInfo != this)
		{
			// Destroy the gameObject
			Destroy(gameObject);
		}
		
		
	}
	/**
	 * This function is call just after the game is launched
	 */
	void Start(){

		// I set my propertyInfos variable to get all the info of the variables of this class
		propertyInfos = typeof(GameInfo).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		// and my auxiliar variable to get all the info of the variables of the playerData class
		propertyInfosAux = typeof(PlayerData).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

		DataLoad(CURRENT_GAME_ID);
	}
	
	/**
	 * This function will save all the variables listed on the PlayerData Class
	 *@param GameId Id of the slot to save on 
	 */
	public void DataSave(byte GameId)
	{
		// new binary formatter
		BinaryFormatter bf = new BinaryFormatter();
		// Creation of the file in which we are going to save (if there is another file with the same name it will be overwriten)
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo" + GameId + ".dat");

		// I create an object of PlayerData Class, which is going to be saved.
		PlayerData data = new PlayerData();
		
		
		// Foreach variable on both classes
		foreach(FieldInfo propertyInfo in propertyInfos)
		{
			foreach(FieldInfo propertyInfoAux in propertyInfosAux)
			{
				// If there is two with the same name
				if(propertyInfo.Name == propertyInfoAux.Name)
				{
					// I change the value of the variable in the PlayerData class
					propertyInfoAux.SetValue(data,propertyInfo.GetValue(this));
				}
			}
		}
		
		// I serialize the object of the class PlayerData
		bf.Serialize(file,data);
		// I close the edition of the file
		file.Close();
	}
	
	
	/**
	 *This function will load all the variables from a saved file
	 *@param GameId Id of the slot to load of
	 */
	public void DataLoad(byte GameId)
	{
		//Check if the file we are trying to acess exists
		if(File.Exists(Application.persistentDataPath + "/playerInfo" + GameId + ".dat"))
		{
			// new binary formater ( to be able to read from binary)
			BinaryFormatter bf = new BinaryFormatter();
			// Open the file on the specific path
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo" + GameId + ".dat",FileMode.Open);
			// Creation of an object of the Class PlayerData, and deserialize the file on it
			PlayerData data = (PlayerData) bf.Deserialize(file);
			// I close the saved file
			file.Close();

			// Foreach variable on both classes
			foreach(FieldInfo propertyInfo in propertyInfos)
			{
				foreach(FieldInfo propertyInfoAux in propertyInfosAux)
				{
					// If there is two with the same name
					if(propertyInfo.Name == propertyInfoAux.Name)
					{
						// I load the value of the variable to the GameInfo class
						propertyInfo.SetValue(this,propertyInfoAux.GetValue(data));
					}
				}
			}
			
		}
	}
	
	/**
	 * This function will erase the saved data on a slot
	 * @param GameId Saved Slot Id
	 */
	public void EraseData(byte GameId)
	{
		// Delete the file on the path
		File.Delete(Application.persistentDataPath + "/playerInfo" + GameId + ".dat");
	}	
	
	
}
// This cleaned Class we will save in our binary files.
[System.Serializable]
class PlayerData
{
	#region Declare all the variables that should be saved here ( also remember to declare them on the GameInfo Class
	
    
    #endregion

}

