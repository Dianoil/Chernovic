using _07._05.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _07._05.Windows
{
    public partial class MaterialLIst : Window
    {

        private int ItemsPerPage = 15;

        private int CurrentPageIndex = 0;

        public List<Material> FilteriedMaterialList;

        public MaterialLIst()
        {
            InitializeComponent();

            var typeMaterialList = DatabaseInteractionn.GetTypes();
            typeMaterialList.Insert(0, new MaterialType { Title = "Все" });

            cbFiltration.ItemsSource = typeMaterialList;

            FilteriedMaterialList = DatabaseInteractionn.GetMaterials();

            lwMaterial.ItemsSource = FilteriedMaterialList;

            GeneragePageButtons();
            ChangePage();

        }

        public void UpdateListMaterial()
        {
            if (tbSearch is null || cbFiltration is null || cbSort is null)
            {
                return;
            }

            FilteriedMaterialList = DatabaseInteractionn.GetMaterials();

            var MaterialCount = FilteriedMaterialList.Count;

            if (tbSearch.Text.Length != 0)
            {
                FilteriedMaterialList = FilteriedMaterialList.Where(m => m.Title.Contains(tbSearch.Text) || m.Description?.Contains(tbSearch.Text) == true).ToList();
            }

            switch (((ComboBoxItem)cbSort.SelectedItem).Content.ToString())
            {
                case "Наименование по возрастанию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderBy(m => m.Title).ToList();
                    break;
                case "Наименование по убыванию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderByDescending(m => m.Title).ToList();
                    break;
                case "Остаток на складе по возрастанию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderBy(m => m.CountInStock).ToList();
                    break;
                case "Остаток на складе по убыванию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderByDescending(m => m.CountInStock).ToList();
                    break;
                case "Стоимость по возрастанию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderBy(m => m.Cost).ToList();
                    break;
                case "Стоимость по убыванию":
                    FilteriedMaterialList = FilteriedMaterialList.OrderByDescending(m => m.Cost).ToList();
                    break;
            }

            if (((MaterialType)cbFiltration.SelectedItem).Title != "Все")
            {

                FilteriedMaterialList = FilteriedMaterialList.Where(m => m.MaterialType == ((MaterialType)cbFiltration.SelectedItem)).ToList();
            }

            var filteriedMaterialCount = FilteriedMaterialList.Count;

            tbMaterialCount.Text = $"Выведено {filteriedMaterialCount} из {MaterialCount}";

            lwMaterial.ItemsSource = FilteriedMaterialList.Skip(CurrentPageIndex * ItemsPerPage).Take(ItemsPerPage);

        }



        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListMaterial();

            GeneragePageButtons();
            ChangePage();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListMaterial();

            GeneragePageButtons();
            ChangePage();
        }

        private void cbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListMaterial();

            GeneragePageButtons();
            ChangePage();
        }

        private void lwMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 0)
            {
                ChangeMinCount.Visibility = Visibility.Hidden;
            }
            else
            {
                ChangeMinCount.Visibility = Visibility.Visible;
            }
        }


        private void ChangeMinCount_Click(object sender, RoutedEventArgs e)
        {
            new ChangeMinConuntWindow(lwMaterial.SelectedItems.Cast<Material>().ToList()).ShowDialog();

            UpdateListMaterial();
        }

        private void ChangeMaterial_Click(object sender, RoutedEventArgs e)
        {
            Material SelectedMaterial = DatabaseInteractionn.GetMaterialById(Convert.ToInt32((sender as Button).Tag));

            new NewMaterialOrChangeMaterialWindow().ShowDialog();
            UpdateListMaterial();
        }

        private void addNewMaterial_Click(object sender, RoutedEventArgs e)
        {
            new NewMaterialOrChangeMaterialWindow().ShowDialog();
            UpdateListMaterial();
        }

        public void GeneragePageButtons()
        {
            if (buttonStackPanel is null)
            {
                return;
            }

            buttonStackPanel.Children.Clear();

            //double count = FilteriedMaterialList.Count / ItemsPerPage;
            //count = (count % 1 > 0) ? count + 1 : count;

            int count = (FilteriedMaterialList.Count % ItemsPerPage > 0) ? (FilteriedMaterialList.Count / ItemsPerPage) + 1 : FilteriedMaterialList.Count / ItemsPerPage;

            for (int i = 0; i < count / 15; i++)
            {
                if (ItemsPerPage * i > FilteriedMaterialList.Count)
                {
                    return;
                }

                Button PageButton = new Button
                {
                    Width = 30,
                    Height = 30,
                    Content = i + 1,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                };

                if (i == 0)
                {
                    PageButton.BorderThickness = new Thickness(0, 0, 0, 5);
                }

                PageButton.Click += PageButton_Click;
                buttonStackPanel.Children.Add(PageButton);
            }
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex = Convert.ToInt32((sender as Button).Content.ToString()) - 1;
            ChangePage();
        }

        private void ChangePage()
        {
            if (buttonStackPanel is null || buttonStackPanel.Children.Count == 0)
            {
                return;
            }

            List<Button> PageButtonsList = buttonStackPanel.Children.OfType<Button>().ToList();

            if (Convert.ToInt32(PageButtonsList.Last().Content.ToString()) == CurrentPageIndex)
            {
                foreach (var i in PageButtonsList)
                {
                    i.Content = Convert.ToInt32(i.Content.ToString()) + 1;
                }
            }

            if (Convert.ToInt32(PageButtonsList.First().Content.ToString()) == CurrentPageIndex + 1 
                && CurrentPageIndex != 0)
            {
                foreach (var i in PageButtonsList)
                {
                    i.Content = Convert.ToInt32(i.Content.ToString()) - 1;
                }
            }

            foreach (var i in PageButtonsList)
            {
                i.BorderThickness = new Thickness(0);

                if (i.Content.ToString().Equals((CurrentPageIndex + 1).ToString()))
                {
                    i.BorderThickness = new Thickness(0, 0, 0, 5);
                }
            }

            UpdateListMaterial();
        }

        private void RightLwChange_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex = (CurrentPageIndex + 1) * ItemsPerPage > FilteriedMaterialList.Count ? CurrentPageIndex : ++CurrentPageIndex;
            ChangePage();
        }

        private void LeftLwChange_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex = CurrentPageIndex == 0 ? 0 : --CurrentPageIndex;
            ChangePage();
        }
    }
}
