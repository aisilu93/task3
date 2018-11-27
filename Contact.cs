using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace task3
{
    [Serializable]
    public class Contact
    {
        public String m_name;
        public String m_workphone;
        public String m_homephone;
        public String m_email;
        public String m_skype;
        public String m_birthday;
        public String m_comment;
    }
    public class ContactBase
    {
        List<Contact> contacts;
        public ContactBase()
        {
            Load();
        }
        public void Add(ref Contact a)
        {
            contacts.Add(a);
        }
        public List<Contact> Search(String req)
        {
            req = req.ToLower();
            var res = from c in contacts
                      where c.m_name.Contains(req)  ||
                      c.m_workphone.Contains(req)   ||
                      c.m_homephone.Contains(req)   ||
                      c.m_email.Contains(req)       ||
                      c.m_skype.Contains(req)       ||
                      c.m_birthday.Contains(req)    ||
                      c.m_comment.Contains(req)
                      select c;
            return (List <Contact>)res;
        }
        public void Delete(ref Contact a)
        {
            contacts.Remove(a);
        }
        public void Save()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Contact>));
            using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
            {
                s.Serialize(fs, contacts);
            }
        }
        public void Load()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Contact>));
            using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
            {
                contacts = (List<Contact>)s.Deserialize(fs);
            }
        }
        ~ContactBase()
        {
            Save();
        }
    }
}
