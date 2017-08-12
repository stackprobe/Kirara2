using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Charlotte
{
	public class ReflecTools
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeName">ex. namespace + "." + class_name</param>
		/// <returns></returns>
		public static FieldInfo[] getFields(string typeName)
		{
			Type type = Type.GetType(typeName);

			if (type == null)
				throw new Exception("そんなタイプありません：" + typeName);

			return getFields(type);
		}

		public static FieldInfo[] getFields(object instance)
		{
			return getFields(instance.GetType());
		}

		private const BindingFlags _bindingFlags =
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Static |
			BindingFlags.Instance;

		public static FieldInfo[] getFields(Type type)
		{
			FieldInfo[] result = type.GetFields(_bindingFlags);
			return result;
		}

		/// <summary>
		/// name が見つからない場合 null を返す。
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static FieldInfo getField(object instance, string name)
		{
			return getField(instance.GetType(), name);
		}

		public static FieldInfo getField(Type type, string name)
		{
			FieldInfo result = type.GetField(name, _bindingFlags);
			return result;
		}

		public static bool equals(FieldInfo a, Type b)
		{
			return equals(a.FieldType, b);
		}

		public static bool equals(Type a, Type b)
		{
			return a.ToString() == b.ToString();
		}

		public static object getValue(FieldInfo fieldInfo, object instance)
		{
			return fieldInfo.GetValue(instance);
		}

		public static void setValue(FieldInfo fieldInfo, object instance, object value)
		{
			fieldInfo.SetValue(instance, value);
		}
	}
}
