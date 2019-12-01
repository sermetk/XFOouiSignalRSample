using System.Linq;
using Xamarin.Forms;

namespace OouiSignalRSample.Behaviors
{
    public class ListViewScrollBehaviour : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemAppearing += OnItemAppearing;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemAppearing -= OnItemAppearing;
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var listView = (ListView)sender;
            if (listView.ItemsSource != null)
            {
                var lastItem = listView.ItemsSource.Cast<object>().LastOrDefault();
                listView.ScrollTo(lastItem, ScrollToPosition.End, false);
            }
        }
    }
}
