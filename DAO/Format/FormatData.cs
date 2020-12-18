using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAO.Format
{
    public class FormatData
    {
        public static void FormatProduct(ProductEntity item)
        {
            item.ListAvatar = item.Avatar.ToList();
            item.FirstAvatar = item.ListAvatar?.FirstOrDefault();
        }
        public static void FormatProduct(List<ProductEntity> items)
        {
            foreach (var item in items)
            {
                FormatProduct(item);
            }
        }
    }
}
