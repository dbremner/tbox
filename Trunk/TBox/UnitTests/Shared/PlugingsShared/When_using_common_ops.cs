using System.Collections.Generic;
using Common.Encoders;
using NUnit.Framework;
using PluginsShared.Encoders;

namespace UnitTests.Shared.PlugingsShared
{
	[TestFixture]
	class When_using_common_ops
	{
		[Test]
		[TestCase("", "")]
		[TestCase("a", "a")]
		[TestCase("\r", "\\r")]
		[TestCase("\r\n", "\\r\\n")]
		[TestCase("\\", "\\\\")]
		[TestCase("\\\\", "\\\\\\\\")]
		[TestCase("\\\r", "\\\\\\r")]
		public void Should_encode_string(string value, string expected)
		{
			Assert.AreEqual(expected, CommonOps.EncodeString(value));
		}

		[Test]
		[TestCase("", "")]
		[TestCase("a", "a")]
		[TestCase("\\\\", "\\")]
		[TestCase("\\r\\n", "\r\n")]
		[TestCase("\\", "\\")]
		[TestCase("\\\\r", "\\r")]
		[TestCase("test\\", "test\\")]
		[TestCase("\"test\\\"", "\"test\"")]
		public void Should_decode_string(string value, string expected)
		{
			Assert.AreEqual(expected, CommonOps.DecodeString(value));
		}

		[Test]
		[TestCase("", "")]
		[TestCase("a", "a")]
		[TestCase("  ", " ")]
		[TestCase("\r\n", " ")]
		[TestCase("\r\n       ", " ")]
		[TestCase("a\r\nb      c", "a b c")]
		public void Should_minimize_string(string value, string expected)
		{
			Assert.AreEqual(expected, CommonOps.Minimize(value));
		}

		[Test]
		[TestCase("", -1)]
		[TestCase("abc", -1)]
		[TestCase("1", -1)]
		[TestCase("{", 0)]
		[TestCase("abc{def", 3)]
		[TestCase("\"{\"", -1)]
		[TestCase("abc\"{\"def}", -1)]
		[TestCase("abc\"{\"def{", 9)]
		[TestCase("\'{\'", -1)]
		[TestCase("abc\'{\'def}", -1)]
		[TestCase("abc\'{\'def{", 9)]
		[TestCase("abc\"{\'def\'{\"", -1)]
		[TestCase("abc\"{\'def\'{\"{", 12)]
		[TestCase("abc\"{\\\"def\\\"{\"{", 14)]
		public void Should_find_first_not_string_char(string value, int expected)
		{
			Assert.AreEqual(expected, value.FindFirstNotString('{', 0));
		}

		[Test]
		[TestCase("", -1)]
		[TestCase("abc", -1)]
		[TestCase("1", -1)]
		[TestCase("}", 0)]
		[TestCase("abc}def", 3)]
		[TestCase("\"}\"", -1)]
		[TestCase("abc\"}\"def{", -1)]
		[TestCase("abc\"}\"def}", 9)]
		[TestCase("\'}\'", -1)]
		[TestCase("abc\'}\'def{", -1)]
		[TestCase("abc\'}\'def}", 9)]
		[TestCase("abc\"}\'def\'}\"", -1)]
		[TestCase("abc\"}\'def\'}\"}", 12)]
		[TestCase("abc\"}\\\"def\\\"}\"}", 14)]
		[TestCase("{abc{abc}abc}}", 13)]
		[TestCase("{abc{abc abc}", -1)]
		public void Should_find_first_not_string_char_and_skip_extra_chars(string value, int expected)
		{
			Assert.AreEqual(expected, value.FindFirstNotString('}', 0, new KeyValuePair<char, char>('{', '}')));
		}
	}
}
