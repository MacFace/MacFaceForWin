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
		public static Hashtable load(string path)
		{
			XmlDocument doc = new XmlDocument();
			XmlTextReader reader = new XmlTextReader(path);
			reader.XmlResolver = null;
			doc.Load(reader);

			return readDictionary(doc.DocumentElement.FirstChild);
		}

		static Hashtable readDictionary(XmlNode node)
		{
			XmlNodeList children = node.ChildNodes;
			int count = children.Count;
			Hashtable table = new Hashtable();

//Trace.WriteLine("#DICT#");
			for (int i = 0; i < count; i+=2) 
			{
				string key = children[i].InnerText;
				object value = readValue(children[i+1]);

				table.Add(key,value);
			}
//Trace.WriteLine("#/DICT#");
			return table;
		}

		static ArrayList readArray(XmlNode node)
		{
			ArrayList array = new ArrayList();
//Trace.WriteLine("#ARRAY#");
			foreach (XmlNode child in node.ChildNodes) 
			{
				array.Add(readValue(child));
			}
//Trace.WriteLine("#/ARRAY#");

			return array;
		}

		static object readValue(XmlNode node)
		{
			object value;
			switch (node.Name)
			{
				case "dict":
					value = readDictionary(node);
					break;
				case "array":
					value = readArray(node);
					break;
				case "string":
					value = node.InnerText;
					break;
				case "integer":
					value = int.Parse(node.InnerText);
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
