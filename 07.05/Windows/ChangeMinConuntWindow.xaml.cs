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
    /// <summary>
    /// Логика взаимодействия для ChangeMinConuntWindow.xaml
    /// </summary>
    public partial class ChangeMinConuntWindow : Window
    {

        private List<Material> _MaterialList;
        public ChangeMinConuntWindow(List<Material> materials)
        {
            InitializeComponent();

            _MaterialList = materials;

            tbChangeMinCount.Text = materials.Max(m => m.MinCount).ToString();
        }

        private void btnChangeMinCount_Click(object sender, RoutedEventArgs e)
        {
            if (tbChangeMinCount.Text.Length == 0)
            {
                MessageBox.Show("Введите значение", "Ошибка", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            var newValue = tbChangeMinCount.Text;

            int newValueInt;

            try
            {
                newValueInt = Convert.ToInt32(newValue);
            }
            catch (Exception)
            {
                MessageBox.Show("Введите число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newValueInt < 0)
            {
                MessageBox.Show("Ведите число больше нуля");
                return;
            }

            foreach (var i in _MaterialList)
            {
                i.MinCount = newValueInt;
                DatabaseInteractionn.SaveChanges();
            }
            MessageBox.Show("Минимальное количество изменено");
            this.Close();
        }
    }
}
