using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class used to handles saves.
/// </summary>
public class SaveController : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _levelAvailables;
	public List<GameObject> Levels { get => _levelAvailables; }

	/// <summary>
	/// Loaded level.
	/// </summary>
	public Level LoadedLevel { get; set; }

	/// <summary>
	/// Loaded save file.
	/// </summary>
	public SaveFile SaveFile { get; private set; }

	/// <summary>
	/// Binary formatter used.
	/// </summary>
	private BinaryFormatter _binaryFormatter;

	/// <summary>
	/// Game save path.
	/// </summary>
	private string _gameSavePath;


	/// <summary>
	/// Does this component is initialized?
	/// </summary>
	public bool Initialized { get; private set; } = false;
	
	/// <summary>
	/// Level chosen.
	/// </summary>
	public int LevelIndex { get; set; }

	public bool Hard { get; set; }


	public GameObject CurrentLevel { get => Levels[LevelIndex]; }


	/// <summary>
	/// Awake method, used for initialization.
	/// </summary>
	private void Awake()
	{
		_gameSavePath = Application.persistentDataPath + "/player.dat";
		_binaryFormatter = new BinaryFormatter();

		RecoverSave();
	}


	/// <summary>
	/// Method used to recover save.
	/// </summary>
	private void RecoverSave()
	{
		if (File.Exists(_gameSavePath))
		{
			try
			{
				FileStream file = File.OpenRead(_gameSavePath);
				SaveFile = (SaveFile)_binaryFormatter.Deserialize(file);
				file.Close();
			}
			catch
			{
				ResetData();
			}

			if (SaveFile == null)
				ResetData();
		}
		else
			CreateSave();

		Initialized = true;
	}


	/// <summary>
	/// Method used to create a new save if inexistant.
	/// </summary>
	private void CreateSave()
	{
		SaveFile = new SaveFile(1, 1, Levels.Count);

		SaveData();
	}


	/// <summary>
	/// Method used to save music level.
	/// </summary>
	/// <param name="newMusicLevel">The new music level</param>
	public void SaveMusicLevel(float newMusicLevel)
	{
		SaveFile.Music = newMusicLevel;
		SaveData();
	}


	/// <summary>
	/// Method used to save sound level.
	/// </summary>
	/// <param name="newSoundLevel">The new sound level</param>
	public void SaveSoundLevel(float newSoundLevel)
	{
		SaveFile.Sound = newSoundLevel;
		SaveData();
	}


	/// <summary>
	/// Method used to save music mute.
	/// </summary>
	/// <param name="musicMuted">Does the music is muted?</param>
	public void SaveMusicMute(bool musicMuted)
	{
		SaveFile.MusicMuted = musicMuted;
		SaveData();
	}


	/// <summary>
	/// Method used to save sound mute.
	/// </summary>
	/// <param name="soundMuted">Does the sound is muted?</param>
	public void SaveSoundMute(bool soundMuted)
	{
		SaveFile.SoundMuted = soundMuted;
		SaveData();
	}


	/// <summary>
	/// Method used to save music mute.
	/// </summary>
	public void SaveBloom(bool bloom)
	{
		SaveFile.Bloom = bloom;

		SaveData();
	}


	/// <summary>
	/// Method used to save music mute.
	/// </summary>
	public void SaveGrain(bool filmgrain)
	{
		SaveFile.FilmGrain = filmgrain;

		SaveData();
	}


	/// <summary>
	/// Method used to save music mute.
	/// </summary>
	public void SaveChromatic(bool chromatic)
	{
		SaveFile.ChromaticAberration = chromatic;

		SaveData();
	}


	/// <summary>
	/// Method used to save music mute.
	/// </summary>
	public void SaveFullScreen(bool fullScreen)
	{
		SaveFile.FullScreen = fullScreen;

		SaveData();
	}


	/// <summary>
	/// Method used to save a level data.
	/// </summary>
	/// <param name="timeSurvived">Time survived in this level</param>
	/// <param name="win">Does this level was won</param>
	public void SaveLevelData(float timeSurvived, bool win)
	{
		LevelSave buffer = SaveFile.LevelsProgression[LevelIndex];
		buffer.Win = buffer.Win || win;

		if (timeSurvived > buffer.Time)
		{
			buffer.Time = timeSurvived;
			buffer.Hard = Hard;

			if (win && LevelIndex + 1 < Levels.Count && SaveFile.LevelsProgression[LevelIndex + 1].Locked)
				SaveFile.LevelsProgression[LevelIndex + 1] = new LevelSave(false);
		}

		SaveData();
	}


	/// <summary>
	/// Method used to save data in a file.
	/// </summary>
	private void SaveData()
	{
		try
		{
			FileStream file = File.OpenWrite(_gameSavePath);

			_binaryFormatter.Serialize(file, SaveFile);
			file.Close();
		}
		catch
		{
			//Display error
		}
	}


	/// <summary>
	/// Method used to reset data if needed.
	/// </summary>
	public void ResetData()
	{
		if (File.Exists(_gameSavePath))
			File.Delete(_gameSavePath);

		CreateSave();
	}
}