// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// aint with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using System.Text;
using System.Xml;

namespace SeaBotCore.Utils
{
    //https://gist.github.com/frankmeola/9500038
    /// <summary>
    ///     Config object for the XML minifier.
    /// </summary>
    internal class XMLMinifierSettings
    {
        public bool RemoveEmptyLines { get; set; }
        public bool RemoveWhitespaceBetweenElements { get; set; }
        public bool CloseEmptyTags { get; set; }
        public bool RemoveComments { get; set; }

        public static XMLMinifierSettings Aggressive => new XMLMinifierSettings
        {
            RemoveEmptyLines = true,
            RemoveWhitespaceBetweenElements = true,
            CloseEmptyTags = true,
            RemoveComments = true
        };

        public static XMLMinifierSettings NoMinification => new XMLMinifierSettings
        {
            RemoveEmptyLines = false,
            RemoveWhitespaceBetweenElements = false,
            CloseEmptyTags = false,
            RemoveComments = false
        };
    }

    internal class XMLMinifier
    {
        private readonly XMLMinifierSettings _minifierSettings;

        public XMLMinifier(XMLMinifierSettings minifierSettings)
        {
            _minifierSettings = minifierSettings;
        }

        public string Minify(string xml)
        {
            var originalXmlDocument = new XmlDocument();
            originalXmlDocument.PreserveWhitespace =
                !(_minifierSettings.RemoveWhitespaceBetweenElements || _minifierSettings.RemoveEmptyLines);
            originalXmlDocument.Load(new MemoryStream(Encoding.UTF8.GetBytes(xml)));

            //remove comments first so we have less to compress later
            if (_minifierSettings.RemoveComments)
            {
                foreach (XmlNode comment in originalXmlDocument.SelectNodes("//comment()"))
                {
                    comment.ParentNode.RemoveChild(comment);
                }
            }

            if (_minifierSettings.CloseEmptyTags)
            {
                foreach (XmlElement el in originalXmlDocument.SelectNodes(
                    "descendant::*[not(*) and not(normalize-space())]"))
                {
                    el.IsEmpty = true;
                }
            }

            if (_minifierSettings.RemoveWhitespaceBetweenElements)
            {
                return originalXmlDocument.InnerXml;
            }

            var minified = new MemoryStream();
            originalXmlDocument.Save(minified);

            return Encoding.UTF8.GetString(minified.ToArray());
        }
    }
}