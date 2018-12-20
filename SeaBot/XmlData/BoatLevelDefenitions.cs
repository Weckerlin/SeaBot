// SeaBotCore
// Copyright (C) 2018 Weespin
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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SeaBot.XmlData
{
    public class BoatLevelDefenitions
    {
        // Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
        /// <remarks/>
        [Serializable]
        [DesignerCategory("code")]
        [XmlRoot("levels"), XmlType("levels")]
        public class levels
        {
            private levelsLevel[] levelField;

            /// <remarks/>
            [XmlElement("level")]
            public levelsLevel[] level
            {
                get { return levelField; }
                set { levelField = value; }
            }
        }

        /// <remarks/>
        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        public class levelsLevel
        {
            private byte idField;

            private byte massField;

            private ushort turn_countField;

            private byte turn_timeField;

            private byte output_idField;

            private byte output_amountField;

            private string model_nameField;

            private byte id1Field;

            /// <remarks/>
            public byte id
            {
                get { return idField; }
                set { idField = value; }
            }

            /// <remarks/>
            public byte mass
            {
                get { return massField; }
                set { massField = value; }
            }

            /// <remarks/>
            public ushort turn_count
            {
                get { return turn_countField; }
                set { turn_countField = value; }
            }

            /// <remarks/>
            public byte turn_time
            {
                get { return turn_timeField; }
                set { turn_timeField = value; }
            }

            /// <remarks/>
            public byte output_id
            {
                get { return output_idField; }
                set { output_idField = value; }
            }

            /// <remarks/>
            public byte output_amount
            {
                get { return output_amountField; }
                set { output_amountField = value; }
            }

            /// <remarks/>
            public string model_name
            {
                get { return model_nameField; }
                set { model_nameField = value; }
            }

            /// <remarks/>
            [XmlAttribute("id")]
            public byte id1
            {
                get { return id1Field; }
                set { id1Field = value; }
            }
        }
    }
}