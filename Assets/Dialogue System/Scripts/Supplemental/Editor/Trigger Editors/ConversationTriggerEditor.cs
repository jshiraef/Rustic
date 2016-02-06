using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {

	[CustomEditor(typeof(ConversationTrigger))]
	public class ConversationTriggerEditor : Editor {

		private ConversationPicker conversationPicker = null;

		public void OnEnable() {
			InitConversationPicker();
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
		}

		public void OnDisable() {
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
		}

		private void OnPlaymodeStateChanged() {
			InitConversationPicker();
		}

		private void InitConversationPicker() {
			var trigger = target as ConversationTrigger;
			if (trigger != null) {
				conversationPicker = new ConversationPicker(trigger.selectedDatabase, trigger.conversation, trigger.useConversationTitlePicker);
			}
		}
		
		public override void OnInspectorGUI() {
			var trigger = target as ConversationTrigger;
			if (trigger == null) return;
			EditorTools.DrawSerializedProperty(serializedObject, "trigger");
			if (conversationPicker != null) {
				if (conversationPicker.Draw()) {
					trigger.conversation = conversationPicker.currentConversation;
					EditorUtility.SetDirty(trigger);
				}
				trigger.useConversationTitlePicker = conversationPicker.usePicker;
				trigger.selectedDatabase = conversationPicker.database;
				if (EditorTools.selectedDatabase == null) EditorTools.selectedDatabase = trigger.selectedDatabase;
			} else {
				trigger.conversation = EditorGUILayout.TextField("Conversation", trigger.conversation);
			}
			trigger.actor = EditorGUILayout.ObjectField(new GUIContent("Actor", "The primary actor (e.g., player). If unassigned, the GameObject that triggered the conversation"), trigger.actor, typeof(Transform), true) as Transform;
			trigger.conversant = EditorGUILayout.ObjectField(new GUIContent("Conversant", "The other actor (e.g., NPC). If unassigned, this GameObject"), trigger.conversant, typeof(Transform), true) as Transform;
			trigger.once = EditorGUILayout.Toggle(new GUIContent("Only Once", "Only trigger once, then destroy this component"), trigger.once);
			trigger.exclusive = EditorGUILayout.Toggle(new GUIContent("Exclusive", "Only trigger if no other conversation is already active"), trigger.exclusive);
			trigger.skipIfNoValidEntries = EditorGUILayout.Toggle(new GUIContent("Skip If No Valid Entries", "Only trigger if at least one entry's Conditions are currently true"), trigger.skipIfNoValidEntries);
			trigger.stopConversationOnTriggerExit = EditorGUILayout.Toggle(new GUIContent("Stop On Trigger Exit", "Stop the triggered conversation if this GameObject receives OnTriggerExit"), trigger.stopConversationOnTriggerExit);
			EditorTools.DrawSerializedProperty(serializedObject, "condition");
		}

	}

}
