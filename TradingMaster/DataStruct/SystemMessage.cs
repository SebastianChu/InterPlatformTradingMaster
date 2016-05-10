using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingMaster
{
    public class SystemMessage : INotifyPropertyChanged
    {
        public static readonly string FeedBackTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";
        private static int _NextId = 1;
        public static string GetNextId()
        {
            string strNextId = _NextId.ToString();
            strNextId = strNextId.PadLeft(8, '0');
            _NextId++;
            return strNextId;
        }
        /// <summary>
        /// 消息编号
        /// </summary>
        private string _MessageID;

        public string MessageID
        {
            get { return _MessageID; }
            set
            {
                _MessageID = value;
                OnPropertyChanged("MessageID");
            }
        }
        /// <summary>
        /// 反馈时间
        /// </summary>
        private string _FeedbackTime;

        public string FeedbackTime
        {
            get { return _FeedbackTime; }
            set
            {
                _FeedbackTime = value;
                OnPropertyChanged("FeedbackTime");
            }
        }
        /// <summary>
        /// 反馈信息
        /// </summary>
        private string _FeedbackMessage;

        public string FeedbackMessage
        {
            get { return _FeedbackMessage; }
            set
            {
                _FeedbackMessage = value;
                OnPropertyChanged("FeedbackMessage");
            }
        }

        /// <summary>
        /// 级别
        /// </summary>
        private string _MessageLevel;

        public string MessageLevel
        {
            get { return _MessageLevel; }
            set
            {
                _MessageLevel = value;
                OnPropertyChanged("MessageLevel");
            }
        }

        /// <summary>
        /// 类别
        /// </summary>
        private string _MessageClass;

        public string MessageClass
        {
            get { return _MessageClass; }
            set
            {
                _MessageClass = value;
                OnPropertyChanged("MessageClass");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}