using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class used to handles saves.
/// </summary>
public class SaveController : MonoBehaviour
{
	/// <summary>
	/// All levels availables.
	/// </summary>
	[SerializeField]
	private List<Level> _levelAvailables;
	public List<Level> Levels { get => _levelAvailables; }

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

	/// <summary>
	/// Difficulty chosen.
	/// </summary>
	public bool Hard { get; set; }

	/// <summary>
	/// Level chosen.
	/// </summary>
	public Level CurrentLevel { get => Levels[LevelIndex]; }



	/// <summary>
	/// Awake method, used for initialization.
	/// </summary>
	private void Awake()
	{
		_gameSavePath = Application.persistentDataPath + "/player.dat";
		_binaryFormatter = new BinaryFormatter();

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
	/// Method used to save that the tutorial has been completed.
	/// </summary>
	public void SaveTutorial()
	{
		SaveFile.Tutorial = true;

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
	/// Method used to save bloom state.
	/// </summary>
	/// <param name="bloom">Bloom state</param>
	public void SaveBloom(bool bloom)
	{
		SaveFile.Bloom = bloom;

		SaveData();
	}


	/// <summary>
	/// Method used to save film grain state.
	/// </summary>
	/// <param name="filmgrain">Film grain state</param>
	public void SaveGrain(bool filmgrain)
	{
		SaveFile.FilmGrain = filmgrain;

		SaveData();
	}


	/// <summary>
	/// Method used to save chromatic aberration state.
	/// </summary>
	/// <param name="chromatic">Chromatic aberration state</param>
	public void SaveChromatic(bool chromatic)
	{
		SaveFile.ChromaticAberration = chromatic;

		SaveData();
	}


	/// <summary>
	/// Method used to save fullscreen state.
	/// </summary>
	/// <param name="fullScreen">Fullscreen state</param>
	public void SaveFullScreen(bool fullScreen)
	{
		SaveFile.FullScreen = fullScreen;

		SaveData();
	}


	/// <summary>
	/// Method used to save a new achievement unlocked.
	/// </summary>
	/// <param name="achievement">The achievement unique id</param>
	public void SaveAchievementProgress(string achievement, int progress, bool increase)
	{
		AchievementProgress achievementProgress = SaveFile.AchievementsProgress.Find(x => x.ID == achievement);

		if (achievementProgress != null)
		{
			if (increase)
				achievementProgress.IncreaseProgress(progress);
			else
				achievementProgress.UpdateProgress(progress);

			if (achievementProgress.Progress >= achievementProgress.ProgressGoal)
				Controller.Instance.AchievementController.TriggerAchievement(achievement);
		}
		else
			SaveFile.AchievementsProgress.Add(new AchievementProgress(achievement, Controller.Instance.AchievementController.Achievements.Find(x => x.ID == achievement).Goal));

		SaveData();
	}


	/// <summary>
	/// Method used to save a new achievement unlocked.
	/// </summary>
	/// <param name="achievementID">The new achievement unique id</param>
	public void SaveAchievement(string achievementID)
	{
		if (!SaveFile.AchievementsUnlocked.Contains(achievementID))
		{
			SaveFile.AchievementsUnlocked.Add(achievementID);

			SaveData();
		}
	}


	/// <summary>
	/// Method called to save if the endless mode is unlocked.
	/// </summary>
	public void SaveEndlessMode()
	{
		SaveFile.EndlessUnlocked = true;

		SaveData();
	}


	/// <summary>
	/// Method called to save the endless mode new high score.
	/// </summary>
	/// <param name="score">The score reached</param>
	public void SaveEndlessModeScore(int score)
	{
		if (SaveFile.EndlessScore < score)
		{
			SaveFile.EndlessScore = score;

			SaveData();
		}
	}


	/// <summary>
	/// Method used to save a level data.
	/// </summary>
	/// <param name="timeSurvived">Time survived in this level</param>
	/// <param name="win">Does this level was won</param>
	public void SaveLevelData(float timeSurvived, bool win)
	{
		LevelSave savedLevel = SaveFile.LevelsProgression[LevelIndex];

		if (win)
		{
			if (savedLevel.State == LevelState.OPENED)
			{
				savedLevel.State = LevelState.WON;

				if (LevelIndex + 1 < Levels.Count)
					SaveFile.LevelsProgression[LevelIndex + 1] = new LevelSave(false);
			}
			else if (savedLevel.State == LevelState.WON && Hard)
				savedLevel.State = LevelState.WONHARD;

			if (LevelIndex == 4 && !Hard)
				Controller.Instance.AchievementController.TriggerAchievement("A-4");
			else if (LevelIndex == 9 && !Hard)
				Controller.Instance.AchievementController.TriggerAchievement("A-13");
			else if (LevelIndex == 14 && !Hard)
				Controller.Instance.AchievementController.TriggerAchievement("A-14");
			else if (LevelIndex == 16 && !Hard)
			{
				Controller.Instance.AchievementController.TriggerAchievement("A-5");
				SaveEndlessMode();
			}

			if (Hard)
			{
				Controller.Instance.AchievementController.TriggerAchievement("A-3");

				bool allHardDone = true;

				foreach(LevelSave levelSave in SaveFile.LevelsProgression)
				{
					if (levelSave.State == LevelState.LOCKED || levelSave.State == LevelState.OPENED || levelSave.State == LevelState.WON)
					{
						allHardDone = false;
						break;
					}
				}

				if (allHardDone)
					Controller.Instance.AchievementController.TriggerAchievement("A-7");
			}
			else
				Controller.Instance.AchievementController.TriggerAchievement("A-1");
		}

		if (Hard && savedLevel.HardTime < timeSurvived)
			savedLevel.HardTime = timeSurvived;
		else if (!Hard && savedLevel.Time < timeSurvived)
			savedLevel.Time = timeSurvived;

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