using System.Windows.Controls;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDNA.WPF.Blank
{
    /// <summary>
    /// Interaction logic for MyAddinControl.xaml
    /// </summary>
    public partial class MyAddinControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MyAddinControl()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// When the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Get the number of selected objects
            var count = 0;
            Application.ActiveModel?.SelectedObjects(objects => count = objects?.Count ?? 0);

            // Let the user know
            Application.ShowMessageBox($"Looks like you have {count} objects selected");
        }
    }
}
