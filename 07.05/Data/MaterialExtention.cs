using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07._05.Data
{
    public partial class Material
    {

        public string ImagePath
        {
            get 
            {
                if (Image is null)
                {
                    return "/Images/picture.png";
                }
                else
                {
                    return Directory.GetCurrentDirectory() + Image.ToString();
                }
            }
        }

        public string Suppliers
        {
            get
            {
                var suppliers = String.Join(",", Supplier.Select(s => s.Title).ToList());

                if (suppliers.Length == 0)
                {
                    return "Нет поставщиков";
                }
                else
                {
                    return suppliers;
                }
            }
        }

        public string BackgroundColor
        {
            get 
            {
                if (MinCount > CountInStock)
                {
                    return "#f19292";
                }
                else if (CountInStock >= MinCount * 3)
                {
                    return "#ffba01";
                }
                else
                {
                    return "#ffffff";
                }
            }
        }

    }
}
