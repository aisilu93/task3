using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace task3
{
    [Serializable]
    public class Contact: IComparable, ICloneable
    {
        [XmlAttribute("name")]
        public String m_name        { get; set; }
        [XmlAttribute("workphone")]
        public String m_workphone   { get; set; }
        [XmlAttribute("homephone")]
        public String m_homephone   { get; set; }
        [XmlAttribute("email")]
        public String m_email       { get; set; }
        [XmlAttribute("skype")]
        public String m_skype       { get; set; }
        [XmlAttribute("birthday")]
        public String m_birthday    { get; set; }
        [XmlAttribute("comment")]
        public String m_comment     { get; set; }

        public Contact()
        {
            m_name      = "";
            m_workphone = "";
            m_homephone = "";
            m_email     = "";
            m_skype     = "";
            m_birthday  = "";
            m_comment   = "";
        }

        public object Clone()
        {
            return new Contact
            {
                m_name      = this.m_name,
                m_workphone = this.m_workphone,
                m_homephone = this.m_homephone,
                m_email     = this.m_email,
                m_skype     = this.m_skype,
                m_birthday  = this.m_birthday,
                m_comment   = this.m_comment
            };
        }
        
        public int CompareTo(object obj)
        {
            Contact c = obj as Contact;
            return m_name.CompareTo(c.m_name);
        }
    }
    public class ContactBase:IEnumerable
    {
        public List<Contact> contacts;
        public ContactBase()
        {
            contacts = new List<Contact>();
            Load();
        }
        public void Add(Contact a)
        {
            contacts.Add(a);
            contacts.Sort();
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
        public void Delete(Contact a)
        {
            contacts.Remove(a);
        }
        public void Save()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Contact>));
            using (FileStream fs = new FileStream("base.xml", FileMode.Truncate))
            {
                s.Serialize(fs, contacts);
            }
        }
        public void Load()
        {
            if (!File.Exists("base.xml")) return;
            XmlSerializer s = new XmlSerializer(typeof(List<Contact>));
            using (FileStream fs = new FileStream("base.xml", FileMode.Open))
            {
                contacts = (List<Contact>)s.Deserialize(fs);
            }
        }
        public IEnumerator GetEnumerator()
        {
            return contacts.GetEnumerator();
        }

        ~ContactBase()
        {
            Save();
        }
    }
}
