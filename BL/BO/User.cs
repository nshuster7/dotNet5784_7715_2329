using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int iD;
        /// <summary>
        /// Unique ID of User
        /// </summary>
        public int ID
        {
            get => iD;
            set
            {
                iD = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        /// <summary>
        /// Unique name of User
        /// </summary>

        private string? name;
        public string Name
        {
            get => name!;
            set
            {
                name = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }

        private string? password;
        /// <summary>
        /// Unique password of User
        /// </summary>
        public string Password
        {
            get => password!;
            set
            {
                password = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }

        /// <summary>
        /// Unique is maneger of user
        /// </summary>
        public bool IsManeger { get; set; }

        /// <summary>
        /// returns a string of the order's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => this.ToStringProperty();
    }
}
