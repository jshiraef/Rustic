﻿using UnityEngine;
using UnityEditor;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PixelCrushers.DialogueSystem.Articy {
	
	/// <summary>
	/// This class manages articy converter prefs. It allows the converter to save
	/// prefs to EditorPrefs between sessions.
	/// </summary>
	public class ConverterPrefs {

		private const string ArticyProjectFilenameKey = "PixelCrushers.DialogueSystem.ArticyProjectFilename";
		private const string ArticyPortraitFolderKey = "PixelCrushers.DialogueSystem.ArticyPortraitFolder";
		private const string ArticyStageDirectionsAreSequencesKey = "PixelCrushers.DialogueSystem.StageDirectionsAreSequences";
		private const string ArticyFlowFragmentModeKey = "PixelCrushers.DialogueSystem.FlowFragmentMode";
		private const string ArticyOutputFolderKey = "PixelCrushers.DialogueSystem.ArticyOutput";
		private const string ArticyOverwriteKey = "PixelCrushers.DialogueSystem.ArticyOverwrite";
		private const string ArticyConversionSettingsKey = "PixelCrushers.DialogueSystem.ArticyConversionSettings";
		private const string ArticyEncodingKey = "PixelCrushers.DialogueSystem.ArticyEncoding";

		public enum FlowFragmentModes { ConversationGroups, Quests, Ignore }

		public string ProjectFilename { get; set; }
		public string PortraitFolder { get; set; }
		public bool StageDirectionsAreSequences { get; set; }
		public FlowFragmentModes FlowFragmentMode { get; set; }
		public string OutputFolder { get; set; }
		public bool Overwrite { get; set; }
		public ConversionSettings ConversionSettings { get; set; }
		public EncodingType EncodingType { get; set; }

		public Encoding Encoding { get { return EncodingTypeTools.GetEncoding(EncodingType); } }
		
		public ConverterPrefs() {
			ProjectFilename  = EditorPrefs.GetString(ArticyProjectFilenameKey);
			PortraitFolder = EditorPrefs.GetString(ArticyPortraitFolderKey);
			StageDirectionsAreSequences = EditorPrefs.HasKey(ArticyStageDirectionsAreSequencesKey) ? EditorPrefs.GetBool(ArticyStageDirectionsAreSequencesKey) : true;
			FlowFragmentMode = (FlowFragmentModes) (EditorPrefs.HasKey(ArticyFlowFragmentModeKey) ? EditorPrefs.GetInt(ArticyFlowFragmentModeKey) : 0);
			OutputFolder = EditorPrefs.GetString(ArticyOutputFolderKey, "Assets");
			Overwrite = EditorPrefs.GetBool(ArticyOverwriteKey, false);
			ConversionSettings = ConversionSettings.FromXml(EditorPrefs.GetString(ArticyConversionSettingsKey));
			EncodingType = EditorPrefs.HasKey(ArticyEncodingKey) ? (EncodingType) EditorPrefs.GetInt(ArticyEncodingKey) : EncodingType.Default;
		}
		
		public void Save() {
			EditorPrefs.SetString(ArticyProjectFilenameKey, ProjectFilename);
			EditorPrefs.SetString(ArticyPortraitFolderKey, PortraitFolder);
			EditorPrefs.SetBool(ArticyStageDirectionsAreSequencesKey, StageDirectionsAreSequences);
			EditorPrefs.SetInt(ArticyFlowFragmentModeKey, (int) FlowFragmentMode);
			EditorPrefs.SetString(ArticyOutputFolderKey, OutputFolder);
			EditorPrefs.SetBool(ArticyOverwriteKey, Overwrite);
			EditorPrefs.SetString(ArticyConversionSettingsKey, ConversionSettings.ToXml());
			EditorPrefs.SetInt(ArticyEncodingKey, (int) EncodingType);
		}
		
		public static void DeleteEditorPrefs() {
			EditorPrefs.DeleteKey(ArticyProjectFilenameKey);
			EditorPrefs.DeleteKey(ArticyPortraitFolderKey);
			EditorPrefs.DeleteKey(ArticyStageDirectionsAreSequencesKey);
			EditorPrefs.DeleteKey(ArticyFlowFragmentModeKey);
			EditorPrefs.DeleteKey(ArticyOutputFolderKey);
			EditorPrefs.DeleteKey(ArticyOverwriteKey);
			EditorPrefs.DeleteKey(ArticyConversionSettingsKey);
			EditorPrefs.DeleteKey(ArticyEncodingKey);
		}

		public void ReviewSpecialProperties(ArticyData articyData) {
			foreach (var articyEntity in articyData.entities.Values) {
				var conversionSetting = ConversionSettings.GetConversionSetting(articyEntity.id);
				if (conversionSetting.Include) {
					if (ArticyConverter.HasField(articyEntity.features, "IsNPC", false)) conversionSetting.Category = EntityCategory.NPC;
					if (ArticyConverter.HasField(articyEntity.features, "IsPlayer", true)) conversionSetting.Category = EntityCategory.Player;
					if (ArticyConverter.HasField(articyEntity.features, "IsItem", true)) conversionSetting.Category = EntityCategory.Item;
					if (ArticyConverter.HasField(articyEntity.features, "IsQuest", true)) conversionSetting.Category = EntityCategory.Quest;
				}
			}
		}
	
	}
	
}