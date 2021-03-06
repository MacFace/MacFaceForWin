using System;

namespace MacFace.FloatApp
{
	/// <summary>
	/// AssemblyBuildDateAttribute の概要の説明です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ApplicationVersionStringAttribute : Attribute
	{
		private String _version;

		public ApplicationVersionStringAttribute(String version)
		{
			_version = version;
		}

		public String Version
		{
			get 
			{
				return _version;
			}
		}
	}
}
