using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class ContactpersonType:ObservableObject
    {
        static ContactpersonType()
        {
            Types = new ObservableCollection<ContactpersonType>();
        }
        private static ObservableCollection<ContactpersonType> _types;

        public static ObservableCollection<ContactpersonType> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
