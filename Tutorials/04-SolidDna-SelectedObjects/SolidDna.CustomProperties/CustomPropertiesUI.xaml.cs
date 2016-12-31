using AngelSix.SolidDna;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;

namespace SolidDna.CustomProperties
{
    /// <summary>
    /// Interaction logic for CustomPropertiesUI.xaml
    /// </summary>
    public partial class CustomPropertiesUI : UserControl
    {
        #region Private Members

        private const string mCustomPropertyDescription = "Description";
        private const string mCustomPropertyStatus = "Status";
        private const string mCustomPropertyRevision = "Revision";
        private const string mCustomPropertyPartNumber = "PartNo";
        private const string mCustomPropertyManufacturingInformation = "Manufacturing Information";
        private const string mCustomPropertyLength = "Length";
        private const string mCustomPropertyFinish = "Finish";
        private const string mCustomPropertyPurchaseInformation = "Purchase Information";
        private const string mCustomPropertySupplierName = "Supplier";
        private const string mCustomPropertySupplierCode = "Supplier Number / Code";
        private const string mCustomPropertyNote = "Note";

        private const string mManufacturingWeld = "WELD";
        private const string mManufacturingAssembly = "ASSEMBLY";
        private const string mManufacturingPlasma = "PLASMA";
        private const string mManufacturingLaser = "LASER";
        private const string mManufacturingPurchase = "PURCHASE";
        private const string mManufacturingLathe = "LATHE";
        private const string mManufacturingDrill = "DRILL";
        private const string mManufacturingFold = "FOLD";
        private const string mManufacturingRoll = "ROLL";
        private const string mManufacturingSaw = "SAW";

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
            this.NoPartContent.Visibility = System.Windows.Visibility.Visible;
            this.MainContent.Visibility = System.Windows.Visibility.Hidden;

            // Listen out for the active model changing
            Dna.Application.ActiveModelInformationChanged += Application_ActiveModelInformationChanged;
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
                var model = Dna.Application.ActiveModel;

                // If we have no model, or the model is not a part
                // then show the No Part screen and return
                if (model == null || (!model.IsPart && !model.IsAssembly))
                {
                    // Show No Part screen
                    this.NoPartContent.Visibility = System.Windows.Visibility.Visible;
                    this.MainContent.Visibility = System.Windows.Visibility.Hidden;

                    return;
                }

                // If we got here, we have a part

                // Listen out for selection changes
                model.SelectionChanged += Model_SelectionChanged;

                // Show the main content
                this.NoPartContent.Visibility = System.Windows.Visibility.Hidden;
                this.MainContent.Visibility = System.Windows.Visibility.Visible;

                // Get custom properties
                // Description
                this.DescriptionText.Text = model.GetCustomProperty(mCustomPropertyDescription);

                // Status
                this.StatusText.Text = model.GetCustomProperty(mCustomPropertyStatus, resolved: true);

                // Revision
                this.RevisionText.Text = model.GetCustomProperty(mCustomPropertyRevision, resolved: true);

                // Part Number
                this.PartNumberText.Text = model.GetCustomProperty(mCustomPropertyPartNumber, resolved: true);

                // Manufacturing Information

                // Clear previous checks
                this.MaterialWeldCheck.IsChecked = this.MaterialAssemblyCheck.IsChecked = this.MaterialPlasmaCheck.IsChecked =
                    this.MaterialPurchaseCheck.IsChecked = this.MaterialLatheCheck.IsChecked = this.MaterialLaserCheck.IsChecked = this.MaterialDrillCheck.IsChecked =
                    this.MaterialFoldCheck.IsChecked = this.MaterialRollCheck.IsChecked = this.MaterialSawCheck.IsChecked = false;

                // Read in value
                var manufacturingInfo = model.GetCustomProperty(mCustomPropertyManufacturingInformation);

