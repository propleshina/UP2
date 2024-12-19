using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UP2;

namespace Function
{
    internal class Function
    {
        public int CalculateMaterial(
               int productTypeId,
               int materialTypeId,
               int productCount,
               double parameter1,
               double parameter2)
        {
            if (productTypeId <= 0 || materialTypeId <= 0 || productCount <= 0 || parameter1 <= 0 || parameter2 <= 0)
            {
                return -1;
            }
            var productType = getProductType(productTypeId);
            if (productType == null)
            {
                return -1;
            }
            var materialType = getMaterialType(materialTypeId);
            if (materialType == null)
            {
                return -1;
            }
            try
            {
                double materialPerUnit = parameter1 * parameter2 * Convert.ToDouble(productType.koef_of_type);
                double totalMaterial = materialPerUnit * productCount;
                double defectMultiplier = 1 + (double)materialType.procent_of_brak;
                totalMaterial *= defectMultiplier;
                return (int)Math.Ceiling(totalMaterial);
            }
            catch
            {
                return -1;
            }
        }

        private Product_type getProductType(int id)
        {
            using (var context = new Entities())
            {
                return context.Product_type.FirstOrDefault(ch => ch.ID == id);
            }
        }

        private Material_type getMaterialType(int id)
        {
            using (var context = new Entities())
            {
                return context.Material_type.FirstOrDefault(ch => ch.ID == id);
            }
        }

    }
}
