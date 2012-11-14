using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CareWin8
{
    public class FriendViewModel : INotifyPropertyChanged
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private String _id;
        public String ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }

        private String _description;
        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private String _avatar;
        public String Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                NotifyPropertyChanged("Avatar");
            }
        }

        private String _avatar2;
        public String Avatar2
        {
            get { return _avatar2; }
            set
            {
                _avatar2 = value;
                NotifyPropertyChanged("Avatar2");
            }
        }


        #region INotifyPropertyChanged实现
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