                // If we have some property, parse it
                if (!string.IsNullOrWhiteSpace(manufacturingInfo))
                {
                    // Remove whitespaces, capitalize and split by ,
                    foreach (var part in manufacturingInfo.Replace(" ", "").ToUpper().Split(','))
                    {
                        switch (part)
                        {
                            case mManufacturingWeld:
                                this.MaterialWeldCheck.IsChecked = true;
                                break;
                            case mManufacturingAssembly:
                                this.MaterialAssemblyCheck.IsChecked = true;
                                break;

                            case mManufacturingPlasma:
                                this.MaterialPlasmaCheck.IsChecked = true;
                                break;

                            case mManufacturingLaser:
                                this.MaterialLaserCheck.IsChecked = true;
                                break;

                            case mManufacturingPurchase:
                                this.MaterialPurchaseCheck.IsChecked = true;
                                break;

                            case mManufacturingLathe:
                                this.MaterialLatheCheck.IsChecked = true;
                                break;

                            case mManufacturingDrill:
                                this.MaterialDrillCheck.IsChecked = true;
                                break;

                            case mManufacturingFold:
                                this.MaterialFoldCheck.IsChecked = true;
                                break;

                            case mManufacturingRoll:
                                this.MaterialRollCheck.IsChecked = true;
                                break;

                            case mManufacturingSaw:
                                this.MaterialSawCheck.IsChecked = true;
                                break;
                        }
                    }
                }


                // Length
                this.SheetMetalLengthText.Text = model.GetCustomProperty(mCustomPropertyLength);
                this.SheetMetalLengthEvaluatedText.Text = model.GetCustomProperty(mCustomPropertyLength, resolved: true);

                // Finish
                var finish = model.GetCustomProperty(mCustomPropertyFinish);

                // Clear the selection first
                this.FinishList.SelectedIndex = -1;

                // Try and find matching item
                foreach (var item in this.FinishList.Items)
                {
                    // Check if the combo box item has the same name
                    if ((string)((ComboBoxItem)item).Content == finish)
                    {
                        // If so select it
                        this.FinishList.SelectedItem = item;
                        break;
                    }
                }

                // Purchase Information
                var purchaseInfo = model.GetCustomProperty(mCustomPropertyPurchaseInformation);

                // Clear the selection first
                this.PurchaseInformationList.SelectedIndex = -1;

                // Try and find matching item
                foreach (var item in this.PurchaseInformationList.Items)
                {
                    // Check if the combo box item has the same name
                    if ((string)((ComboBoxItem)item).Content == purchaseInfo)
                    {
                        // If so select it
                        this.PurchaseInformationList.SelectedItem = item;
                        break;
                    }
                }

                // Supplier Name
                this.SupplierNameText.Text = model.GetCustomProperty(mCustomPropertySupplierName);

                // Supplier Code
                this.SupplierCodeText.Text = model.GetCustomProperty(mCustomPropertySupplierCode);

                // Note
                this.NoteText.Text = model.GetCustomProperty(mCustomPropertyNote);

                // Mass
                this.MassText.Text = model.MassProperties?.MassInMetric();

                // Get all materials
                var materials = Dna.Application.GetMaterials();
                materials.Insert(0, new Material { Name = "Remove Material", Classification = "Not specified", DatabaseFileFound = false });

                this.RawMaterialList.ItemsSource = materials;
                this.RawMaterialList.DisplayMemberPath = "DisplayName";

                // Clear selection
                this.RawMaterialList.SelectedIndex = -1;

                // Select existing material
                var existingMaterial = model.GetMaterial();

