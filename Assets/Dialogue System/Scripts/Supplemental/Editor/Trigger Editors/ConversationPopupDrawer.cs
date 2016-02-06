using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	[CustomPropertyDrawer(typeof(ConversationPopupAttribute))]
	public class ConversationPopupDrawer : PropertyDrawer {

		private bool usePicker = true;
		private string[] titles = null;
		private int currentIndex = -1;
		
		private bool ShowReferenceDatabase() {
			var attr = attribute as ConversationPopupAttribute;
			return (attr != null) ? attr.showReferenceDatabase : false;
		}
		
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
			return base.GetPropertyHeight(property, label) + 
				(ShowReferenceDatabase() ? EditorGUIUtility.singleLineHeight : 0);
		}
		
		public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
			// Validate internal variables:
			if (EditorTools.selectedDatabase == null) {
				EditorTools.SetInitialDatabaseIfNull();
			}
			if (EditorTools.selectedDatabase == null) {
				usePicker = false;
			}
			if (usePicker && (titles == null)) {
				UpdateTitles(prop.stringValue);
			}

			// Set up property drawer:
			EditorGUI.BeginProperty(position, GUIContent.none, prop);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			if (ShowReferenceDatabase()) {
				var dbPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
				position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);
				EditorTools.selectedDatabase = EditorGUI.ObjectField(dbPosition, EditorTools.selectedDatabase, typeof(DialogueDatabase), true) as DialogueDatabase;
			}

			// Draw popup or plain text field:
			var rect = new Rect(position.x, position.y, position.width - 28, position.height);
			if (usePicker) {
				var newIndex = EditorGUI.Popup(rect, currentIndex, titles);
				if ((newIndex != currentIndex) && (0 <= newIndex && newIndex < titles.Length)) {
					currentIndex = newIndex;
					prop.stringValue = titles[currentIndex];
				}
			} else {
				EditorGUI.BeginChangeCheck ();
				string value = EditorGUI.TextField(rect, prop.stringValue);
				if (EditorGUI.EndChangeCheck()) {
					prop.stringValue = value;
				}
			}

			// Radio button toggle between popup and plain text field:
			rect = new Rect(position.x + position.width - 22, position.y, 22, position.height);
			var newToggleValue = EditorGUI.Toggle(rect, usePicker, EditorStyles.radioButton);
			if (newToggleValue != usePicker) {
				usePicker = newToggleValue;
				if (usePicker && (EditorTools.selectedDatabase == null)) EditorTools.selectedDatabase = EditorTools.FindInitialDatabase();
				titles = null;
			}

			EditorGUI.EndProperty();
		}
		
		public void UpdateTitles(string currentConversation) {
			var database = EditorTools.selectedDatabase;
			currentIndex = -1;
			if (database == null || database.conversations == null) {
				titles = new string[0];
			} else {
				titles = new string[database.conversations.Count];
				for (int i = 0; i < database.conversations.Count; i++) {
					titles[i] = database.conversations[i].Title;
					if (string.Equals(currentConversation, titles[i])) {
						currentIndex = i;
					}
				}
			}
		}
		
	}

}
