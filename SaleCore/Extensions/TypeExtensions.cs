using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SaleCore.Utilities;

namespace SaleCore.Extensions
{
	public static class TypeExtensions
	{

	    public static string ToMoneyString(this object data)
	    {
	        var i = DataUtil.ToInt(data);
	        return DataUtil.ChangeMoneyToString(i);
	    }


		public static IDictionary<string, object> ToDictionary(this object data)
		{
			if (data == null)
			{
				return new Dictionary<string, object>();
			}
			if (data.GetType().IsIDictionary())
			{
				return (IDictionary<string, object>)data;
			}
			return HtmlHelper.AnonymousObjectToHtmlAttributes(data);
		}
		public static IDictionary<string, object> ToHtmlDataAttributes(this object data)
		{
			if (data == null)
			{
				return new Dictionary<string, object>();
			}
			IDictionary<string, object> dictionary = data.ToDictionary();
			RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
			foreach (KeyValuePair<string, object> current in dictionary)
			{
				routeValueDictionary.Add("data-" + current.Key, current.Value);
			}
			return routeValueDictionary;
		}
		public static TagBuilder AddOrMergeCssClass(this TagBuilder tb, string attrValue)
		{
			if (tb.Attributes.ContainsKey("class"))
			{
				if (!tb.Attributes["class"].Contains(attrValue))
				{
					IDictionary<string, string> attributes;
					(attributes = tb.Attributes)["class"] = attributes["class"] + " " + attrValue;
				}
			}
			else
			{
				tb.Attributes.Add("class", attrValue);
			}
			return tb;
		}
		public static TagBuilder AddOrMergeAttribute(this TagBuilder tb, string attrName, string attrValue)
		{
			if (tb.Attributes.ContainsKey(attrName))
			{
				if (!tb.Attributes[attrName].Contains(attrValue))
				{
					IDictionary<string, string> attributes;
					(attributes = tb.Attributes)[attrName] = attributes[attrName] + " " + attrValue;
				}
			}
			else
			{
				tb.Attributes.Add(attrName, attrValue);
			}
			return tb;
		}
		public static IDictionary<string, object> AddOrReplace(this IDictionary<string, object> data, string key, string value)
		{
			if (data.ContainsKey(key))
			{
				data[key] = value;
			}
			else
			{
				data.Add(key, value);
			}
			return data;
		}
		public static IDictionary<string, object> AddOrMergeCssClass(this IDictionary<string, object> data, string key, string value)
		{
			if (data.ContainsKey(key))
			{
				if (data[key] is string && !Regex.IsMatch(data[key].ToString(), string.Format("(^|\\s){0}($|\\s)", value)))
				{
					data[key] = data[key] + " " + value;
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(value))
				{
					data.Add(key, value);
				}
			}
			return data;
		}
		public static IDictionary<string, object> RemoveCssClass(this IDictionary<string, object> data, string key, string value)
		{
			if (data.ContainsKey(key) && data[key] is string && Regex.IsMatch(data[key].ToString(), string.Format("(^|\\s){0}($|\\s)", value)))
			{
				data[key] = ((string)data[key]).Replace(value, "").Replace("  ", " ").Trim();
			}
			return data;
		}
		public static IDictionary<string, object> AddIfNotExist(this IDictionary<string, object> data, string key, string value)
		{
			if (!data.ContainsKey(key))
			{
				data.Add(key, value);
			}
			return data;
		}
		public static void MergeHtmlAttributes(this IDictionary<string, object> source, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> current in htmlAttributes)
			{
				if (!source.ContainsKey(current.Key))
				{
					source.Add(current.Key, current.Value);
				}
				else
				{
					if (current.Key.ToLower() == "class")
					{
						source[current.Key] = source[current.Key] + " " + current.Value;
					}
					else
					{
						source[current.Key] = current.Value;
					}
				}
			}
		}
		public static void MergeHtmlAttributes(this TagBuilder tb, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null)
			{
				throw new ArgumentNullException("Collection is null");
			}
			foreach (KeyValuePair<string, object> current in htmlAttributes)
			{
				if (!tb.Attributes.ContainsKey(current.Key))
				{
					tb.Attributes.Add(current.Key, (current.Value != null) ? current.Value.ToString() : string.Empty);
				}
				else
				{
					if (current.Key.ToLower() == "class")
					{
						tb.Attributes[current.Key] = tb.Attributes[current.Key] + " " + current.Value;
					}
					else
					{
						tb.Attributes[current.Key] = ((current.Value != null) ? current.Value.ToString() : string.Empty);
					}
				}
			}
		}
		public static IDictionary<string, object> FormatHtmlAttributes(this IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null)
			{
				return new Dictionary<string, object>();
			}
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (string current in htmlAttributes.Keys)
			{
				dictionary.Add(current.Replace('_', '-'), htmlAttributes[current]);
			}
			return dictionary;
		}
		public static IDictionary<string, object> ObjectToHtmlAttributesDictionary(this object htmlAttributes)
		{
			if (htmlAttributes == null)
			{
				return new Dictionary<string, object>();
			}
			IDictionary<string, object> dictionary = htmlAttributes as IDictionary<string, object>;
			if (dictionary == null)
			{
				dictionary = htmlAttributes.ToDictionary();
			}
			return dictionary;
		}
		public static TagBuilder AddCssStyle(this TagBuilder tb, string styleName, string styleValue)
		{
			if (tb.Attributes.ContainsKey("style"))
			{
				tb.Attributes["style"] = string.Concat(new string[]
				{
					tb.Attributes["style"],
					styleName,
					":",
					styleValue,
					";"
				});
			}
			else
			{
				tb.Attributes.Add("style", styleName + ":" + styleValue + ";");
			}
			return tb;
		}
		public static string SplitByUpperCase(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return null;
			}
			Regex regex = new Regex("(?!^)(?=[A-Z])");
			return regex.Replace(s, " ");
		}
		public static string FormatForMvcInputId(this string s)
		{
			return s.Replace(".", "_").Replace('[', '_').Replace(']', '_');
		}
		public static string GetEnumDescription(this Enum enumerationValue)
		{
			Type type = enumerationValue.GetType();
			if (!type.IsEnum)
			{
				throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
			}
			MemberInfo[] member = type.GetMember(enumerationValue.ToString());
			if (member != null && member.Length > 0)
			{
				object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				object[] customAttributes2 = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					return ((DescriptionAttribute)customAttributes[0]).Description;
				}
				if (customAttributes2 != null && customAttributes2.Length > 0)
				{
					return ((DisplayAttribute)customAttributes2[0]).Name;
				}
			}
			return enumerationValue.ToString();
		}
		public static string F(this string s, params object[] args)
		{
			return string.Format(s, args);
		}
		public static bool IsNullableEnum(this Type t)
		{
			Type underlyingType = Nullable.GetUnderlyingType(t);
			return underlyingType != null && underlyingType.IsEnum;
		}
		public static bool IsGenericEnumerable(this Type t)
		{
			return (t.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(t.GetGenericTypeDefinition())) || null != t.GetInterface("IEnumerable`1");
		}
		public static bool IsIDictionary(this Type t)
		{
			return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>)) || t.Name == "RouteValueDictionary";
		}
		public static IEnumerable<SelectListItem> SelectListItemsFromEnumMetadata(this ModelMetadata metadata)
		{
			Type type = metadata.ModelType;
			if (type.IsNullableEnum() || type.IsGenericEnumerable())
			{
				type = type.GetGenericArguments().First<Type>();
			}
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			List<SelectListItem> list = new List<SelectListItem>();
			foreach (Enum current in Enum.GetValues(type).OfType<Enum>())
			{
				list.Add(new SelectListItem
				{
					Value = Enum.Parse(type, current.ToString()).ToString(),
					Text = current.GetEnumDescription(),
					Selected = current.Equals(metadata.Model)
				});
			}
			return list;
		}
		public static RouteValueDictionary ToRouteValues(this NameValueCollection queryString)
		{
			if (queryString == null || !queryString.HasKeys())
			{
				return new RouteValueDictionary();
			}
			RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
			foreach (string current in
				from x in queryString.AllKeys
				where !string.IsNullOrEmpty(x)
				select x)
			{
				routeValueDictionary.Add(current, queryString[current]);
			}
			return routeValueDictionary;
		}
		public static TagBuilder PrependHtml(this TagBuilder source, string html)
		{
			if (source.InnerHtml == null)
				source.InnerHtml = string.Empty;
			source.InnerHtml = html + source.InnerHtml;
			return source;
		}

		public static TagBuilder PrependHtml(this TagBuilder source, TagBuilder tag)
		{
			return PrependHtml(source, tag.ToString());
		}

		public static TagBuilder PrependHtml(this TagBuilder source, IHtmlString html)
		{
			return PrependHtml(source, html.ToString());
		}


		public static TagBuilder AppendHtml(this TagBuilder source, string html)
		{
			if (source.InnerHtml == null)
				source.InnerHtml = string.Empty;
			source.InnerHtml += html;
			return source;
		}

		public static TagBuilder AppendHtml(this TagBuilder source, TagBuilder tag)
		{
			return AppendHtml(source, tag.ToString());
		}

		public static TagBuilder AppendHtml(this TagBuilder source, IHtmlString html)
		{
			if (html == null)
			{
				return source;
			}
			return AppendHtml(source, html.ToString());
		}


		public static TagBuilder New(string tagName)
		{
			return new TagBuilder(tagName);
		}
	}
}