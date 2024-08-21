using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.Soundy;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using MyAudios.Soundy.Sources.Managers.Controllers;
using MyAudios.Soundy.Sources.Managers.Domain.Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace MyAudios.Soundy.Sources.DataBases.Domain.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     NamesDatabaseType of database, of SoundGroupData, used by Soundy
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Soundy/Sound Database", order = 51)]
    public class SoundDatabase : ScriptableObject
    {
        #region Public Variables

        /// <summary> The database name </summary>
        public string DatabaseName;

        /// <summary> The output audio mixer group that all the sounds contained in this database will get routed through when playing </summary>
        public AudioMixerGroup OutputAudioMixerGroup;

        /// <summary> List of all the SoundGroupData sound names that this database contains </summary>
        public List<string> SoundNames = new List<string>();

        /// <summary> List of references to SoundGroupData assets </summary>
        public List<SoundGroupData> Database = new List<SoundGroupData>();

        #endregion

        #region Properties

        /// <summary> Returns TRUE if at least one sound group data has no AudioClip referenced. It helps with a visual issue tracking system in the Editor </summary>
        public bool HasSoundsWithMissingAudioClips
        {
            get
            {
                foreach (SoundGroupData soundGroupData in Database)
                {
                    if (soundGroupData.HasMissingAudioClips)
                        return true;
                }

                return false;
            }
        }

        #endregion

        #region Public Mehtods

        /// <summary> Adds a new entry, a SoundGroupData, to the database. Returns TRUE if the operation was successful </summary>
        /// <param name="data"> SoundGroupData to add to the database </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public bool Add(SoundGroupData data, bool saveAssets)
        {
            if (data == null)
                return false;

            data.DatabaseName = DatabaseName;
            AddObjectToAsset(data);
            SetDirty(saveAssets);

            return true;
        }

        public void RenameSoundGroup(SoundGroupData soundGroupData, string newName, bool isApplyFirstSoundName = false)
        {
            if (Database.Contains(soundGroupData) == false)
                return;

            if (string.IsNullOrEmpty(newName))
                return;

            // if (isApplyFirstSoundName)
            // {
            //     if (soundGroupData.Sounds.Count == 0)
            //         return;
            //     
            //     AssetDatabase.RenameAsset(
            //         AssetDatabase.GetAssetPath(soundGroupData),
            //         soundGroupData.Sounds[0].AudioClip.name);
            //     
            //     return;
            // }
            //
            //AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(soundGroupData), "SoundGroupData_" + newName);
        }
        
        /// <summary> Adds a new entry, a sound name, to the database. Returns a reference to the newly added SoundGroupData </summary>
        /// <param name="soundName"> The sound name </param>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public SoundGroupData Add(string soundName, bool performUndo, bool saveAssets)
        {
            soundName = soundName.Trim();
            string newName = soundName;
            int counter = 0;

            while (Contains(newName))
            {
                counter++;
                newName = soundName + " (" + counter + ")";
            }

            if (performUndo)
                UndoRecord(SoundDataBaseConst.AddItemConst);

            SoundGroupData data = CreateInstance<SoundGroupData>();
            data.DatabaseName = DatabaseName;
            data.SoundName = newName;
            data.name = data.SoundName;
            data.SetDirty(false);

            if (Database == null)
                Database = new List<SoundGroupData>();

            Database.Add(data);
            AddObjectToAsset(data);
            SetDirty(saveAssets);

            return data;
        }

        /// <summary> Returns TRUE if the sound name has been found in the database </summary>
        /// <param name="soundName"> Target sound name to search for </param>
        public bool Contains(string soundName)
        {
            if (Database == null)
            {
                Database = new List<SoundGroupData>();

                return false;
            }

            foreach (SoundGroupData data in Database)
            {
                if (data.SoundName.Equals(soundName))
                    return true;
            }

            return false;
        }

        /// <summary> Returns TRUE if the SoundGroupData has been found in the database </summary>
        /// <param name="soundGroupData"> SoundGroupData to search for </param>
        public bool Contains(SoundGroupData soundGroupData)
        {
            if (soundGroupData != null && Database.Contains(soundGroupData))
                return true;

            return false;
        }

        /// <summary> Iterates through the database to look for the given sound name to return the corresponding SoundGroupData. If the name does not exist, it will return null </summary>
        /// <param name="soundName"> The sound name to search for </param>
        public SoundGroupData GetData(string soundName)
        {
            foreach (SoundGroupData data in Database)
            {
                if (data.SoundName.Equals(soundName))
                    return data;
            }

            return null;
        }

        /// <summary> Looks if this database has the 'No Sound' option. If it does not, it adds it </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void Initialize(bool saveAssets) =>
            RefreshDatabase(false, saveAssets);

        /// <summary> Iterates through the database to look for the data. If found, removes the entry and returns TRUE </summary>
        /// <param name="data"> SoundGroupData to search for </param>
        /// <param name="showDialog"> Should a display dialog be shown before executing the action </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public bool Remove(SoundGroupData data, bool showDialog = false, bool saveAssets = false)
        {
            if (data == null)
                return false;

            if (Contains(data) == false)
                return false;

            for (int i = Database.Count - 1; i >= 0; i--)
            {
                if (Database[i] == data)
                {
                    if (data != null)
                    {
#if UNITY_EDITOR
                        AssetDatabase.RemoveObjectFromAsset(data);
                        DestroyImmediate(data, true);
#endif
                    }

                    Database.RemoveAt(i);

                    break;
                }
            }

            UpdateSoundNames(false);
            SetDirty(saveAssets);

            return true;
        }

        /// <summary> Refreshes the entire database by removing empty, duplicate and unnamed entries, sorting the database and updating the sound names list </summary>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void RefreshDatabase(bool performUndo, bool saveAssets)
        {
            if (performUndo)
                UndoRecord(SoundDataBaseConst.RefreshDatabaseConst);

            bool addedTheNoSoundSoundGroup = AddNoSound();
            RemoveUnreferencedData();
            RemoveUnnamedEntries(false);
            RemoveDuplicateEntries(false);
            bool foundDataWithWrongDatabaseName = CheckAllDataForCorrectDatabaseName(false);
            Sort(false);
            UpdateSoundNames(false);
            SetDirty(saveAssets && (addedTheNoSoundSoundGroup || foundDataWithWrongDatabaseName));
        }

        /// <summary> Removes any entries that have no AudioClip referenced </summary>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void RemoveEntriesWithNoAudioClipsReferenced(bool performUndo, bool saveAssets = false)
        {
            if (performUndo)
                UndoRecord(SoundDataBaseConst.RemovedEntryConst);

            for (int i = Database.Count - 1; i >= 0; i--)
            {
                SoundGroupData data = Database[i];

                if (data.SoundName.Equals(SoundyManagerConstant.NoSound))
                    continue;

                if (data.Sounds == null)
                {
                    Database.RemoveAt(i);

                    continue;
                }

                for (int j = data.Sounds.Count - 1; j >= 0; j--)
                {
                    if (data.Sounds[j] == null)
                        data.Sounds.RemoveAt(j);
                }

                if (data.Sounds.Count == 0)
                    Database.RemoveAt(i);
            }

            SetDirty(saveAssets);
        }

        /// <summary> Removes any duplicate entries found in the database </summary>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void RemoveDuplicateEntries(bool performUndo, bool saveAssets = false)
        {
            if (performUndo)
                UndoRecord(SoundDataBaseConst.RemovedDuplicateEntriesConst);

            Database = Database.GroupBy(data => data.SoundName)
                .Select(n => n.First())
                .ToList();

            SetDirty(saveAssets);
        }

        /// <summary> Removes any entries that have no name </summary>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void RemoveUnnamedEntries(bool performUndo, bool saveAssets = false)
        {
            if (performUndo)
                UndoRecord(SoundDataBaseConst.RemoveEmptyEntriesConst);

            Database = Database.Where(data => !string.IsNullOrEmpty(data.SoundName.Trim())).ToList();
            SetDirty(saveAssets);
        }

        /// <summary> [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects) </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void SetDirty(bool saveAssets) =>
            MyUtils.SetDirty(this, saveAssets);

        /// <summary> Sorts the entire database by sound name </summary>
        /// <param name="performUndo"> Record changes? </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void Sort(bool performUndo, bool saveAssets = false)
        {
            if (performUndo)
                UndoRecord(SoundDataBaseConst.SortDatabaseConst);

            Database = Database.OrderBy(data => data.SoundName).ToList();

            //remove the 'No Sound' entry wherever it is
            SoundGroupData noSoundSoundGroupData = null;

            foreach (SoundGroupData audioData in Database)
            {
                if (!audioData.SoundName.Equals(SoundyManagerConstant.NoSound))
                    continue;

                noSoundSoundGroupData = audioData;
                Database.Remove(audioData);

                break;
            }

            if (noSoundSoundGroupData != null)
                Database.Insert(0, noSoundSoundGroupData); //insert back the 'No Sound' entry at the top

            UpdateSoundNames(false);
            SetDirty(saveAssets);
        }

        /// <summary> Records any changes done on the object after this function </summary>
        /// <param name="undoMessage"> The title of the action to appear in the undo history (i.e. visible in the undo menu) </param>
        public void UndoRecord(string undoMessage) =>
            MyUtils.UndoRecordObject(this, undoMessage);

        /// <summary> Updates the list of sound names found in the database </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public void UpdateSoundNames(bool saveAssets)
        {
#if UNITY_EDITOR
            if (SoundNames == null)
                SoundNames = new List<string>();

            if (Database == null)
                Database = new List<SoundGroupData>();

            AddNoSound();
#endif
            SoundNames.Clear();
            SoundNames.Add(SoundyManagerConstant.NoSound);

            var list = new List<string>();

            foreach (SoundGroupData data in Database)
                list.Add(data.SoundName);

            list.Sort();
            SoundNames.AddRange(list);
            SetDirty(saveAssets);
        }

        #endregion

        #region Private Methods

        /// <summary> Adds the 'No Sound' entry to the database (if it does not exist) </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        private bool AddNoSound(bool saveAssets = false)
        {
            if (Contains(SoundyManagerConstant.NoSound))
                return false;

            if (SoundNames == null)
                SoundNames = new List<string>();

            SoundNames.Add(SoundyManagerConstant.NoSound);
            SoundGroupData data = CreateInstance<SoundGroupData>();
            data.DatabaseName = DatabaseName;
            data.SoundName = SoundyManagerConstant.NoSound;
            data.name = data.SoundName;
            data.SetDirty(false);

            if (Database == null)
                Database = new List<SoundGroupData>();

            Database.Add(data);
            AddObjectToAsset(data);
            SetDirty(saveAssets);

            return true;
        }

        /// <summary>[Editor Only] Adds an object to this asset </summary>
        /// <param name="objectToAdd"> Object that will get added under the asset</param>
        private void AddObjectToAsset(Object objectToAdd) =>
            MyUtils.AddObjectToAsset(objectToAdd, this);

        /// <summary> Checks that all the sound group data entries have the correct database name. Returns TRUE if an inconsistency was found </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        private bool CheckAllDataForCorrectDatabaseName(bool saveAssets)
        {
            bool foundSoundGroupWithWrongDatabaseName = false;

            foreach (SoundGroupData data in Database)
            {
                if (data == null)
                    continue;

                if (data.DatabaseName.Equals(DatabaseName))
                    continue;

                foundSoundGroupWithWrongDatabaseName = true;
                data.DatabaseName = DatabaseName;
                data.SetDirty(false);
            }

            SetDirty(saveAssets);

            return foundSoundGroupWithWrongDatabaseName;
        }

        /// <summary> Removes any unreferenced SoundGroupData from the database </summary>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        private void RemoveUnreferencedData(bool saveAssets = false)
        {
#if UNITY_EDITOR
            Object[] objects =
                AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this)); //load all of the data assets

            if (objects == null)
                return;

            //make sure they are not null
            List<SoundGroupData>
                foundAudioData =
                    objects.OfType<SoundGroupData>().ToList(); //create a temp list of all the found sub assets data

            if (Database == null)
                Database = new List<SoundGroupData>(); //sanity check

            bool save = false;

            //mark true if any sub asset was destroyed
            foreach (SoundGroupData data in foundAudioData)
            {
                if (Database.Contains(data))
                    continue; //reference was FOUND in the list -> continue

                DestroyImmediate(data, true); //reference was NOT FOUND in the list -> destroy the asset
                save = true; //mark true to set as dirty and save
            }

            if (!save)
                return; //if no sub asset was destroyed -> stop here

            SetDirty(saveAssets); //save database
#endif
        }

        #endregion
    }
}