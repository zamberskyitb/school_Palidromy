using System;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
/*
 * celý projekt je dostupný na https://github.com/zamberskyitb/school_Palidromy
 * pravděpodobně nebude fungovat s Visual Studio 2017 či starší
 */
namespace App1
{
    public sealed partial class MainPage : Page
    { //aplikace nepoužívá Windows forms ale UWP+WinUI
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FileOpenPicker();
            dialog.FileTypeFilter.Add(".rtf");
            StorageFile file = await dialog.PickSingleFileAsync();
            if (file != null)
            {
                inRTB.Document.LoadFromStream(TextSetOptions.FormatRtf, await file.OpenAsync(FileAccessMode.Read));
                inRTB.Focus(FocusState.Programmatic); //po načtení se text nezobrazí, je nutno akutalizovat textbox např. změnou fokusu
            }
        }
        private void processWord_Click(object sender, RoutedEventArgs e)
        {
            process(" \n", ".?,");
        }

        private void processSentence_Click(object sender, RoutedEventArgs e)
        {
            process(".?", " \n,");
        }
        private void process(string v1, string v2)
        {
            inRTB.Document.GetText(TextGetOptions.UseLf, out string txt);
            outRTB.Document.SetText(TextSetOptions.UnicodeBidi, "");
            foreach (var s in txt.Split(v1.ToCharArray(),StringSplitOptions.RemoveEmptyEntries))
            {
                var s2 = s;
                if (s2.Length <= 1) continue;
                foreach (var c in v2) s2 = s2.Replace(c.ToString(), "");
                var success = true;
                var s3 = s2.ToLower().ToCharArray().Where(Char.IsLetterOrDigit).ToList();
                for(var i = 0; i < s2.Length; i++)
                {
                    if (char.ToLower(s3[i]) != char.ToLower(s3[s3.Count() - (i + 1)]))
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    outRTB.Document.GetText(TextGetOptions.UseLf, out string original);
                    outRTB.Document.SetText(TextSetOptions.UnicodeBidi, original + new string(s3.ToArray()) + "\n");
                }
            }
        }
    }
}