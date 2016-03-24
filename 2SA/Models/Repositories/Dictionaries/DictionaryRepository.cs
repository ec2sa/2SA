using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Controllers;

namespace eArchiver.Models.Repositories.Dictionaries
{
    public class DictionaryRepository
    {
        EArchiverDataContext _context = new EArchiverDataContext();

        public IQueryable<Type2> GetTypes2()
        {
            return _context.Type2s.Where(s => s.ClientID == AppContext.GetCID()).OrderBy(t => t.Name);
        }

        public IQueryable<Category> GetCategories()
        {
            return _context.Categories.Where(s => s.ClientID == AppContext.GetCID()).OrderBy(c => c.Name);
        }

        public IQueryable<Type> GetTypes()
        {
            return _context.Types.Where(s => s.ClientID == AppContext.GetCID()).OrderBy(t => t.Name);
        }

        public IQueryable<Type> GetTypes(int categoryID)
        {
            return GetTypes().Where(t => t.CategoryID == categoryID);
        }

        public IQueryable<Sender> GetSenders()
        {
            return _context.Senders.Where(s => s.ClientID == AppContext.GetCID()).OrderBy(s => s.Company);
        }

        public Sender GetSender(int senderID)
        {
            return GetSenders().Single(s => s.SenderID == senderID);
        }

        public void CreateSender(Sender sender)
        {
            _context.Senders.InsertOnSubmit(sender);
        }

        public bool CategoryExists(string categoryName)
        {
            try
            {
                Category category = GetCategories().Where(c => c.Name.ToLower() == categoryName.ToLower()).Single();
                if (category == null)
                    throw new Exception();
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Type2Exists(string type2Name)
        {
            try
            {
                Type2 type2 = GetTypes2().Where(c => c.Name.ToLower() == type2Name.ToLower()).Single();
                if (type2 == null)
                    throw new Exception();
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateCategory(string categoryName)
        {
            Category newCategory = new Category()
            {
                Name=categoryName,
                ClientID = AppContext.GetCID()
            };

            _context.Categories.InsertOnSubmit(newCategory);
            
        }

        public void CreateType2(string type2Name)
        {
            Type2 newRype2 = new Type2()
            {
                Name = type2Name,
                ClientID = AppContext.GetCID()
            };

            _context.Type2s.InsertOnSubmit(newRype2);

        }

        public bool TypeExists(string typeName, int categoryID)
        {
            try
            {
                Type type = GetTypes(categoryID).Where(t => t.Name.ToLower() == typeName.ToLower()).Single();

                if (type == null)
                    throw new Exception();
                else
                    return true;
            }
            catch
            {
                return false;
            }

        }

        public void CreateType(string typeName, int categoryID)
        {
            Type newType = new Type()
            {
                CategoryID = categoryID,
                Name = typeName,
                ClientID = AppContext.GetCID()
            };

            _context.Types.InsertOnSubmit(newType);

        }

        public void DeleteCategory(int categoryID)
        {
            _context.DeleteCategory(categoryID);
        }

        public void DeleteType2(int type2ID)
        {
            _context.DeleteType2(type2ID);
        }

        public void DeleteType(int typeID)
        {
            _context.DeleteType(typeID);
        }

        public pOCRGetConfigurationResult GetOCRConfiguration()
        {
           return _context.OCRGetConfiguration().FirstOrDefault();   
        }

        public bool SaveOCRConfiguration(string startHour, string endHour,bool enabled)
        {
            try
            {
                _context.OCRSaveConfiguration(startHour, endHour,enabled?"1":"0");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }
    }
}
