using System;
using System.Net;
using System.Windows;
using System.ComponentModel;

namespace CareWin8
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        private DateTimeOffset _timeObject;
        private string _iconURL;
        private string _content;
        private string _title;
        private string _id;
        private EntryType _type;

        public EntryType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }
        public string IconURL
        {
            get
            {
                return _iconURL;
            }
            set
            {
                if (value != _iconURL)
                {
                    _iconURL = value;
                    NotifyPropertyChanged("IconURL");
                }
            }
        }

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        public DateTimeOffset TimeObject
        {
            get
            {
                return _timeObject;
            }
            set
            {
                if (value != _timeObject)
                {
                    _timeObject = value;
                    NotifyPropertyChanged("TimeObject");
                }
            }
        }

        public string Time
        {
            get
            {
                string timePart = ExtHelpers.TimeObjectToString(_timeObject);
                return timePart;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
