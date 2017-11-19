using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Wave.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Wave.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static Bitmap solidez
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("solidez", Resources.resourceCulture);
			}
		}

		internal static Bitmap splash
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("splash", Resources.resourceCulture);
			}
		}

		internal static Bitmap topBPiso
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("topBPiso", Resources.resourceCulture);
			}
		}

		internal static Bitmap topTitle
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("topTitle", Resources.resourceCulture);
			}
		}

		internal static Bitmap topToledo
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("topToledo", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
