using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OUserVM : BaseViewModel
    {
        private User m_user;
        private bool m_update;
        public OUserVM()
        {
            m_user = new User();
            m_update = false;
            SaveUser = new RelayCommand(Save, CanSave);
        }

        public OUserVM(int UID)
        { 
            m_user = ObjectManager.Instance.Users.Find(item=>item.ID == UID);
            m_update = true;
            SaveUser = new RelayCommand(Save, CanSave);
        }

        public string FirstName
        { get { return m_user.FirstName; } set { m_user.FirstName = value; } }

        public string LastName
        { get { return m_user.LastName; } set { m_user.LastName = value; } }

        public string Token
        { get { return m_user.Token; } set { m_user.Token = value; } }

        public bool isActive
        { get { return m_user.isActive; } set { m_user.isActive = value; } }

        public RelayCommand SaveUser { get; set; }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveUser(m_user, true);
            else
                ObjectManager.Instance.saveUser(m_user, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_user.FirstName != "" && m_user.LastName != "" && m_user.Token != "")
                return true;
            else
                return false;
        }

        #endregion

    }
}
