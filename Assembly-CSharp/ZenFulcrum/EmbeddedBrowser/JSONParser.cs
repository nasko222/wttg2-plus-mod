using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace ZenFulcrum.EmbeddedBrowser
{
	internal static class JSONParser
	{
		static JSONParser()
		{
			JSONParser.EscapeTable = new char[93];
			JSONParser.EscapeTable[34] = '"';
			JSONParser.EscapeTable[92] = '\\';
			JSONParser.EscapeTable[8] = 'b';
			JSONParser.EscapeTable[12] = 'f';
			JSONParser.EscapeTable[10] = 'n';
			JSONParser.EscapeTable[13] = 'r';
			JSONParser.EscapeTable[9] = 't';
		}

		public static JSONNode Parse(string json)
		{
			JSONNode result;
			if (JSONParser.TryDeserializeObject(json, out result))
			{
				return result;
			}
			throw new SerializationException("Invalid JSON string");
		}

		public static bool TryDeserializeObject(string json, out JSONNode obj)
		{
			bool result = true;
			if (json != null)
			{
				char[] json2 = json.ToCharArray();
				int num = 0;
				obj = JSONParser.ParseValue(json2, ref num, ref result);
			}
			else
			{
				obj = null;
			}
			return result;
		}

		public static string EscapeToJavascriptString(string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return jsonString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < jsonString.Length)
			{
				char c = jsonString[i++];
				if (c == '\\')
				{
					int num = jsonString.Length - i;
					if (num >= 2)
					{
						char c2 = jsonString[i];
						if (c2 == '\\')
						{
							stringBuilder.Append('\\');
							i++;
						}
						else if (c2 == '"')
						{
							stringBuilder.Append("\"");
							i++;
						}
						else if (c2 == 't')
						{
							stringBuilder.Append('\t');
							i++;
						}
						else if (c2 == 'b')
						{
							stringBuilder.Append('\b');
							i++;
						}
						else if (c2 == 'n')
						{
							stringBuilder.Append('\n');
							i++;
						}
						else if (c2 == 'r')
						{
							stringBuilder.Append('\r');
							i++;
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static JSONNode ParseObject(char[] json, ref int index, ref bool success)
		{
			JSONNode jsonnode = new JSONNode(JSONNode.NodeType.Object);
			JSONParser.NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = JSONParser.LookAhead(json, index);
				if (num == 0)
				{
					success = false;
					return null;
				}
				if (num == 6)
				{
					JSONParser.NextToken(json, ref index);
				}
				else
				{
					if (num == 2)
					{
						JSONParser.NextToken(json, ref index);
						return jsonnode;
					}
					string k = JSONParser.ParseString(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					num = JSONParser.NextToken(json, ref index);
					if (num != 5)
					{
						success = false;
						return null;
					}
					JSONNode value = JSONParser.ParseValue(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					jsonnode[k] = value;
				}
			}
			return jsonnode;
		}

		private static JSONNode ParseArray(char[] json, ref int index, ref bool success)
		{
			JSONNode jsonnode = new JSONNode(JSONNode.NodeType.Array);
			JSONParser.NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = JSONParser.LookAhead(json, index);
				if (num == 0)
				{
					success = false;
					return null;
				}
				if (num == 6)
				{
					JSONParser.NextToken(json, ref index);
				}
				else
				{
					if (num == 4)
					{
						JSONParser.NextToken(json, ref index);
						break;
					}
					JSONNode item = JSONParser.ParseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonnode.Add(item);
				}
			}
			return jsonnode;
		}

		private static JSONNode ParseValue(char[] json, ref int index, ref bool success)
		{
			switch (JSONParser.LookAhead(json, index))
			{
			case 1:
				return JSONParser.ParseObject(json, ref index, ref success);
			case 3:
				return JSONParser.ParseArray(json, ref index, ref success);
			case 7:
				return JSONParser.ParseString(json, ref index, ref success);
			case 8:
				return JSONParser.ParseNumber(json, ref index, ref success);
			case 9:
				JSONParser.NextToken(json, ref index);
				return true;
			case 10:
				JSONParser.NextToken(json, ref index);
				return false;
			case 11:
				JSONParser.NextToken(json, ref index);
				return JSONNode.NullNode;
			}
			success = false;
			return JSONNode.InvalidNode;
		}

		private static JSONNode ParseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			JSONParser.EatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag)
			{
				if (index == json.Length)
				{
					break;
				}
				c = json[index++];
				if (c == '"')
				{
					flag = true;
					break;
				}
				if (c == '\\')
				{
					if (index == json.Length)
					{
						break;
					}
					c = json[index++];
					if (c == '"')
					{
						stringBuilder.Append('"');
					}
					else if (c == '\\')
					{
						stringBuilder.Append('\\');
					}
					else if (c == '/')
					{
						stringBuilder.Append('/');
					}
					else if (c == 'b')
					{
						stringBuilder.Append('\b');
					}
					else if (c == 'f')
					{
						stringBuilder.Append('\f');
					}
					else if (c == 'n')
					{
						stringBuilder.Append('\n');
					}
					else if (c == 'r')
					{
						stringBuilder.Append('\r');
					}
					else if (c == 't')
					{
						stringBuilder.Append('\t');
					}
					else if (c == 'u')
					{
						int num = json.Length - index;
						if (num < 4)
						{
							break;
						}
						uint num2;
						if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2)))
						{
							return string.Empty;
						}
						if (55296u <= num2 && num2 <= 56319u)
						{
							index += 4;
							num = json.Length - index;
							uint num3;
							if (num < 6 || !(new string(json, index, 2) == "\\u") || !uint.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3) || 56320u > num3 || num3 > 57343u)
							{
								success = false;
								return string.Empty;
							}
							stringBuilder.Append((char)num2);
							stringBuilder.Append((char)num3);
							index += 6;
						}
						else
						{
							stringBuilder.Append(JSONParser.ConvertFromUtf32((int)num2));
							index += 4;
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			return stringBuilder.ToString();
		}

		private static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
			}
			if (55296 <= utf32 && utf32 <= 57343)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
			}
			if (utf32 < 65536)
			{
				return new string((char)utf32, 1);
			}
			utf32 -= 65536;
			return new string(new char[]
			{
				(char)((utf32 >> 10) + 55296),
				(char)(utf32 % 1024 + 56320)
			});
		}

		private static JSONNode ParseNumber(char[] json, ref int index, ref bool success)
		{
			JSONParser.EatWhitespace(json, ref index);
			int lastIndexOfNumber = JSONParser.GetLastIndexOfNumber(json, index);
			int length = lastIndexOfNumber - index + 1;
			string text = new string(json, index, length);
			JSONNode result;
			if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
			{
				double v;
				success = double.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out v);
				result = v;
			}
			else
			{
				long num;
				success = long.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out num);
				result = (float)num;
			}
			index = lastIndexOfNumber + 1;
			return result;
		}

		private static int GetLastIndexOfNumber(char[] json, int index)
		{
			int i;
			for (i = index; i < json.Length; i++)
			{
				if ("0123456789+-.eE".IndexOf(json[i]) == -1)
				{
					break;
				}
			}
			return i - 1;
		}

		private static void EatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length)
			{
				if (" \t\n\r\b\f".IndexOf(json[index]) == -1)
				{
					break;
				}
				index++;
			}
		}

		private static int LookAhead(char[] json, int index)
		{
			int num = index;
			return JSONParser.NextToken(json, ref num);
		}

		private static int NextToken(char[] json, ref int index)
		{
			JSONParser.EatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			switch (c)
			{
			case ',':
				return 6;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return 8;
			default:
				switch (c)
				{
				case '[':
					return 3;
				default:
					switch (c)
					{
					case '{':
						return 1;
					default:
					{
						if (c == '"')
						{
							return 7;
						}
						index--;
						int num = json.Length - index;
						if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
						{
							index += 5;
							return 10;
						}
						if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
						{
							index += 4;
							return 9;
						}
						if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
						{
							index += 4;
							return 11;
						}
						return 0;
					}
					case '}':
						return 2;
					}
					break;
				case ']':
					return 4;
				}
				break;
			case ':':
				return 5;
			}
		}

		public static string Serialize(JSONNode node)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!JSONParser.SerializeValue(node, stringBuilder))
			{
				throw new SerializationException("Failed to serialize JSON");
			}
			return stringBuilder.ToString();
		}

		private static bool SerializeValue(JSONNode value, StringBuilder builder)
		{
			bool result = true;
			switch (value.Type)
			{
			case JSONNode.NodeType.Invalid:
				throw new SerializationException("Cannot serialize invalid JSONNode");
			case JSONNode.NodeType.String:
				result = JSONParser.SerializeString(value, builder);
				break;
			case JSONNode.NodeType.Number:
				result = JSONParser.SerializeNumber(value, builder);
				break;
			case JSONNode.NodeType.Object:
			{
				Dictionary<string, JSONNode> dictionary = value;
				result = JSONParser.SerializeObject(dictionary.Keys, dictionary.Values, builder);
				break;
			}
			case JSONNode.NodeType.Array:
				result = JSONParser.SerializeArray(value, builder);
				break;
			case JSONNode.NodeType.Bool:
				builder.Append((!value) ? "false" : "true");
				break;
			case JSONNode.NodeType.Null:
				builder.Append("null");
				break;
			default:
				throw new SerializationException("Unknown JSONNode type");
			}
			return result;
		}

		private static bool SerializeObject(IEnumerable<string> keys, IEnumerable<JSONNode> values, StringBuilder builder)
		{
			builder.Append("{");
			IEnumerator<string> enumerator = keys.GetEnumerator();
			IEnumerator<JSONNode> enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				string text = enumerator.Current;
				JSONNode value = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				string text2 = text;
				if (text2 != null)
				{
					JSONParser.SerializeString(text2, builder);
				}
				else if (!JSONParser.SerializeValue(value, builder))
				{
					return false;
				}
				builder.Append(":");
				if (!JSONParser.SerializeValue(value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		private static bool SerializeArray(IEnumerable<JSONNode> anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			foreach (JSONNode value in anArray)
			{
				if (!flag)
				{
					builder.Append(",");
				}
				if (!JSONParser.SerializeValue(value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		private static bool SerializeString(string aString, StringBuilder builder)
		{
			if (aString.IndexOfAny(JSONParser.EscapeCharacters) == -1)
			{
				builder.Append('"');
				builder.Append(aString);
				builder.Append('"');
				return true;
			}
			builder.Append('"');
			int num = 0;
			char[] array = aString.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if ((int)c >= JSONParser.EscapeTable.Length || JSONParser.EscapeTable[(int)c] == '\0')
				{
					num++;
				}
				else
				{
					if (num > 0)
					{
						builder.Append(array, i - num, num);
						num = 0;
					}
					builder.Append('\\');
					builder.Append(JSONParser.EscapeTable[(int)c]);
				}
			}
			if (num > 0)
			{
				builder.Append(array, array.Length - num, num);
			}
			builder.Append('"');
			return true;
		}

		private static bool SerializeNumber(double number, StringBuilder builder)
		{
			builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
			return true;
		}

		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		private static readonly char[] EscapeTable;

		private static readonly char[] EscapeCharacters = new char[]
		{
			'"',
			'\\',
			'\b',
			'\f',
			'\n',
			'\r',
			'\t'
		};
	}
}
