using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.Services
{
    public class CMS_Services_Message
    {
        /// <summary>
        /// Private constructor to avoid creating instances by other classes
        /// </summary>
        private CMS_Services_Message()
        { 
            //nothing to do, just private
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static CMS_Services_Message _instance;

        /// <summary>
        /// List of stored messages
        /// </summary>
        private static List<String> _messages = new List<string>();

        /// <summary>
        /// List of stored errors
        /// </summary>
        private static List<String> _errors = new List<string>();
        
        /// <summary>
        /// Singleton getInstance method
        /// Returns new Instance if none exists, otherwise the existing one
        /// </summary>
        /// <returns></returns>
        public static CMS_Services_Message getInstance()
        {
            if (_instance == null)
            {
                _instance = new CMS_Services_Message();
            }
            return _instance;
        }

        /// <summary>
        /// Add the given error into the list
        /// </summary>
        /// <param name="error">Error message</param>
        public void addError(string error)
        {
            _errors.Add(error);
        }

        /// <summary>
        /// Add the given message into the list
        /// </summary>
        /// <param name="message">Message</param>
        public void addMessage(string message)
        {
            _messages.Add(message);
        }

        /// <summary>
        /// Is there any error?
        /// </summary>
        /// <returns>Error existence</returns>
        public bool hasErrors()
        {
            return (_errors.Count > 0);
        }

        /// <summary>
        /// Is there any message?
        /// </summary>
        /// <returns>Message existence</returns>
        public bool hasMessages()
        {
            return (_messages.Count > 0);
        }

        /// <summary>
        /// Get the stored list of messages
        /// </summary>
        /// <returns>List of messages</returns>
        public List<string> getMessages()
        {
            return _messages;
        }

        /// <summary>
        /// Get the stored list of errors
        /// </summary>
        /// <returns>List of errors</returns>
        public List<string> getErrors()
        {
            return _errors;
        }

        /// <summary>
        /// Removes all stored errors and messages
        /// </summary>
        public void truncate()
        {
            _messages = new List<string>();
            _errors = new List<string>();
        }

    }
}
