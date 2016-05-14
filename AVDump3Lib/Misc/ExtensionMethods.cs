﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AVDump3Lib.Misc {
    public static class ExtensionMethods {
		#region Invariant String<->Type Conversion Extensions
		public static double ToInvDouble(this string s) { return double.Parse(s, CultureInfo.InvariantCulture); }
		public static double ToInvDouble(this string s, double defVal) { double val; if(double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static double ToInvDouble(this string s, NumberStyles style, double defVal) { double val; if(double.TryParse(s, style, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static string ToInvString(this double s) { return s.ToString(CultureInfo.InvariantCulture); }
		public static string ToInvString(this double s, string format) { return s.ToString(format, CultureInfo.InvariantCulture); }

		public static float ToInvFloat(this string s) { return float.Parse(s, CultureInfo.InvariantCulture); }
		public static float ToInvFloat(this string s, float defVal) { float val; if(float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static float ToInvFloat(this string s, NumberStyles style, float defVal) { float val; if(float.TryParse(s, style, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static string ToInvString(this float s) { return s.ToString(CultureInfo.InvariantCulture); }

        public static Int64 ToInvInt64(this string s) { return Int64.Parse(s, CultureInfo.InvariantCulture); }
        public static Int64 ToInvInt64(this string s, Int64 defVal) { Int64 val; if(Int64.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
        public static string ToInvString(this Int64 v) { return v.ToString(CultureInfo.InvariantCulture); }

        public static UInt64 ToInvUInt64(this string s) { return UInt64.Parse(s, CultureInfo.InvariantCulture); }
        public static UInt64 ToInvUInt64(this string s, UInt64 defVal) { UInt64 val; if(UInt64.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
        public static string ToInvString(this UInt64 v) { return v.ToString(CultureInfo.InvariantCulture); }

        public static Int32 ToInvInt32(this string s) { return Int32.Parse(s, CultureInfo.InvariantCulture); }
		public static Int32 ToInvInt32(this string s, Int32 defVal) { Int32 val; if(Int32.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static string ToInvString(this Int32 v) { return v.ToString(CultureInfo.InvariantCulture); }

		public static Int16 ToInvInt16(this string s) { return Int16.Parse(s, CultureInfo.InvariantCulture); }
		public static Int16 ToInvInt16(this string s, Int16 defVal) { Int16 val; if(Int16.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) return val; else return defVal; }
		public static string ToInvString(this Int16 v) { return v.ToString(CultureInfo.InvariantCulture); }

		public static DateTime ToInvDateTime(this string s) { return DateTime.Parse(s, CultureInfo.InvariantCulture); }
		public static DateTime ToInvDateTime(this string s, DateTime defVal) { DateTime val; if(DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out val)) return val; else return defVal; }
		public static DateTime? ToInvDateTime(this string s, DateTime? defVal) { DateTime val; if(DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out val)) return val; else return defVal; }
		public static string ToInvString(this DateTime v) { return v.ToString(CultureInfo.InvariantCulture); }
		#endregion

		public static TResult OnNotNullReturn<TResult, TSource>(this TSource n, Func<TSource, TResult> transform) where TSource : class {
			return n != null ? transform(n) : default(TResult);
		}
		public static TResult? OnNotNullReturn<TResult, TSource>(this TSource n, Func<TSource, TResult?> transform) where TResult : struct {
			return n != null ? transform(n) : null;
		}
		public static void OnNotNull<TSource>(this TSource n, Action<TSource> transform) { if(n != null) transform(n); }


		public static string Truncate(this string value, int maxLength) {
			return (value ?? "").Length <= maxLength ? value : value.Substring(0, maxLength);
		}
	}

	public class BitConverterEx {
		public const string Base2 = "01";
		public const string Base4 = "0123";
		public const string Base8 = "01234567";
		public const string Base10 = "0123456789";
		public const string Base16 = "0123456789ABCDEF";
		public const string Base32Hex = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
		public const string Base32Z = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
		public const string Base32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
		public const string Base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public const string Base62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public const string Base64 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ+/";

		public static string ToBase32String(byte[] inArray, string symbols = Base32) {
			if(inArray == null) return null;

			int len = inArray.Length;
			// divide the input into 40-bit groups, so let's see, 
			// how many groups of 5 bytes can we get out of it?
			int numberOfGroups = len / 5;
			// and how many remaining bytes are there?
			int numberOfRemainingBytes = len - 5 * numberOfGroups;

			// after this, we're gonna split it into eight 5 bit
			// values. 
			StringBuilder sb = new StringBuilder();
			//int resultLen = 4*((len + 2)/3);
			//StringBuffer result = new StringBuffer(resultLen);

			// Translate all full groups from byte array elements to Base64
			int byteIndexer = 0;
			for(int i = 0;i < numberOfGroups;i++) {
				byte b0 = inArray[byteIndexer++];
				byte b1 = inArray[byteIndexer++];
				byte b2 = inArray[byteIndexer++];
				byte b3 = inArray[byteIndexer++];
				byte b4 = inArray[byteIndexer++];

				// first 5 bits from byte 0
				sb.Append(symbols[b0 >> 3]);
				// the remaining 3, plus 2 from the next one
				sb.Append(symbols[(b0 << 2) & 0x1F | (b1 >> 6)]);
				// get bit 3, 4, 5, 6, 7 from byte 1
				sb.Append(symbols[(b1 >> 1) & 0x1F]);
				// then 1 bit from byte 1, and 4 from byte 2
				sb.Append(symbols[(b1 << 4) & 0x1F | (b2 >> 4)]);
				// 4 bits from byte 2, 1 from byte3
				sb.Append(symbols[(b2 << 1) & 0x1F | (b3 >> 7)]);
				// get bit 2, 3, 4, 5, 6 from byte 3
				sb.Append(symbols[(b3 >> 2) & 0x1F]);
				// 2 last bits from byte 3, 3 from byte 4
				sb.Append(symbols[(b3 << 3) & 0x1F | (b4 >> 5)]);
				// the last 5 bits
				sb.Append(symbols[b4 & 0x1F]);
			}

			// Now, is there any remaining bytes?
			if(numberOfRemainingBytes > 0) {
				byte b0 = inArray[byteIndexer++];
				// as usual, get the first 5 bits
				sb.Append(symbols[b0 >> 3]);
				// now let's see, depending on the 
				// number of remaining bytes, we do different
				// things
				switch(numberOfRemainingBytes) {
					case 1:
						// use the remaining 3 bits, padded with five 0 bits
						sb.Append(symbols[(b0 << 2) & 0x1F]);
						//						sb.Append("======");
						break;
					case 2:
						byte b1 = inArray[byteIndexer++];
						sb.Append(symbols[(b0 << 2) & 0x1F | (b1 >> 6)]);
						sb.Append(symbols[(b1 >> 1) & 0x1F]);
						sb.Append(symbols[(b1 << 4) & 0x1F]);
						//						sb.Append("====");
						break;
					case 3:
						b1 = inArray[byteIndexer++];
						byte b2 = inArray[byteIndexer++];
						sb.Append(symbols[(b0 << 2) & 0x1F | (b1 >> 6)]);
						sb.Append(symbols[(b1 >> 1) & 0x1F]);
						sb.Append(symbols[(b1 << 4) & 0x1F | (b2 >> 4)]);
						sb.Append(symbols[(b2 << 1) & 0x1F]);
						//						sb.Append("===");
						break;
					case 4:
						b1 = inArray[byteIndexer++];
						b2 = inArray[byteIndexer++];
						byte b3 = inArray[byteIndexer++];
						sb.Append(symbols[(b0 << 2) & 0x1F | (b1 >> 6)]);
						sb.Append(symbols[(b1 >> 1) & 0x1F]);
						sb.Append(symbols[(b1 << 4) & 0x1F | (b2 >> 4)]);
						sb.Append(symbols[(b2 << 1) & 0x1F | (b3 >> 7)]);
						sb.Append(symbols[(b3 >> 2) & 0x1F]);
						sb.Append(symbols[(b3 << 3) & 0x1F]);
						//						sb.Append("=");
						break;
				}
			}
			return sb.ToString();
		}
		public static string ToBase16String(byte[] value) { return string.Concat(value.Select(b => b.ToString("X2"))); } //TODO: Improve
	}
}