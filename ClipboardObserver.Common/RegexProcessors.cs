using System;
using System.Text.RegularExpressions;

namespace ClipboardObserver.Common
{
	public sealed class RegExConverter<TOut> : RegExMatcher
		where TOut : class
	{
		private Func<GroupCollection, TOut> Conversion { get; }

		public RegExConverter(Regex pattern, Func<GroupCollection, TOut> conversion) : base(pattern)
		{
			Conversion = conversion;
		}

		public bool CanConvert(string input) => IsMatch(input) && Conversion != null;

		public TOut Convert(string input) => CanConvert(input) ? Conversion(MatchingGroups(input)) : null;
	}

	public sealed class RegExPattern : RegExMatcher
	{
		private Func<GroupCollection, string> ReplaceInterpolation { get; }

		public RegExPattern(Regex pattern, Func<GroupCollection, string> replaceInterpolation = null) : base(pattern)
		{
			ReplaceInterpolation = replaceInterpolation;
		}
		public bool CanTransform => ReplaceInterpolation != null;

		public string Transform(string input) => CanTransform && IsMatch(input) ? ReplaceInterpolation(MatchingGroups(input)) : string.Empty;
	}

	public class RegExMatcher
	{
		public RegExMatcher(Regex pattern)
		{
			Pattern = pattern;
		}

		public Regex Pattern { get; private set; }

		public bool IsMatch(string input) => Pattern.IsMatch(input);

		public Match Match(string input) => !IsMatch(input) ? null : Pattern.Match(input);

		public GroupCollection MatchingGroups(string input) => (!IsMatch(input) ? null : Match(input).Groups) ?? null;

	}}
