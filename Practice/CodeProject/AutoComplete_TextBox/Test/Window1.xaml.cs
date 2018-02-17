using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Test
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            this.InitializeComponent();
            DataContext = new DirectoriesViewModel();

            ViewModel vm = this.Resources["vm"] as ViewModel;

            if (vm != null)
            {
                try
                {
                    if (System.IO.File.Exists(this.SettingsFileName))
                    {
                        // Create a new file stream for reading the XML file
                        using (
                            FileStream readFileStream = new FileStream(this.SettingsFileName, FileMode.Open,
                                FileAccess.Read, FileShare.Read))
                        {
                            // Create a new XmlSerializer instance with the type of the test class
                            XmlSerializer serializerObj = new XmlSerializer(typeof (ViewModel));

                            // Load the object saved above by using the Deserialize function
                            ViewModel viewModel = (ViewModel) serializerObj.Deserialize(readFileStream);

                            ((ViewModel) this.Resources["vm"]).SuggestEntries = viewModel.SuggestEntries;

                            // Cleanup
                            readFileStream.Close();
                        }
                    }
                }
                catch (Exception exp)
                {
                    Debug.Print(exp.ToString());
                }
            }
        }

        private string SettingsFileName
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ACTBFolderSettings.xml");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ViewModel vm = this.Resources["vm"] as ViewModel;

            if (vm != null)
            {
                if (vm.SuggestEntries.Count > 0)
                {
                    try
                    {
                        // Create a new file stream to write the serialized object to a file
                        using (TextWriter writeFileStream = new StreamWriter(this.SettingsFileName))
                        {
                            // Create a new XmlSerializer instance with the type of the test class
                            XmlSerializer serializerObj = new XmlSerializer(typeof (ViewModel));

                            serializerObj.Serialize(writeFileStream, vm);

                            writeFileStream.Close(); // Cleanup
                        }
                    }
                    catch (Exception exp)
                    {
                        Debug.Print(exp.ToString());
                    }
                }
            }
        }
        
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
