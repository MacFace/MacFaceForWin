/* 
 * AutoConfigurator.cs
 * $Id$
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
using System.ComponentModel;
using System.Reflection;

namespace Misuzilla.Utilities
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class
	AutoConfigureAttribute : Attribute
	{
		private String _key = "";
		public
		AutoConfigureAttribute(String appSettingKey)
		{
			_key = appSettingKey;
		}
		public String
		Key
		{
			get { return _key; }
			set { _key = value; }
		}
	}
	
	public class
	AutoConfigurator
	{
		private AutoConfigurator() {} // 
		public delegate Object ConfigurationHandler(String key, Type propertyType);
		public delegate void GetConfigurationHandler(String key, Object value, Type propertyType);
		public static void
		Configure(Object o, ConfigurationHandler confHandler)
		{
			PropertyInfo pi ;
			FieldInfo fi;
			Type t = o.GetType();
			foreach (MemberInfo mi in t.GetMembers()) {
				AutoConfigureAttribute attr = Attribute.GetCustomAttribute(mi, typeof(AutoConfigureAttribute)) as AutoConfigureAttribute;
				if (attr != null && confHandler != null) {
					try {
						pi = mi as PropertyInfo;
						fi = mi as FieldInfo;
						if (pi != null) {
							Object value = confHandler(attr.Key, pi.PropertyType);
							if (value != null) {
								pi.SetValue(o, value, null);
							}
						} else if (fi != null) {
							Object value = confHandler(attr.Key, fi.FieldType);
							if (value != null) {
								fi.SetValue(o, value);
							}
						}
						} catch (Exception) {
					}
				}
			}
		}
		public static void
		GetConfiguration(Object o, GetConfigurationHandler confHandler)
		{
			Type t = o.GetType();
			PropertyInfo pi;
			FieldInfo fi;
			foreach (MemberInfo mi in t.GetMembers()) {
				AutoConfigureAttribute attr = Attribute.GetCustomAttribute(mi, typeof(AutoConfigureAttribute)) as AutoConfigureAttribute;
				if (attr != null && confHandler != null) {
					try {
						pi = mi as PropertyInfo;
						fi = mi as FieldInfo;
						if (pi != null) {
							confHandler(attr.Key, pi.GetValue(o, null), pi.PropertyType);
						} else if (fi != null) {
							confHandler(attr.Key, fi.GetValue(o), fi.FieldType);
						}
					} catch (Exception) {
					}
				}
			}
		}
	}
}