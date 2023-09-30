using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Extentions
{
    public static class DocumentRepositoriesExtentions
    {
        public static IQueryable<Product> SearchProducts(this IQueryable<Product> products,string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerCaseTerm = searchTerm.ToLower();
                products = products.Where(c=>c.Title.ToLower() == lowerCaseTerm );
                
            }
            return products;
        }
        public static IQueryable<Publication> SearchPublication(this IQueryable<Publication> publications,string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerCaseTerm = searchTerm.ToLower();
                publications = publications.Where(c=>c.Title.ToLower() == lowerCaseTerm );
                
            }
            return publications;
        }
    }
}
