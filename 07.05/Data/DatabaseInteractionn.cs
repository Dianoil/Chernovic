using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07._05.Data
{
    public static class DatabaseInteractionn
    {

        private static Entities2 _Context = new Entities2();

        public static List<Material> GetMaterials()
        {
            return _Context.Material.ToList();
        }

        public static List<MaterialType> GetTypes()
        {
            return _Context.MaterialType.ToList();
        }

        public static Material GetMaterialById(int id)
        {
            return _Context.Material.Where(i => i.ID == id).Single();
        }

        public static void SaveChanges()
        {
            _Context.SaveChanges();
        }
    }
}
