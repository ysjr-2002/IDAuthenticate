using Common.NotifyBase;
using IDReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Main.ViewModel
{
    class MainViewModel : PropertyNotifyObject
    {
        private MainViewModel()
        {
        }

        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance
        {
            get
            {
                return _instance;
            }
        }


        public Visibility WelcomeVisibility
        {
            get { return this.GetValue(s => s.WelcomeVisibility); }
            set { this.SetValue(s => s.WelcomeVisibility, value); }
        }

        public Visibility SZFVisibility
        {
            get { return this.GetValue(s => s.SZFVisibility); }
            set { this.SetValue(s => s.SZFVisibility, value); }
        }


        public Visibility PassVisibility
        {
            get { return this.GetValue(s => s.PassVisibility); }
            set { this.SetValue(s => s.PassVisibility, value); }
        }


        public void Init()
        {
            var open = SFZReader.Instance.Open();
            if (open)
            {

            }
            else
            {

            }
        }

        public void Work()
        {

        }

        public void Stop()
        {

        }

        private void OnReadIDCardInfo(IDCardInfo card)
        {

        }
    }
}
