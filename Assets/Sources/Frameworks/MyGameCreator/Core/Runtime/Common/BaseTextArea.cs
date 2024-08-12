using System;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Core.Runtime.Common
{
    [Serializable]
    public class BaseTextArea
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private string m_Text;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public string Text => this.m_Text;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        protected BaseTextArea()
        {
            this.m_Text = string.Empty;
        }

        protected BaseTextArea(string text) : this()
        {
            this.m_Text = text;
        }
    }
}