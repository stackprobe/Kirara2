using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Charlotte.Tools
{
	public class XNode
	{
		public string name; // null ng
		public string value; // null ng
		public List<XNode> children; // null ng

		public XNode(string name = "", string value = "", List<XNode> children = null)
		{
			if (children == null)
				children = new List<XNode>();

			this.name = name;
			this.value = value;
			this.children = children;
		}

		public static XNode load(string xmlFile)
		{
			XNode node = new XNode();
			Stack<XNode> parents = new Stack<XNode>();

			using (XmlReader reader = XmlReader.Create(xmlFile))
			{
				while (reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							{
								XNode child = new XNode(reader.LocalName);

								node.children.Add(child);
								parents.Push(node);
								node = child;

								bool singleTag = reader.IsEmptyElement;

								while (reader.MoveToNextAttribute())
									node.children.Add(new XNode(reader.Name, reader.Value));

								if (singleTag)
									node = parents.Pop();
							}
							break;

						case XmlNodeType.Text:
							node.value = reader.Value;
							break;

						case XmlNodeType.EndElement:
							node = parents.Pop();
							break;

						default:
							break;
					}
				}
			}
			node = node.children[0];
			postLoad(node);
			return node;
		}

		private static void postLoad(XNode node)
		{
			node.name = nvFltr(node.name);
			node.value = nvFltr(node.value);

			{
				int index = node.name.IndexOf(':');

				if (index != -1)
					node.name = nvFltr(node.name.Substring(index + 1));
			}

			foreach (XNode child in node.children)
				postLoad(child);
		}

		private static string nvFltr(string value)
		{
			if (value == null)
				value = "";

			return value.Trim();
		}

		public void save(string xmlFile)
		{
			File.WriteAllText(xmlFile, getString(), Encoding.UTF8);
		}

		private const string SAVE_INDENT = "\t";
		private const string SAVE_NEW_LINE = "\n";

		public string getString()
		{
			StringBuilder buff = new StringBuilder();
			buff.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			buff.Append(SAVE_NEW_LINE);
			addTo(buff, "");
			return buff.ToString();
		}

		private void addTo(StringBuilder buff, String indent)
		{
			string name = this.name;

			if (name == "")
				name = "node";

			if (children.Count != 0)
			{
				buff.Append(indent);
				buff.Append("<");
				buff.Append(name);
				buff.Append(">");
				buff.Append(SAVE_NEW_LINE);

				foreach (XNode child in children)
					child.addTo(buff, indent + SAVE_INDENT);

				if (value != "")
				{
					buff.Append(indent);
					buff.Append(SAVE_INDENT);
					buff.Append(value);
					buff.Append(SAVE_NEW_LINE);
				}
				buff.Append(indent);
				buff.Append("</");
				buff.Append(name);
				buff.Append(">");
				buff.Append(SAVE_NEW_LINE);
			}
			else if (value != "")
			{
				buff.Append(indent);
				buff.Append("<");
				buff.Append(name);
				buff.Append(">");
				buff.Append(value);
				buff.Append("</");
				buff.Append(name);
				buff.Append(">");
				buff.Append(SAVE_NEW_LINE);
			}
			else
			{
				buff.Append(indent);
				buff.Append("<");
				buff.Append(name);
				buff.Append("/>");
				buff.Append(SAVE_NEW_LINE);
			}
		}

		public XNode get(string name)
		{
			foreach (XNode child in children)
				if (child.name == name)
					return child;

			return null; // not found
		}
	}
}
