using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Collections;

namespace Helpers
{
    public static class TextBoxHelper
    {
        public static string LabeledTextBox(this HtmlHelper html, string name, string label, object value,object htmlAttributes)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextBox(name, value,htmlAttributes));
        }

        public static string LabeledTextBox(this HtmlHelper html, string name, string label,object value)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextBox(name, value));
        }

        public static string LabeledTextBox(this HtmlHelper html, string name, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name,label, html.TextBox(name));
        }

        public static string LabeledTextArea(this HtmlHelper html, string name, string label, int rows, int columns, object htmlAttributes)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextArea(name, null, rows, columns, htmlAttributes));
        }

        public static string LabeledTextArea(this HtmlHelper html, string name, string label, object value, int rows, int columns, object htmlAttributes)
        {
            string text = value==null?string.Empty:value.ToString();

            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextArea(name, text, rows, columns, htmlAttributes));
        }

        public static string LabeledTextArea(this HtmlHelper html, string name, string label, object value, object htmlAttributes)
        {
            string text = value == null ? string.Empty : value.ToString();
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextArea(name, text, htmlAttributes));
        }

        public static string LabeledTextArea(this HtmlHelper html, string name, string label, object value)
        {
            string text = value == null ? string.Empty : value.ToString();
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextArea(name, text));
        }

        public static string LabeledTextArea(this HtmlHelper html, string name, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.TextArea(name));
        }

        public static string LabeledDropDownList(this HtmlHelper html, string name, string optionLabel, string[] values, string label)
        {
            SelectList source = new SelectList(values);
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.DropDownList(name, source, optionLabel));
        }

        public static string LabeledDropDownList(this HtmlHelper html, string name, string optionLabel, SelectList selectList, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.DropDownList(name, selectList, optionLabel));
        }
        
        public static string LabeledDropDownList(this HtmlHelper html, string name, SelectList selectList, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.DropDownList(name, selectList));
        }

        public static string LabeledListBox(this HtmlHelper html, string name, string[] values, string label)
        {
            SelectList source = new SelectList(values);
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.ListBox(name, source));
        }

        public static string LabeledListBox(this HtmlHelper html, string name, string[] values, string label, object htmlAttributes)
        {
            SelectList source = new SelectList(values);
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.ListBox(name, source, htmlAttributes));
        }

        public static string LabeledListBox(this HtmlHelper html, string name, SelectList selectList, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.ListBox(name, selectList));
        }

        public static string LabeledListBox(this HtmlHelper html, string name, SelectList selectList, string label, object htmlAttributes)
        {
            return string.Format("<label for=\"{0}\">{1}</label>{2}", name, label, html.ListBox(name, selectList, htmlAttributes));
        }

        public static string IsActiveText(this HtmlHelper html, bool isActive)
        {
            if (isActive)
                return "Tak";
            else
                return "Nie";
        }

        public static string DatePicker(this HtmlHelper htmlHelper, string name)
        {
            return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"\"/>", name);
        }
        
        public static string LabeledDatePicker(this HtmlHelper htmlHelper, string name, string label)
        {
            return string.Format("<label for=\"{0}\">{1}</label><input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"\"/>", name, label);
        }

        public static string LabeledDatePicker(this HtmlHelper htmlHelper, string name, string label, DateTime value)
        {
            return string.Format("<label for=\"{0}\">{1}</label><input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"{2}\"/>", name, label, value.ToShortDateString());
        }

        public static string LabeledPropertyValue(this HtmlHelper htmlHelper, string label, string value)
        {
            return string.Format("<label>{0}: </label><span>{1}</span>",label, htmlHelper.Encode(value));
        }

        public static string SenderPicker(this HtmlHelper htmlHelper, IEnumerable senders, string senderLabel, bool showNewLink)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div id=\"senderSelect\">");
            sb.Append(LabeledDropDownList(htmlHelper, "SenderID", Resources.Strings.S120, new SelectList(senders, "SenderID", "FullName"), senderLabel));
            if (showNewLink)
            {
                sb.Append(string.Concat("<a class=\"showDialog\" href=\"#\" onclick=\"jQuery('#senderDialog').dialog('open'); return false\" />Nowy ", senderLabel, "</a>"));
            }
            sb.Append("<div><input type=\"text\" id=\"senderSearch\" /></div>");
            
            //if(showNewLink){    
            //    sb.Append(string.Concat("<div id=\"senderDialog\" title=\"Nowy ",senderLabel,"\">"));
            //    htmlHelper.RenderPartial("SenderUserControl");
            //    sb.Append("</div>");
            //}
            sb.Append("</div>");

            return sb.ToString();
        }
        public static string EncodeLong(this HtmlHelper htmlHelper, string text, int maxLength)
        {
            if (text!=null && text.Length > maxLength)
            {
                text = string.Concat(text.Substring(0, maxLength-3), "...");
            }
            return htmlHelper.Encode(text);
        }

        public static string UserNameTextBox(this HtmlHelper htmlHelper,string name,string prefix,string userName){

            //string code = string.Format("<div class='userNameTextBox'><input type='hidden' id='{0}' name='{0}' value='{1}{2}'/><input type='text' id='{0}prefix' class='userNameTextBoxPrefix' disabled='disabled' value='{1}' style='width:{3}em;' /><input type='text' class='userNameTextBoxName' value='{2}' onchange='$(\"#{0}\").val($(\"#{0}prefix\").val()+$(this).val());' /></div>", name, prefix, userName,prefix.Length-1);

            string code = string.Format("<div class='userNameTextBox'><input type='hidden' id='{0}' name='{0}' value='{1}{2}'/><span id='{0}prefix' class='userNameTextBoxPrefix'>{1}</span><input type='text' class='userNameTextBoxName' value='{2}' onchange='$(\"#{0}\").val($(\"#{0}prefix\").text()+$(this).val());' /></div>", name, prefix, userName);

            return code;
        }

        public static string SearchOcrCheckBox(this HtmlHelper htmlHelper, string name, string label, bool allowOCR)
        {
            if (allowOCR)
            {
                return string.Format("{0}<label for=\"{1}\">{2}</label>",htmlHelper.CheckBox(name, false),name, label);
            }
            else
            {
                return string.Format("{0}<label for=\"{1}\">{2}</label>", htmlHelper.CheckBox(name, false, new { Disabled="disabled" }), name, label);
            }
        }
    }
}
