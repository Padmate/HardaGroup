using HardaGroup.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace HardaGroup.Utility
{
    public static class SystemConfig
    {
        public static SystemConfiguration Init
        {
            get
            {
                return SystemConfiguration.GetInstance();
            }
        }
    }

    /// <summary>
    /// 系统配置文件
    /// 单例模式
    /// </summary>
    public class SystemConfiguration
    {
        // 定义一个静态变量来保存类的实例
        private static SystemConfiguration uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        static XDocument _doc;
        // 定义私有构造函数，使外界不能创建该类实例
        private SystemConfiguration()
        {
            var mapPath = HttpContext.Current.Request.PhysicalApplicationPath;

            string configPath = Path.Combine(mapPath, "System.config");
            _doc = XDocument.Load(configPath);
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static SystemConfiguration GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new SystemConfiguration();
                    }
                }
            }
            return uniqueInstance;
        }


        /// <summary>
        /// 获取邮件服务器配置信息
        /// </summary>
        /// <param name="mapPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public MailSmtp MailSmtp(string type)
        {

            var configuration = _doc.Descendants("configuration").FirstOrDefault();
            if (configuration == null)
                throw new Exception("系统配置文件中找不到configuration节点信息");
            var mailsection = configuration.Descendants("mailsection").FirstOrDefault();
            if (mailsection == null)
                throw new Exception("系统配置文件中找不到mailsection节点信息");
            var smtpclients = mailsection.Descendants("smtpclients");
            if (smtpclients == null)
                throw new Exception("系统配置文件中找不到smtpclients节点信息");
            var lsClients = mailsection.Descendants("smtpclient");

            XElement client = lsClients.SingleOrDefault(c => c.Attribute("type").Value.Equals(type));

            if (client == null)
                return null;


            MailSmtp mailSmtp = new MailSmtp();
            mailSmtp.Server = client.Attribute("server").Value;
            mailSmtp.UserName = client.Attribute("username").Value;
            mailSmtp.Password = client.Attribute("password").Value;
            mailSmtp.DisplayName = client.Attribute("displayname").Value;

            return mailSmtp;
        }

        /// <summary>
        /// 获取邮件配置信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MailConfig MailConfiguration(string type)
        {

            var configuration = _doc.Descendants("configuration").FirstOrDefault();
            if (configuration == null)
                throw new Exception("系统配置文件中找不到configuration节点信息");
            var mailsection = configuration.Descendants("mailsection").FirstOrDefault();
            if (mailsection == null)
                throw new Exception("系统配置文件中找不到mailsection节点信息");
            var mails = mailsection.Descendants("mails");
            if (mails == null)
                throw new Exception("系统配置文件中找不到mails节点信息");
            var lsMails = mails.Descendants("mail");

            XElement mail = lsMails.SingleOrDefault(c => c.Attribute("type").Value.Equals(type));

            if (mail == null)
                return null;


            MailConfig model = new MailConfig();
            model.To = mail.Attribute("to").Value;
            model.Cc = mail.Attribute("cc").Value;

            return model;
        }

        public Hashtable PathConfiguration
        {
            get
            {

                var configuration = _doc.Descendants("configuration").FirstOrDefault();
                if (configuration == null)
                    throw new Exception("系统配置文件中找不到configuration节点信息");
                var pathsection = configuration.Descendants("pathsection").FirstOrDefault();
                if (pathsection == null)
                    throw new Exception("系统配置文件中找不到pathsection节点信息");
                var paths = pathsection.Descendants("path").ToList();

                Hashtable hashPaths = new Hashtable();
                foreach (var path in paths)
                {
                    hashPaths.Add(path.Attribute("id").Value, path.Attribute("value").Value);
                }

                return hashPaths;

            }
        }

    }
}
