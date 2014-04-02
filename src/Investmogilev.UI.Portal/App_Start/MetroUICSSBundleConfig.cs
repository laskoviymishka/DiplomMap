// // -----------------------------------------------------------------------
// // <copyright file="MetroUICSSBundleConfig.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

#region Using

using Investmogilev.UI.Portal;
using WebActivatorEx;

#endregion

[assembly: PostApplicationStartMethod(typeof (MetroUICSSBundleConfig), "RegisterBundles")]

namespace Investmogilev.UI.Portal
{
	#region Using

	using System.Web.Optimization;

	#endregion

	public class MetroUICSSBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/metro-ui/css") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/metro-ui") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, ASP.Net will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
			BundleTable.Bundles.Add(
				new ScriptBundle("~/bundles/metro-ui").Include("~/Scripts/metro-ui/jquery.ui.widget.js")
					.Include("~/Scripts/metro-ui/metro-*"));
			BundleTable.Bundles.Add(
				new StyleBundle("~/Content/metro-ui/css").Include("~/Content/metro-ui/css/metro-bootstrap.css",
					"~/Content/metro-ui/css/metro-bootstrap-responsive.css"));
		}
	}
}