                // If we have a material
                if (existingMaterial != null)
                    this.RawMaterialList.SelectedItem = materials?.FirstOrDefault(f => f.Database == existingMaterial.Database && f.Name == existingMaterial.Name);
            });
        }

        private void Model_SelectionChanged()
        {
            Dna.Application.ActiveModel?.SelectedObjects((objects) =>
            {
                var haveDimension = objects.Any(f => f.IsDimension);

                ThreadHelpers.RunOnUIThread(() =>
                {
                    this.LengthButton.IsEnabled = haveDimension;
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
            var model = Dna.Application.ActiveModel;

            // Check we have a part
            if (model == null || !model.IsPart)
                return;

            // Description
            model.SetCustomProperty(mCustomPropertyDescription, this.DescriptionText.Text);

            // If user doesn't have a material selected, clear it
            if (this.RawMaterialList.SelectedIndex < 0)
                model.SetMaterial(null);
            // Otherwise set the material to the selected one
            else
                model.SetMaterial((Material)this.RawMaterialList.SelectedItem);

            // Manufacturing Info
            var manufacturingInfo = new List<string>();

            if (this.MaterialWeldCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingWeld);
            if (this.MaterialAssemblyCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingAssembly);
            if (this.MaterialPlasmaCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingPlasma);
            if (this.MaterialLaserCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingLaser);
            if (this.MaterialPurchaseCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingPurchase);
            if (this.MaterialLatheCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingLathe);
            if (this.MaterialDrillCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingDrill);
            if (this.MaterialFoldCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingFold);
            if (this.MaterialRollCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingRoll);
            if (this.MaterialSawCheck.IsChecked.Value)
                manufacturingInfo.Add(mManufacturingSaw);

            // Set manufacturing info
            model.SetCustomProperty(mCustomPropertyManufacturingInformation, string.Join(",", manufacturingInfo));

            // Length
            model.SetCustomProperty(mCustomPropertyLength, this.SheetMetalLengthText.Text);

            // Finish
            model.SetCustomProperty(mCustomPropertyFinish, (string)((ComboBoxItem)this.FinishList.SelectedValue)?.Content);

            // Purchase Info
            model.SetCustomProperty(mCustomPropertyPurchaseInformation, (string)((ComboBoxItem)this.PurchaseInformationList.SelectedValue)?.Content);

            // Suppler Name
            model.SetCustomProperty(mCustomPropertySupplierName, this.SupplierNameText.Text);

            // Supplier Code
            model.SetCustomProperty(mCustomPropertySupplierCode, this.SupplierCodeText.Text);

            // Note
            model.SetCustomProperty(mCustomPropertyNote, this.NoteText.Text);

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
            this.DescriptionText.Text = string.Empty;
            this.StatusText.Text = string.Empty;
            this.RevisionText.Text = string.Empty;
            this.PartNumberText.Text = string.Empty;

            this.RawMaterialList.SelectedIndex = -1;

            this.MaterialWeldCheck.IsChecked = this.MaterialAssemblyCheck.IsChecked = this.MaterialPlasmaCheck.IsChecked =
                this.MaterialPurchaseCheck.IsChecked = this.MaterialLatheCheck.IsChecked = this.MaterialLaserCheck.IsChecked = this.MaterialDrillCheck.IsChecked =
                this.MaterialFoldCheck.IsChecked = this.MaterialRollCheck.IsChecked = this.MaterialSawCheck.IsChecked = false;

            this.SheetMetalLengthText.Text = string.Empty;
            this.SheetMetalLengthEvaluatedText.Text = string.Empty;

            this.FinishList.SelectedIndex = -1;
            this.PurchaseInformationList.SelectedIndex = -1;

            this.SupplierNameText.Text = string.Empty;
            this.SupplierCodeText.Text = string.Empty;
            this.NoteText.Text = string.Empty;
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
            this.MaterialPlasmaCheck.IsChecked = false;
        }

        private void MaterialPlasmaCheck_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Uncheck assembly
            this.MaterialAssemblyCheck.IsChecked = false;
        }

        private void LengthButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dna.Application.ActiveModel?.SelectedObjects((objects) =>
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
                    this.SheetMetalLengthText.Text = $"\"{dimensionSelectionName}\"";
                });
            });
        }
    }
}
