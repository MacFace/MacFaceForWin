/* 
 * appSettings.cs
 * $Id$
 * $CVSId: appSettings.cs,v 1.1.1.1 2003/04/01 04:00:51 Mayuki Sawatari Exp $
 *
 * Copyright (c) 2003 Mayuki Sawatari, All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE MISUZILLA.ORG ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */
 
using System;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

namespace
Misuzilla.Utilities
{
	public class
	Setting
	{
		
		public static void
		SaveSettings(String path, StringDictionary sd)
		{
			XmlDocument xmlconf = new XmlDocument();
			xmlconf.AppendChild(xmlconf.CreateElement("appSettings"));
			foreach (DictionaryEntry o in sd) {
				//Console.WriteLine("{0}={1}", o.Key, o.Value);
				XmlElement e = xmlconf.CreateElement("entry");
				xmlconf.DocumentElement.AppendChild(e);
				e.SetAttributeNode(xmlconf.CreateAttribute("key"));
				e.Attributes["key"].Value = (String)o.Key;
				e.InnerText = (String)o.Value;
			}

			try {
				//Console.WriteLine(xmlconf.InnerXml);
				xmlconf.Save(path);
			} catch (System.Xml.XmlException) {
			} catch (System.IO.IOException) {
			}
			
			//return true;
		} // SaveSettings
		
		public static StringDictionary
		GetSettings(String path)
		{
			StringDictionary sd = new StringDictionary();
			XmlDocument xmlconf = new XmlDocument();
			
			try {
				xmlconf.Load(path);
				foreach (XmlNode xnode in xmlconf.GetElementsByTagName("entry")) {
					if (xnode.Attributes["key"] != null) {
						sd.Add(xnode.Attributes["key"].Value.Trim(), xnode.InnerText);
					}
				}
			} catch (System.Xml.XmlException) {
			} catch (System.IO.IOException) {
			}
			
			return sd;
		} // GetSettings

	} // Setting

} // Misuzilla.Utilities