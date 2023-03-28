using Android.OS;
using Contracts;
using MassTransit;

namespace MauiAppTest
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IServiceProvider _provider;

        public MainPage(IServiceProvider provider)
        {
            _provider = provider;
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                var client = _provider.GetRequiredService<IRequestClient<ServiceManagerRequest>>();
                var result = await client.GetResponse<ServiceManagerResult>(new ServiceManagerRequest
                    { Action = $"OK - {DateTime.Now}" });
            }
            catch (Exception ex)
            {
                CounterBtn.Text = ex.Message;
                return;
            }

            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}