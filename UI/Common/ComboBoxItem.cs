using System;

namespace UI.Common
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        
        public ComboBoxItem(string text, int value)
        {
            Text = text;
            Value = value;
        }
        
        public ComboBoxItem()
        {
            Text = string.Empty;
            Value = 0;
        }
        
        public override string ToString()
        {
            return Text;
        }
    }
}
