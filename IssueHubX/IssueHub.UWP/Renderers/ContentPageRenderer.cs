using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ContentPage), typeof(IssueHub.UWP.Renderers.ContentPageRenderer))]

namespace IssueHub.UWP.Renderers
{
    public class ContentPageRenderer : PageRenderer
    {
        /*
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var toolbarItems = ((ContentPage)Element).ToolbarItems.OrderBy(w => w.Priority);
            var navigationItem = NavigationController.TopViewController.NavigationItem;
            var rightItems = new List<UIBarButtonItem>();
            var leftItems = new List<UIBarButtonItem>();

            foreach (var item in toolbarItems)
            {
                // Priority がマイナスだったらナビゲーションバーの左側に配置する
                if (item.Priority < 0)
                {
                    leftItems.Add(item.ToUIBarButtonItem());
                }
                else
                {
                    rightItems.Add(item.ToUIBarButtonItem());
                }
            }

            navigationItem.SetRightBarButtonItems(rightItems.ToArray(), animated);
            navigationItem.SetLeftBarButtonItems(leftItems.ToArray(), animated);
        }
        */
    }
}