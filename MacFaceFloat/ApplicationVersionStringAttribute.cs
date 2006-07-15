using System;

namespace MacFace.FloatApp
{
	/// <summary>
	/// AssemblyBuildDateAttribute ‚ÌŠT—v‚Ìà–¾‚Å‚·B
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
