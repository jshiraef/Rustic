using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem
{
	
	/// <summary>
	/// This is a typewriter effect for Unity UI. It handles bold, italic, and 
	/// color rich text tags and certain RPGMaker-style tags. It also works with any text alignment.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Typewriter Effect")]
	public class UnityUITypewriterEffect : MonoBehaviour
    {

		/// <summary>
		/// How fast to "type."
		/// </summary>
		[Tooltip("How fast to type. This is separate from Dialogue Manager > Subtitle Settings > Chars Per Second")]
		public float charactersPerSecond = 50;
		
		/// <summary>
		/// The audio clip to play with each character.
		/// </summary>
		[Tooltip("Optional audio clip to play with each character")]
		public AudioClip audioClip = null;

        /// <summary>
        /// The audio source through which to play the clip. If unassigned, will look for an
        /// audio source on this GameObject.
        /// </summary>
        [Tooltip("Optional audio source through which to play the clip")]
        public AudioSource audioSource = null;
		
		/// <summary>
		/// Indicates whether the effect is playing.
		/// </summary>
		/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get; private set; }

        private const string RichTextBoldOpen = "<b>";
        private const string RichTextBoldClose = "</b>";
        private const string RichTextItalicOpen = "<i>";
        private const string RichTextItalicClose = "</i>";
        private const string RichTextColorOpenPrefix = "<color=";
        private const string RichTextColorClose = "</color>";
        private const string RPGMakerCodeQuarterPause = @"\,";
        private const string RPGMakerCodeFullPause = @"\.";
        private const string RPGMakerCodeSkipToEnd = @"\^";
        private const string RPGMakerCodeInstantOpen = @"\>";
        private const string RPGMakerCodeInstantClose = @"\<";
        private const float FullPauseDuration = 1f;
        private const float QuarterPauseDuration = 0.25f;

        private enum TokenType
        {
            Character,
            BoldOpen,
            BoldClose,
            ItalicOpen,
            ItalicClose,
            ColorOpen,
            ColorClose,
            Pause,
            InstantOpen,
            InstantClose
        }

        private class Token
        {
            public TokenType tokenType;
            public char character;
            public string colorCode;
            public float duration;

            public Token(TokenType tokenType, char character, string colorCode, float duration)
            {
                this.tokenType = tokenType;
                this.character = character;
                this.colorCode = colorCode;
                this.duration = duration;
            }
        }

        private UnityEngine.UI.Text control;
        private bool started = false;
        private bool paused = false;
        private string original = string.Empty;

        private int MaxSafeguard = 16384;

        public void Awake()
        {
			control = GetComponent<UnityEngine.UI.Text>();
			if (audioSource == null) audioSource = GetComponent<AudioSource>();
		}
		
		public void Start()
        {
			if (control != null) control.supportRichText = true;
			if (!IsPlaying)
            {
				StopAllCoroutines();
				StartCoroutine(Play());
			}
			started = true;
		}
		
		public void OnEnable()
        {
			if (!IsPlaying && started && gameObject.activeInHierarchy)
            {
				StopAllCoroutines();
				StartCoroutine(Play());
			}
		}
		
		public void OnDisable()
        {
			Stop();
		}
		
		/// <summary>
		/// Pauses the effect.
		/// </summary>
		public void Pause()
        {
			paused = true;
		}
		
		/// <summary>
		/// Unpauses the effect. The text will resume at the point where it
		/// was paused; it won't try to catch up to make up for the pause.
		/// </summary>
		public void Unpause()
        {
    		paused = false;
		}
		
		/// <summary>
		/// Plays the typewriter effect.
		/// </summary>
		public IEnumerator Play()
        {
            if ((control != null) && (charactersPerSecond > 0))
            {
                IsPlaying = true;
                paused = false;
                float delay = 1 / charactersPerSecond;
                float lastTime = DialogueTime.time;
                float elapsed = 0;
                int charactersTyped = 0;
                original = control.text;
                var tokens = Tokenize(original);
                var openTokenTypes = new List<TokenType>();
                var current = new StringBuilder();
                while (tokens.Count > 0)
                {
                    if (!paused)
                    {
                        var deltaTime = DialogueTime.time - lastTime;
                        elapsed += deltaTime;
                        var goal = elapsed * charactersPerSecond;
                        var isCodeNext = false;
                        while (((charactersTyped < goal) || isCodeNext) && (tokens.Count > 0))
                        {
                            var token = GetNextToken(tokens);
                            switch (token.tokenType)
                            {
                                case TokenType.Character:
                                    current.Append(token.character);
                                    PlayCharacterAudio();
                                    charactersTyped++;
                                    break;
                                case TokenType.BoldOpen:
                                case TokenType.ItalicOpen:
                                case TokenType.ColorOpen:
                                    OpenRichText(current, token, openTokenTypes);
                                    break;
                                case TokenType.BoldClose:
                                case TokenType.ItalicClose:
                                case TokenType.ColorClose:
                                    CloseRichText(current, token, openTokenTypes);
                                    break;
                                case TokenType.Pause:
                                    control.text = GetCurrentText(current, openTokenTypes, tokens);
                                    paused = true;
                                    yield return new WaitForSeconds(token.duration);
                                    paused = false;
                                    break;
                                case TokenType.InstantOpen:
                                    AddInstantText(current, openTokenTypes, tokens);
                                    break;
                            }
                            isCodeNext = (tokens.Count > 0) && (tokens[0].tokenType != TokenType.Character);
                        }
                    }
                    control.text = GetCurrentText(current, openTokenTypes, tokens);
                    //---Uncomment the line below to debug: 
                    // Debug.Log(control.text.Replace("<", "[").Replace(">", "]"));
                    lastTime = DialogueTime.time;
                    yield return new WaitForSeconds(delay);
                }
            }
            Stop();
		}

        private void PlayCharacterAudio()
        {
            if (audioClip != null && audioSource != null && !audioSource.isPlaying) audioSource.PlayOneShot(audioClip);
        }

        private Token GetNextToken(List<Token> tokens)
        {
            if (tokens.Count == 0) return null;
            var token = tokens[0];
            tokens.RemoveAt(0);
            return token;
        }

        private void OpenRichText(StringBuilder current, Token token, List<TokenType> openTokens)
        {
            switch (token.tokenType)
            {
                case TokenType.BoldOpen:
                    current.Append(RichTextBoldOpen);
                    break;
                case TokenType.ItalicOpen:
                    current.Append(RichTextItalicOpen);
                    break;
                case TokenType.ColorOpen:
                    current.Append(token.colorCode);
                    break;
            }
            openTokens.Insert(0, token.tokenType);
        }

        private void CloseRichText(StringBuilder current, Token token, List<TokenType> openTokens)
        {
            var openTokenType = TokenType.BoldOpen;
            switch (token.tokenType)
            {
                case TokenType.BoldClose:
                    current.Append(RichTextBoldClose);
                    openTokenType = TokenType.BoldOpen;
                    break;
                case TokenType.ItalicClose:
                    current.Append(RichTextItalicClose);
                    openTokenType = TokenType.ItalicOpen;
                    break;
                case TokenType.ColorClose:
                    current.Append(RichTextColorClose);
                    openTokenType = TokenType.ColorOpen;
                    break;
            }
            var first = -1;
            for (int i = 0; i < openTokens.Count; i++)
            {
                if (openTokens[i] == openTokenType)
                {
                    first = i;
                    break;
                }
            }
            if (first != -1) openTokens.RemoveAt(first);
        }

        private void AddInstantText(StringBuilder current, List<TokenType> openTokenTypes, List<Token> tokens)
        {
            int safeguard = 0;
            while ((tokens.Count > 0) && (safeguard < MaxSafeguard))
            {
                safeguard++;
                var token = GetNextToken(tokens);
                switch (token.tokenType)
                {
                    case TokenType.Character:
                        current.Append(token.character);
                        break;
                    case TokenType.BoldOpen:
                    case TokenType.ItalicOpen:
                    case TokenType.ColorOpen:
                        OpenRichText(current, token, openTokenTypes);
                        break;
                    case TokenType.BoldClose:
                    case TokenType.ItalicClose:
                    case TokenType.ColorClose:
                        CloseRichText(current, token, openTokenTypes);
                        break;
                    case TokenType.InstantClose:
                        return;
                }
            }
        }

        private string GetCurrentText(StringBuilder current, List<TokenType> openTokenTypes, List<Token> tokens) 
        {
            // Start with the current text:
            var sb = new StringBuilder(current.ToString());
            
            // Close all open tags:
            for (int i = 0; i < openTokenTypes.Count; i++)
            {
                switch (openTokenTypes[i])
                {
                    case TokenType.BoldOpen:
                        sb.Append(RichTextBoldClose);
                        break;
                    case TokenType.ItalicOpen:
                        sb.Append(RichTextItalicClose);
                        break;
                    case TokenType.ColorOpen:
                        sb.Append(RichTextColorClose);
                        break;
                }
            }

            // Add the rest as invisible text so size/alignment is correct:
            sb.Append("<color=#00000000>");
            for (int i = openTokenTypes.Count - 1; i >= 0 ; i--) // Reopen early-closed codes.
            {
                switch (openTokenTypes[i])
                {
                    case TokenType.BoldOpen:
                        sb.Append(RichTextBoldOpen);
                        break;
                    case TokenType.ItalicOpen:
                        sb.Append(RichTextItalicOpen);
                        break;
                }
            }
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                switch (token.tokenType)
                {
                    case TokenType.BoldOpen:
                        sb.Append(RichTextBoldOpen);
                        break;
                    case TokenType.BoldClose:
                        sb.Append(RichTextBoldClose);
                        break;
                    case TokenType.ItalicOpen:
                        sb.Append(RichTextItalicOpen);
                        break;
                    case TokenType.ItalicClose:
                        sb.Append(RichTextItalicClose);
                        break;
                    case TokenType.ColorOpen:
                    case TokenType.ColorClose:
                        break;
                    case TokenType.Character:
                        sb.Append(token.character);
                        break;
                }
            }
            sb.Append("</color>");
            return sb.ToString();
        }

        private List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var remainder = text;
            int safeguard = 0;
            while (!string.IsNullOrEmpty(remainder) && (safeguard < MaxSafeguard))
            {
                safeguard++;
                Token token = null;
                if (remainder.StartsWith(@"<"))
                {
                    token = TryTokenize(RichTextBoldOpen, TokenType.BoldOpen, 0, ref remainder);
                    if (token == null) token = TryTokenize(RichTextBoldClose, TokenType.BoldClose, 0, ref remainder);
                    if (token == null) token = TryTokenize(RichTextItalicOpen, TokenType.ItalicOpen, 0, ref remainder);
                    if (token == null) token = TryTokenize(RichTextItalicClose, TokenType.ItalicClose, 0, ref remainder);
                    if (token == null) token = TryTokenize(RichTextColorClose, TokenType.ColorClose, 0, ref remainder);
                    if (token == null) token = TryTokenizeColorOpen(ref remainder);
                }
                else if (remainder.StartsWith(@"\"))
                {
                    // Check for RPGMaker-style codes:
                    token = TryTokenize(RPGMakerCodeFullPause, TokenType.Pause, FullPauseDuration, ref remainder);
                    if (token == null) token = TryTokenize(RPGMakerCodeQuarterPause, TokenType.Pause, QuarterPauseDuration, ref remainder);
                    if (token == null) token = TryTokenize(RPGMakerCodeInstantOpen, TokenType.InstantOpen, 0, ref remainder);
                    if (token == null) token = TryTokenize(RPGMakerCodeInstantClose, TokenType.InstantClose, 0, ref remainder);
                    if (token == null) token = TryTokenize(RPGMakerCodeSkipToEnd, TokenType.InstantOpen, 0, ref remainder);
                }
                if (token == null)
                {
                    // Get regular character:
                    token = new Token(TokenType.Character, remainder[0], string.Empty, 0);
                    remainder = remainder.Remove(0, 1);
                }
                tokens.Add(token);
            }
            return tokens;
        }

        private Token TryTokenize(string code, TokenType tokenType, float duration, ref string remainder)
        {
            if (remainder.StartsWith(code))
            {
                remainder = remainder.Remove(0, code.Length);
                return new Token(tokenType, ' ', string.Empty, duration);
            }
            else
            {
                return null;
            }
        }

        private Token TryTokenizeColorOpen(ref string remainder)
        {
            if (remainder.StartsWith(RichTextColorOpenPrefix))
            {
                var colorCode = remainder.Substring(0, remainder.IndexOf('>') + 1);
                remainder = remainder.Remove(0, colorCode.Length);
                return new Token(TokenType.ColorOpen, ' ', colorCode, 0);
            }
            else
            {
                return null;
            }
        }

        private string StripRPGMakerCodes(string s)
        {
            return s.Contains(@"\") ? s.Replace(RPGMakerCodeQuarterPause, string.Empty).
                Replace(RPGMakerCodeFullPause, string.Empty).
                Replace(RPGMakerCodeSkipToEnd, string.Empty).
                Replace(RPGMakerCodeInstantOpen, string.Empty).
                Replace(RPGMakerCodeInstantClose, string.Empty)
                : s;
        }

        /// <summary>
        /// Stops the effect.
        /// </summary>
        public void Stop() {
			StopAllCoroutines();
			IsPlaying = false;
			if (control != null) control.text = StripRPGMakerCodes(original);
			original = string.Empty;
		}
		
	}
	
}
