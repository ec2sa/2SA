using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.ViewModels.Settings
{
    public class TypesDictViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Type> Types { get; set; }
        public int? SelectedCategory { get; set; }
    }
}
