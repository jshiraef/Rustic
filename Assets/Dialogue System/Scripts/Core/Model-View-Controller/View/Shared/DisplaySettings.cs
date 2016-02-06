using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// Display settings to apply to the dialogue UI and sequencer.
	/// </summary>
	[System.Serializable]
	public class DisplaySettings {
	
		[Tooltip("Assign a GameObject that contains an active dialogue UI component.")]
		public GameObject dialogueUI;
		
		[System.Serializable]
		public class LocalizationSettings {
			/// <summary>
			/// The current language, or blank to use the default language.
			/// </summary>
			[Tooltip("Current language, or blank to use the default language.")]
			public string language = string.Empty;

			/// <summary>
			/// Set <c>true</c> to automatically use the system language at startup.
			/// </summary>
			[Tooltip("Use the system language at startup.")]
			public bool useSystemLanguage = false;

			/// <summary>
			/// An optional localized text table. Used by DialogueSystemController.GetLocalizedText()
			/// and ShowAlert() if assigned.
			/// </summary>
			[Tooltip("Optional localized text for alerts and other general text.")]
			public LocalizedTextTable localizedText = null;
		}
		
		public LocalizationSettings localizationSettings = new LocalizationSettings();

		[System.Serializable]
		public class SubtitleSettings {
			/// <summary>
			/// Specifies whether to show NPC subtitles while speaking a line of dialogue.
			/// </summary>
			[Tooltip("Show NPC subtitle text while NPC speaks a line of dialogue.")]
			public bool showNPCSubtitlesDuringLine = true;
			
			/// <summary>
			/// Specifies whether to should show NPC subtitles while presenting the player's follow-up
			/// responses.
			/// </summary>
			[Tooltip("Show NPC subtitle reminder text while showing the player response menu.")]
			public bool showNPCSubtitlesWithResponses = true;
		
			/// <summary>
			/// Specifies whether to show PC subtitles while speaking a line of dialogue.
			/// </summary>
			[Tooltip("Show PC subtitle text while PC speaks a line of dialogue.")]
			public bool showPCSubtitlesDuringLine = false;

			/// <summary>
			/// Set <c>true</c> to allow PC subtitles to be used for the reminder line
			/// during the response menu.
			/// </summary>
			[Tooltip("Allow PC subtitles to be used for reminder text while showing the response menu.")]
			public bool allowPCSubtitleReminders = false;
			
			/// <summary>
			/// The default subtitle characters per second. This value is used to compute the default 
			/// duration to display a subtitle if no sequence is specified for a line of dialogue.
			/// This value is also used when displaying alerts.
			/// </summary>
			[Tooltip("Used to compute default duration to display subtitle. Typewriter effects have their own separate setting.")]
			public float subtitleCharsPerSecond = 30f;
			
			/// <summary>
			/// The minimum duration to display a subtitle if no sequence is specified for a line of 
			/// dialogue. This value is also used when displaying alerts.
			/// </summary>
			[Tooltip("Minimum default duration to display subtitle.")]
			public float minSubtitleSeconds = 2f;
			
			public enum ContinueButtonMode {
				/// <summary>
				/// Never wait for the continue button. Use this if your UI doesn't have continue buttons.
				/// </summary>
				Never,

				/// <summary>
				/// Always wait for the continue button.
				/// </summary>
				Always,

				/// <summary>
				/// Wait for the continue button, except when the response menu is next show but don't wait.
				/// </summary>
				OptionalBeforeResponseMenu,

				/// <summary>
				/// Wait for the continue button, except when the response menu is next hide it.
				/// </summary>
				NotBeforeResponseMenu,

				/// <summary>
				/// Wait for the continue button, except when a PC auto-select response or response
				/// menu is next, show but don't wait.
				/// </summary>
				OptionalBeforePC,

				/// <summary>
				/// Wait for the continue button, except with a PC auto-select response or response
				/// menu is next, hide it.
				/// </summary>
				NotBeforePC
			}

			/// <summary>
			/// How to handle continue buttons.
			/// </summary>
			public ContinueButtonMode continueButton = ContinueButtonMode.Never;
			
			/// <summary>
			/// Set <c>true</c> to convert "[em#]" tags to rich text codes in formatted text.
			/// Your implementation of IDialogueUI must support rich text.
			/// </summary>
			[Tooltip("Convert [em#] tags to rich text codes.")]
			public bool richTextEmphases = false;

			/// <summary>
			/// Set <c>true</c> to send OnSequenceStart and OnSequenceEnd messages with 
			/// every dialogue entry's sequence.
			/// </summary>
			[Tooltip("Send OnSequenceStart and OnSequenceEnd messages with every dialogue entry's sequence.")]
			public bool informSequenceStartAndEnd = false;
		}
		
		/// <summary>
		/// The subtitle settings.
		/// </summary>
		public SubtitleSettings subtitleSettings = new SubtitleSettings();
	
		[System.Serializable]
		public class CameraSettings {
			/// <summary>
			/// The camera (or prefab) to use for sequences. If unassigned, the sequencer will use the
			/// main camera; when the sequence is done, it will restore the main camera's original
			/// position.
			/// </summary>
			[Tooltip("Camera or prefab to use for sequences. If unassigned, sequences use the current main camera.")]
			public Camera sequencerCamera = null;

			/// <summary>
			/// An alternate camera object to use instead of sequencerCamera. Use this, for example,
			/// if you have an Oculus VR GameObject that's a parent of two cameras.  Currently this 
			/// <em>must</em> be an object in the scene, not a prefab.
			/// </summary>
			[Tooltip("If assigned, use instead of Sequencer Camera -- for example, Oculus VR GameObject. Can't be a prefab.")]
			public GameObject alternateCameraObject = null;
			
			/// <summary>
			/// The camera angle object (or prefab) to use for the "Camera()" sequence command. See
			/// @ref sequencerCommandCamera for more information.
			/// </summary>
			[Tooltip("Camera angle object or prefab. If unassigned, uses default camera angle definitions.")]
			public GameObject cameraAngles = null;
			
			/// <summary>
			/// The default sequence to use if the dialogue entry doesn't have a sequence defined 
			/// in its Sequence field. See @ref dialogueCreation and @ref sequencer for
			/// more information. The special keyword "{{end}}" gets replaced by the default
			/// duration for the subtitle being displayed.
			/// </summary>
			[Tooltip("Used when a dialogue entry doesn't define its own Sequence. Set to Delay({{end}}) to leave the camera untouched.")]
			public string defaultSequence = "Delay({{end}})"; //---Was: "Camera(default); required Camera(default,listener)@{{end}}";

			[Tooltip("If defined, overrides Default Sequence for player (PC) lines only.")]
			public string defaultPlayerSequence = string.Empty;

			[Tooltip("Used when a dialogue entry doesn't define its own Response Menu Sequence.")]
			public string defaultResponseMenuSequence = string.Empty;

			/// <summary>
			/// The format to use for the <c>entrytag</c> keyword.
			/// </summary>
			[Tooltip("Format to use for the 'entrytag' keyword.")]
			public EntrytagFormat entrytagFormat = EntrytagFormat.ActorName_ConversationID_EntryID;

			/// <summary>
			/// Set <c>true</c> to disable the internal sequencer commands -- for example, if you
			/// want to replace them with your own.
			/// </summary>
			[HideInInspector]
			public bool disableInternalSequencerCommands = false;
		}
		
		/// <summary>
		/// The camera settings.
		/// </summary>
		public CameraSettings cameraSettings = new CameraSettings();
		
		[System.Serializable]
		public class InputSettings {
			
			/// <summary>
			/// If <c>true</c>, always forces the response menu even if there's only one response.
			/// If <c>false</c>, you can use the <c>[f]</c> tag to force a response.
			/// </summary>
			[Tooltip("Show the response menu even if there's only one response.")]
			public bool alwaysForceResponseMenu = true;

			/// <summary>
			/// If `true`, includes responses whose Conditions are false. The `enabled` field of
			/// those responses will be `false`.
			/// </summary>
			[Tooltip("Include responses whose Conditions are false. typically shown in a disabled state.")]
			public bool includeInvalidEntries = false;
			
			/// <summary>
			/// If not <c>0</c>, the duration in seconds that the player has to choose a response; 
			/// otherwise the currently-focused response is auto-selected. If no response is
			/// focused (e.g., hovered over), the first response is auto-selected. If <c>0</c>,
			/// there is no timeout; the player can take as long as desired to choose a response.
			/// </summary>
			[Tooltip("If nonzero, the duration in seconds until the response menu times out.")]
			public float responseTimeout = 0f;
			
			/// <summary>
			/// The response timeout action.
			/// </summary>
			[Tooltip("What to do if the response menu times out.")]
			public ResponseTimeoutAction responseTimeoutAction = ResponseTimeoutAction.ChooseFirstResponse;

			/// <summary>
			/// The em tag to wrap around old responses. A response is old if its SimStatus 
			/// is "WasDisplayed". You can change this from EmTag.None if you want to visually
			/// mark old responses in the player response menu.
			/// </summary>
			[Tooltip("The [em#] tag to wrap around responses that have been previously chosen.")]
			public EmTag emTagForOldResponses = EmTag.None;
			
			/// <summary>
			/// The buttons QTE (Quick Time Event) buttons. QTE 0 & 1 default to the buttons
			/// Fire1 and Fire2.
			/// </summary>
			public string[] qteButtons = new string[] { "Fire1", "Fire2" };
			
			/// <summary>
			/// The key and/or button that allows the player to cancel subtitle sequences.
			/// </summary>
			[Tooltip("Cancels subtitle sequences.")]
			public InputTrigger cancel = new InputTrigger(KeyCode.Escape);
			
			/// <summary>
			/// The key and/or button that allows the player to cancel conversations.
			/// </summary>
			[Tooltip("Cancels the active conversation.")]
			public InputTrigger cancelConversation = new InputTrigger(KeyCode.Escape);
		}
		
		/// <summary>
		/// The input settings.
		/// </summary>
		public InputSettings inputSettings = new InputSettings();
		
		[System.Serializable]
		public class AlertSettings {
			
			/// <summary>
			/// Set <c>true</c> to allow the dialogue UI to show alerts during conversations.
			/// </summary>
			[Tooltip("Allow the dialogue UI to show alerts during conversations.")]
			public bool allowAlertsDuringConversations = false;
		
			/// <summary>
			/// How often to check if the Lua Variable['Alert'] has been set. To disable
			/// automatic monitoring, set this to <c>0</c>.
			/// </summary>
			[Tooltip("If nonzero, check Variable['Alert'] at this frequency to show alert messages.")]
			public float alertCheckFrequency = 0f;
			
		}
		
		/// <summary>
		/// The gameplay alert message settings.
		/// </summary>
		public AlertSettings alertSettings = new AlertSettings();
		
	}
	
	/// <summary>
	/// Response timeout action specifies what to do if the response menu times out.
	/// </summary>
	public enum ResponseTimeoutAction { 
		/// <summary>
		/// Auto-select the first menu choice.
		/// </summary>
		ChooseFirstResponse, 
		
		/// <summary>
		/// End of conversation.
		/// </summary>
		EndConversation };

	public enum EmTag {
		None,
		Em1,
		Em2,
		Em3,
		Em4
	}

}
