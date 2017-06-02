using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 52419, "[A] OnAppearing called for previous pages in a tab's navigation when switching active tabs", PlatformAffected.Android)]
	public class Bugzilla52419 : TestTabbedPage
	{
		protected override void Init()
		{
			var nav1 = new NavigationPage();
			nav1.PushAsync(new Bugzilla52419Page1 { Title = "Tab 1" });
			var nav2 = new NavigationPage();
			nav2.PushAsync(new Bugzilla52419Page2 { Title = "Tab 2" });
			Children.Add(nav1);
			Children.Add(nav2);

			Appearing += (s, e) =>
			{
				System.Diagnostics.Debug.WriteLine("TabbedPage appearing");
			};
		}
	}

	class Bugzilla52419Page1 : ContentPage
	{
		string _guid = Guid.NewGuid().ToString();
		public Bugzilla52419Page1()
		{
			Content = new StackLayout
			{
				Children =
					{
						new Label
						{
							Text = "Page Guid: " + _guid
						},
						new Label
						{
							Text = "Click the button a couple times, switch to the second tab, and then back to the first. The debug output should only fire the Appearing event for the visible first tab."
						},
						new Button
						{
							Text = "Push new TabbedPage",
							Command = new Command(() => Navigation.PushAsync(new Bugzilla52419Page1()))
						}
					}
			};
			Appearing += OnAppearing;
		}

		void OnAppearing(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnAppearing: {0}", _guid);
		}
	}

	class Bugzilla52419Page2 : ContentPage
	{
		public Bugzilla52419Page2()
		{
			Content = new StackLayout
			{
				Children =
				{
					new Label
					{
						Text = "Other content"
					}
				}
			};
		}
	}
}