using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.ViewModels.Settings
{
    public class DictionaryViewModel
    {
        public string DictionaryType { get; set; }

        public List<Category> Categories { get; set; }
        public TypesDictViewModel TypesDictModel { get; set; }
    }
}
