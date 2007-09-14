/*
 * PropertyList.cs
 * $Id$
 * 
 */

using System;
using System.Xml;
using System.Collections;
using System.Diagnostics;

namespace MacFace
{
	/// <summary>
	/// PropertyList ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class PropertyList : Hashtable
	{
		private PropertyList() {}

		public static Hashtable Load(string path)
		{
			XmlDocument doc = new XmlDocument();
			XmlTextReader reader = new XmlTextReader(path);
			reader.XmlResolver = null;
			doc.Load(reader);
			reader.Close();

			return ReadDictionary(doc.DocumentElement.FirstChild);
		}

		private static Hashtable ReadDictionary(XmlNode node)
		{
			XmlNodeList children = node.ChildNodes;
			int count = children.Count;
			Hashtable table = new Hashtable();

//Trace.WriteLine("#DICT#");
			for (int i = 0; i < count; i+=2) 
			{
				string key = children[i].InnerText;
				object value = ReadValue(children[i+1]);

				table.Add(key,value);
			}
//Trace.WriteLine("#/DICT#");
			return table;
		}

		private static ArrayList ReadArray(XmlNode node)
		{
			ArrayList array = new ArrayList();
//Trace.WriteLine("#ARRAY#");
			foreach (XmlNode child in node.ChildNodes) 
			{
				array.Add(ReadValue(child));
			}
//Trace.WriteLine("#/ARRAY#");

			return array;
		}

		private static object ReadValue(XmlNode node)
		{
			object value;
			switch (node.Name)
			{
				case "dict":
					value = ReadDictionary(node);
					break;
				case "array":
					value = ReadArray(node);
					break;
				case "string":
					value = node.InnerText;
					break;
                case "integer":
                    value = Int64.Parse(node.InnerText);
                    break;
                case "real":
                    value = Double.Parse(node.InnerText);
                    break;
                case "true":
                    value = true;
                    break;
                case "false":
                    value = false;
                    break;
                default:
					value = node;
					break;
			}
//Trace.WriteLine(value);
			return value;
		}
	}
}
