using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {
	
	[CustomEditor(typeof(DialogueSystemTrigger))]
	public class DialogueSystemTriggerEditor : Editor {

		private static bool actionFoldout = false;
		private static bool conversationFoldout = false;
		private static bool barkFoldout = false;
		private static bool sequenceFoldout = false;
		private static bool questFoldout = false;
		private static bool luaFoldout = false;
		private static bool alertFoldout = false;
		private ConversationPicker conversationPicker = null;
		private ConversationPicker barkPicker = null;
		private QuestPicker questPicker = null;
		private LuaScriptWizard luaScriptWizard = null;
		
		public void OnEnable() {
			var trigger = target as DialogueSystemTrigger;
			if (trigger != null) {
				EditorTools.SetInitialDatabaseIfNull();
				luaScriptWizard = new LuaScriptWizard(EditorTools.selectedDatabase);
				questPicker = new QuestPicker(trigger.selectedDatabase, trigger.questName, trigger.useQuestNamePicker);
				questPicker.showReferenceDatabase = false;
				InitConversationPickers();
				EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
				EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
			}
		}
		
		public void OnDisable() {
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
		}
		
		private void OnPlaymodeStateChanged() {
			InitConversationPickers();
		}
		
		private void InitConversationPickers() {
			var trigger = target as DialogueSystemTrigger;
			if (trigger != null) {
				conversationPicker = new ConversationPicker(trigger.selectedDatabase, trigger.conversation, trigger.useConversationTitlePicker);
				barkPicker = new ConversationPicker(trigger.selectedDatabase, trigger.barkConversation, trigger.useBarkTitlePicker);
			}
		}
		
		public override void OnInspectorGUI() {
			var trigger = target as DialogueSystemTrigger;
			if (trigger == null) return;

			// Trigger event:
			EditorTools.DrawSerializedProperty(serializedObject, "trigger");

			EditorTools.DrawReferenceDatabase();

			// Condition:
			EditorTools.DrawSerializedProperty(serializedObject, "condition");
			trigger.exclusive = EditorGUILayout.Toggle(new GUIContent("Exclusive", "Only trigger if no other conversation is already active"), trigger.exclusive);

			// Action:
			actionFoldout = EditorGUILayout.Foldout(actionFoldout, "Action");
			if (!actionFoldout) return;

			EditorWindowTools.StartIndentedSection();

			trigger.once = EditorGUILayout.Toggle(new GUIContent("Only Once", "Only trigger once, then destroy this component"), trigger.once);

			// Conversation:
			conversationFoldout = EditorGUILayout.Foldout(conversationFoldout, "Start Conversation");
			if (conversationFoldout) {
				EditorWindowTools.StartIndentedSection();

				if (conversationPicker != null) {
					if (conversationPicker.Draw(false)) {
						trigger.conversation = conversationPicker.currentConversation;
						EditorUtility.SetDirty(trigger);
					}
					trigger.useConversationTitlePicker = conversationPicker.usePicker;
					trigger.selectedDatabase = conversationPicker.database;
					if (EditorTools.selectedDatabase == null) EditorTools.selectedDatabase = trigger.selectedDatabase;
				} else {
					trigger.conversation = EditorGUILayout.TextField("Conversation", trigger.conversation);
				}
				if (!string.IsNullOrEmpty(trigger.conversation)) {
					trigger.conversationActor = EditorGUILayout.ObjectField(new GUIContent("Actor", "The primary actor (e.g., player). If unassigned, the GameObject that triggered the conversation"), trigger.conversationActor, typeof(Transform), true) as Transform;
					trigger.conversationConversant = EditorGUILayout.ObjectField(new GUIContent("Conversant", "The other actor (e.g., NPC). If unassigned, this GameObject"), trigger.conversationConversant, typeof(Transform), true) as Transform;
					trigger.skipIfNoValidEntries = EditorGUILayout.Toggle(new GUIContent("Skip If No Valid Entries", "Only trigger if at least one entry's Conditions are currently true"), trigger.skipIfNoValidEntries);
					trigger.stopConversationOnTriggerExit = EditorGUILayout.Toggle(new GUIContent("Stop On Trigger Exit", "Stop the triggered conversation if this GameObject receives OnTriggerExit"), trigger.stopConversationOnTriggerExit);
				}

				EditorWindowTools.EndIndentedSection();
			}

			// Bark:
			barkFoldout = EditorGUILayout.Foldout(barkFoldout, "Bark");
			if (barkFoldout) {
				EditorWindowTools.StartIndentedSection();

				if (barkPicker != null) {
					if (barkPicker.Draw(false)) {
						trigger.barkConversation = barkPicker.currentConversation;
						EditorUtility.SetDirty(trigger);
					}
					trigger.useBarkTitlePicker = barkPicker.usePicker;
					trigger.selectedDatabase = barkPicker.database;
					if (EditorTools.selectedDatabase == null) EditorTools.selectedDatabase = trigger.selectedDatabase;
				} else {
					trigger.barkConversation = EditorGUILayout.TextField("Bark Conversation", trigger.barkConversation);
				}
				if (!string.IsNullOrEmpty(trigger.barkConversation)) {
					trigger.barkOrder = (BarkOrder) EditorGUILayout.EnumPopup(new GUIContent("Bark Order", "The order in which to bark dialogue entries"), trigger.barkOrder);
					trigger.barker = EditorGUILayout.ObjectField(new GUIContent("Barker", "The actor speaking the bark. If unassigned, this GameObject"), trigger.barker, typeof(Transform), true) as Transform;
					trigger.barkTarget = EditorGUILayout.ObjectField(new GUIContent("Target", "The GameObject being barked at"), trigger.barkTarget, typeof(Transform), true) as Transform;
					trigger.skipBarkIfNoValidEntries = EditorGUILayout.Toggle(new GUIContent("Skip If No Valid Entries", "Only trigger if at least one entry's Conditions are currently true"), trigger.skipBarkIfNoValidEntries);
					trigger.allowBarksDuringConversations = EditorGUILayout.Toggle(new GUIContent("Allow During Conversations", "Allow barks during active conversations"), trigger.allowBarksDuringConversations);
					trigger.cacheBarkLines = EditorGUILayout.Toggle(new GUIContent("Cache Bark Lines", "Cache all bark lines on first bark. Faster, but loses dynamic barks"), trigger.cacheBarkLines);

				}

				EditorWindowTools.EndIndentedSection();
			}
			
			// Sequence:
			sequenceFoldout = EditorGUILayout.Foldout(sequenceFoldout, "Play Sequence");
			if (sequenceFoldout) {
				EditorWindowTools.StartIndentedSection();

				if (trigger.trigger == DialogueTriggerEvent.OnEnable || trigger.trigger == DialogueTriggerEvent.OnStart) {
					trigger.waitOneFrameOnStartOrEnable = EditorGUILayout.Toggle(new GUIContent("Wait 1 Frame", "Tick to wait one frame to allow other components to finish their OnStart/OnEnable"), trigger.waitOneFrameOnStartOrEnable);
				}

				trigger.sequence = SequenceEditorTools.DrawLayout(new GUIContent("Sequence", "The sequence to play"), trigger.sequence);
				//---Was:
				//EditorGUILayout.LabelField(new GUIContent("Sequence", "The sequence to play"));
				//EditorWindowTools.StartIndentedSection();
				//trigger.sequence = EditorGUILayout.TextArea(trigger.sequence);
				//EditorWindowTools.EndIndentedSection();

				trigger.sequenceSpeaker = EditorGUILayout.ObjectField(new GUIContent("Speaker", "The GameObject referenced by 'speaker'. If unassigned, this GameObject"), trigger.sequenceSpeaker, typeof(Transform), true) as Transform;
				trigger.sequenceListener = EditorGUILayout.ObjectField(new GUIContent("Listener", "The GameObject referenced by 'listener'. If unassigned, the GameObject that triggered this sequence"), trigger.sequenceListener, typeof(Transform), true) as Transform;

				EditorWindowTools.EndIndentedSection();
			}

			// Quest:
			questFoldout = EditorGUILayout.Foldout(questFoldout, "Set Quest State");
			if (questFoldout) {
				EditorWindowTools.StartIndentedSection();

				// Quest picker:
				if (questPicker != null) {
					questPicker.Draw();
					trigger.questName = questPicker.currentQuest;
					trigger.useQuestNamePicker = questPicker.usePicker;
					trigger.selectedDatabase = questPicker.database;
					if (EditorTools.selectedDatabase == null) EditorTools.selectedDatabase = trigger.selectedDatabase;
				}
				
				// Quest state:
				trigger.setQuestState = EditorGUILayout.Toggle("Set Quest State", trigger.setQuestState);
				if (trigger.setQuestState) {
					EditorTools.DrawSerializedProperty(serializedObject, "questState");
				}
				
				// Quest entry state:
				trigger.setQuestEntryState = EditorGUILayout.Toggle(new GUIContent("Set Quest Entry State", "Tick to set the state of a quest entry (subtask) in a quest"), trigger.setQuestEntryState);
				if (trigger.setQuestEntryState) {
					trigger.questEntryNumber = EditorGUILayout.IntField("Quest Entry Number", trigger.questEntryNumber);
					EditorTools.DrawSerializedProperty(serializedObject, "questEntryState");
				}

				EditorWindowTools.EndIndentedSection();
			}
			
			// Lua code / wizard:
			luaFoldout = EditorGUILayout.Foldout(luaFoldout, "Run Lua Code");
			if (luaFoldout) {
				EditorWindowTools.StartIndentedSection();
				if (EditorTools.selectedDatabase != luaScriptWizard.database) {
					luaScriptWizard.database = EditorTools.selectedDatabase;
					luaScriptWizard.RefreshWizardResources();
				}
				trigger.luaCode = luaScriptWizard.Draw(new GUIContent("Lua Code", "The Lua code to run when the condition is true"), trigger.luaCode);
				EditorWindowTools.EndIndentedSection();
			}

			// Alert:
			alertFoldout = EditorGUILayout.Foldout(alertFoldout, "Show Alert");
			if (alertFoldout) {
				EditorWindowTools.StartIndentedSection();
				trigger.alertMessage = EditorGUILayout.TextField(new GUIContent("Alert Message", "Optional alert message to display when triggered"), trigger.alertMessage);
				trigger.localizedTextTable = EditorGUILayout.ObjectField(new GUIContent("Localized Text Table", "The localized text table to use for the alert message text"), trigger.localizedTextTable, typeof(LocalizedTextTable), true) as LocalizedTextTable;
				bool specifyAlertDuration = !Mathf.Approximately(0, trigger.alertDuration);
				specifyAlertDuration = EditorGUILayout.Toggle(new GUIContent("Specify Duration", "Tick to specify an alert duration; untick to use the default"), specifyAlertDuration);
				if (specifyAlertDuration) {
					if (Mathf.Approximately(0, trigger.alertDuration)) trigger.alertDuration = 5;
					trigger.alertDuration = EditorGUILayout.FloatField("Seconds", trigger.alertDuration);
				} else {
					trigger.alertDuration = 0;
				}
				EditorWindowTools.EndIndentedSection();
			}

			// Send Messages list:
			EditorTools.DrawSerializedProperty(serializedObject, "sendMessages");

			EditorWindowTools.EndIndentedSection();
			

		}
		
	}
	
}
