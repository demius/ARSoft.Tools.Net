﻿#region Copyright and License
// Copyright 2010..2012 Alexander Reinert
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARSoft.Tools.Net.Dns
{
	/// <summary>
	///   <para>Text strings</para> <para>Defined in
	///                               <see cref="!:http://tools.ietf.org/html/rfc1035">RFC 1035</see>
	///                             </para>
	/// </summary>
	public class TxtRecord : DnsRecordBase, ITextRecord
	{
		/// <summary>
		///   Text data
		/// </summary>
		public string TextData { get; protected set; }

		internal TxtRecord() {}

		/// <summary>
		///   Creates a new instance of the TxtRecord class
		/// </summary>
		/// <param name="name"> Name of the record </param>
		/// <param name="timeToLive"> Seconds the record should be cached at most </param>
		/// <param name="textData"> Text data </param>
		public TxtRecord(string name, int timeToLive, string textData)
			: base(name, RecordType.Txt, RecordClass.INet, timeToLive)
		{
			TextData = textData ?? String.Empty;
		}

		internal override void ParseRecordData(byte[] resultData, int startPosition, int length)
		{
			int endPosition = startPosition + length;

			TextData = String.Empty;
			while (startPosition < endPosition)
			{
				TextData += DnsMessageBase.ParseText(resultData, ref startPosition);
			}
		}

		internal override string RecordDataToString()
		{
			return " \"" + TextData + "\"";
		}

		protected internal override int MaximumRecordDataLength
		{
			get { return TextData.Length + (TextData.Length / 255) + (TextData.Length % 255 == 0 ? 0 : 1); }
		}

		protected internal override void EncodeRecordData(byte[] messageData, int offset, ref int currentPosition, Dictionary<string, ushort> domainNames)
		{
			DnsMessageBase.EncodeText(messageData, ref currentPosition, TextData);
		}
	}
}