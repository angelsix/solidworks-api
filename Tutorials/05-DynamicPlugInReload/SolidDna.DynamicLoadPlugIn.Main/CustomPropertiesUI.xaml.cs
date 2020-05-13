using AngelSix.SolidDna;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDna.DynamicLoadPlugIn
{
    /// <summary>
    /// Interaction logic for CustomPropertiesUI.xaml
    /// </summary>
    public partial class CustomPropertiesUI : UserControl
    {
        #region Private Members

        private const string CustomPropertyDescription = "Description";
        private const string CustomPropertyStatus = "Status";
        private const string CustomPropertyRevision = "Revision";
        private const string CustomPropertyPartNumber = "PartNo";
        private const string CustomPropertyManufacturingInformation = "Manufacturing Information";
        private const string CustomPropertyLength = "Length";
        private const string CustomPropertyFinish = "Finish";
        private const string CustomPropertyPurchaseInformation = "Purchase Information";
        private const string CustomPropertySupplierName = "Supplier";
        private const string CustomPropertySupplierCode = "Supplier Number / Code";
        private const string CustomPropertyNote = "Note";

        private const string ManufacturingWeld = "WELD";
        private const string ManufacturingAssembly = "ASSEMBLY";
        private const string ManufacturingPlasma = "PLASMA";
        private const string ManufacturingLaser = "LASER";
        private const string ManufacturingPurchase = "PURCHASE";
        private const string ManufacturingLathe = "LATHE";
        private const string ManufacturingDrill = "DRILL";
        private const string ManufacturingFold = "FOLD";
        private const string ManufacturingRoll = "ROLL";
        private const string ManufacturingSaw = "SAW";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomPropertiesUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Startup

        /// <summary>
        /// Fired when the control is fully loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // By default show the No Part open screen
            NoPartContent.Visibility = System.Windows.Visibility.Visible;
            MainContent.Visibility = System.Windows.Visibility.Hidden;

            // Listen out for the active model changing
            Application.ActiveModelInformationChanged += Application_ActiveModelInformationChanged;
        }

        #endregion

        #region Model Events

        /// <summary>
        /// Fired when the active SolidWorks model is changed
        /// </summary>
        /// <param name="obj"></param>
        private void Application_ActiveModelInformationChanged(Model obj)
        {
            ReadDetails();
        }

        #endregion

        #region Read Details

        /// <summary>
        /// Reads all the details from the active SolidWorks model
        /// </summary>
        private void ReadDetails()
        {
            ThreadHelpers.RunOnUIThread(() =>
            {
                // Get the active model
                var model = Application.ActiveModel;

                // If we have no model, or the model is not a part
                // then show the No Part screen and return
                if (model == null || (!model.IsPart && !model.IsAssembly))
                {
                    // Show No Part screen
                    NoPartContent.Visibility = System.Windows.Visibility.Visible;
                    MainContent.Visibility = System.Windows.Visibility.Hidden;

                    return;
                }

                // If we got here, we have a part

                // Listen out for selection changes
                model.SelectionChanged += Model_SelectionChanged;

                // Show the main content
                NoPartContent.Visibility = System.Windows.Visibility.Hidden;
                MainContent.Visibility = System.Windows.Visibility.Visible;

                // Get custom properties
                // Description
                DescriptionText.Text = model.GetCustomProperty(CustomPropertyDescription);

                // Status
                StatusText.Text = model.GetCustomProperty(CustomPropertyStatus, resolved: true);

                // Revision
                RevisionText.Text = model.GetCustomProperty(CustomPropertyRevision, resolved: true);

                // Part Number
                PartNumberText.Text = model.GetCustomProperty(CustomPropertyPartNumber, resolved: true);

                // Manufacturing Information

                // Clear previous checks
                MaterialWeldCheck.IsChecked = MaterialAssemblyCheck.IsChecked = MaterialPlasmaCheck.IsChecked =
                    MaterialPurchaseCheck.IsChecked = MaterialLatheCheck.IsChecked = MaterialLaserCheck.IsChecked = MaterialDrillCheck.IsChecked =
                    MaterialFoldCheck.IsChecked = MaterialRollCheck.IsChecked = MaterialSawCheck.IsChecked = false;

                // Read in value
                var manufacturingInfo = model.GetCustomProperty(CustomPropertyManufacturingInformation);

                // If we have some property, parse it
                if (!string.IsNullOrWhiteSpace(manufacturingInfo))
                {
                    // Remove whitespaces, capitalize and split by ,
                    foreach (var part in manufacturingInfo.Replace(" ", "").ToUpper().Split(','))
                    {
                        switch (part)
                        {
                            case ManufacturingWeld:
                                MaterialWeldCheck.IsChecked = true;
                                break;
                            case ManufacturingAssembly:
                                MaterialAssemblyCheck.IsChecked = true;
                                break;

                            case ManufacturingPlasma:
                                MaterialPlasmaCheck.IsChecked = true;
                                break;

                            case ManufacturingLaser:
                                MaterialLaserCheck.IsChecked = true;
                                break;

                            case ManufacturingPurchase:
                                MaterialPurchaseCheck.IsChecked = true;
                                break;

                            case ManufacturingLathe:
                                MaterialLatheCheck.IsChecked = true;
                                break;

                            case ManufacturingDrill:
                                MaterialDrillCheck.IsChecked = true;
                                break;

                            case ManufacturingFold:
                                MaterialFoldCheck.IsChecked = true;
                                break;

                            case ManufacturingRoll:
                                MaterialRollCheck.IsChecked = true;
                                break;

                            case ManufacturingSaw:
                                MaterialSawCheck.IsChecked = true;
                                break;
                        }
                    }
                }


                // Length
                SheetMetalLengthText.Text = model.GetCustomProperty(CustomPropertyLength);
                SheetMetalLengthEvaluatedText.Text = model.GetCustomProperty(CustomPropertyLength, resolved: true);

                // Finish
                var finish = model.GetCustomProperty(CustomPropertyFinish);

                // Clear the selection first
                FinishList.SelectedIndex = -1;

                // Try and find matching item
                foreach (var item in FinishList.Items)
                {
                    // Check if the combo box item has the same name
                    if ((string)((ComboBoxItem)item).Content == finish)
                    {
                        // If so select it
                        FinishList.SelectedItem = item;
                        break;
                    }
                }

                // Purchase Information
                var purchaseInfo = model.GetCustomProperty(CustomPropertyPurchaseInformation);

                // Clear the selection first
                PurchaseInformationList.SelectedIndex = -1;

                // Try and find matching item
                foreach (var item in PurchaseInformationList.Items)
                {
                    // Check if the combo box item has the same name
                    if ((string)((ComboBoxItem)item).Content == purchaseInfo)
                    {
                        // If so select it
                        PurchaseInformationList.SelectedItem = item;
                        break;
                    }
                }

                // Supplier Name
                SupplierNameText.Text = model.GetCustomProperty(CustomPropertySupplierName);

                // Supplier Code
                SupplierCodeText.Text = model.GetCustomProperty(CustomPropertySupplierCode);

                // Note
                NoteText.Text = model.GetCustomProperty(CustomPropertyNote);

                // Mass
                MassText.Text = model.MassProperties?.MassInMetric();

                // Get all materials
                var materials = Application.GetMaterials();
                materials.Insert(0, new Material { Name = "Remove Material", Classification = "Not specified", DatabaseFileFound = false });

                RawMaterialList.ItemsSource = materials;
                RawMaterialList.DisplayMemberPath = "DisplayName";

                // Clear selection
                RawMaterialList.SelectedIndex = -1;

                // Select existing material
                var existingMaterial = model.GetMaterial();

                // If we have a material
                if (existingMaterial != null)
                    RawMaterialList.SelectedItem = materials?.FirstOrDefault(f => f.Database == existingMaterial.Database && f.Name == existingMaterial.Name);
            });
        }

        private void Model_SelectionChanged()
        {
            Application?.ActiveModel?.SelectedObjects((objects) =>
            {
                var haveDimension = objects.Any(f => f.IsDimension);

                ThreadHelpers.RunOnUIThread(() =>
                {
                    LengthButton.IsEnabled = haveDimension;
                });
            });
        }

        #endregion

        #region Set Details

        /// <summary>
        /// Sets all the details to the active SolidWorks model
        /// </summary>
        public void SetDetails()
        {
            var model = Application.ActiveModel;

            // Check we have a part
            if (model == null || !model.IsPart)
                return;

            // Description
            model.SetCustomProperty(CustomPropertyDescription, DescriptionText.Text);

            // If user doesn't have a material selected, clear it
            if (RawMaterialList.SelectedIndex < 0)
                model.SetMaterial(null);
            // Otherwise set the material to the selected one
            else
                model.SetMaterial((Material)RawMaterialList.SelectedItem);

            // Manufacturing Info
            var manufacturingInfo = new List<string>();

            if (MaterialWeldCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingWeld);
            if (MaterialAssemblyCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingAssembly);
            if (MaterialPlasmaCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingPlasma);
            if (MaterialLaserCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingLaser);
            if (MaterialPurchaseCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingPurchase);
            if (MaterialLatheCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingLathe);
            if (MaterialDrillCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingDrill);
            if (MaterialFoldCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingFold);
            if (MaterialRollCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingRoll);
            if (MaterialSawCheck.IsChecked.Value)
                manufacturingInfo.Add(ManufacturingSaw);

            // Set manufacturing info
            model.SetCustomProperty(CustomPropertyManufacturingInformation, string.Join(",", manufacturingInfo));

            // Length
            model.SetCustomProperty(CustomPropertyLength, SheetMetalLengthText.Text);

            // Finish
            model.SetCustomProperty(CustomPropertyFinish, (string)((ComboBoxItem)FinishList.SelectedValue)?.Content);

            // Purchase Info
            model.SetCustomProperty(CustomPropertyPurchaseInformation, (string)((ComboBoxItem)PurchaseInformationList.SelectedValue)?.Content);

            // Suppler Name
            model.SetCustomProperty(CustomPropertySupplierName, SupplierNameText.Text);

            // Supplier Code
            model.SetCustomProperty(CustomPropertySupplierCode, SupplierCodeText.Text);

            // Note
            model.SetCustomProperty(CustomPropertyNote, NoteText.Text);

            // Re-read details to confirm they are correct
            ReadDetails();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Called when the read button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReadDetails();
        }

        /// <summary>
        /// Called when the reset button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Clear all values
            DescriptionText.Text = string.Empty;
            StatusText.Text = string.Empty;
            RevisionText.Text = string.Empty;
            PartNumberText.Text = string.Empty;

            RawMaterialList.SelectedIndex = -1;

            MaterialWeldCheck.IsChecked = MaterialAssemblyCheck.IsChecked = MaterialPlasmaCheck.IsChecked =
                MaterialPurchaseCheck.IsChecked = MaterialLatheCheck.IsChecked = MaterialLaserCheck.IsChecked = MaterialDrillCheck.IsChecked =
                MaterialFoldCheck.IsChecked = MaterialRollCheck.IsChecked = MaterialSawCheck.IsChecked = false;

            SheetMetalLengthText.Text = string.Empty;
            SheetMetalLengthEvaluatedText.Text = string.Empty;

            FinishList.SelectedIndex = -1;
            PurchaseInformationList.SelectedIndex = -1;

            SupplierNameText.Text = string.Empty;
            SupplierCodeText.Text = string.Empty;
            NoteText.Text = string.Empty;
        }

        /// <summary>
        /// Called when the apply button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetDetails();
        }

        #endregion

        private void MaterialAssemblyCheck_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Unckech plasma
            MaterialPlasmaCheck.IsChecked = false;
        }

        private void MaterialPlasmaCheck_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Uncheck assembly
            MaterialAssemblyCheck.IsChecked = false;
        }

        private void LengthButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.ActiveModel?.SelectedObjects((objects) =>
            {
                // Get the newest dimension
                var lastDimension = objects.LastOrDefault(f => f.IsDimension);

                // Double check we have one
                if (lastDimension == null)
                    return;

                var dimensionSelectionName = string.Empty;

                // Get the dimension name
                lastDimension.AsDimension((dimension) => dimensionSelectionName = dimension.SelectionName);

                // Set the length button text
                ThreadHelpers.RunOnUIThread(() =>
                {
                    SheetMetalLengthText.Text = $"\"{dimensionSelectionName}\"";
                });
            });
        }
    }
}
