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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBotCore.Localizaion
{
    public static class LocalizationController
    {
        public enum ELanguages
        {
            EN,
            RU
        }

        public static ELanguages CurrentLang
        {
            get { return Core.Config.language; }

            set
            {
                Core.Config.language = value;
                SetLanguage(value);
            }
        }

        public static ELanguages GetDefaultLang()
        {
            foreach (var lang in Enum.GetNames(typeof(ELanguages)))
            {
                CultureInfo ci = CultureInfo.InstalledUICulture;
                if (string.Compare(ci.TwoLetterISOLanguageName, lang, true) == 0)
                {
                    return (ELanguages) Enum.Parse(typeof(ELanguages), lang);
                }
            }

            return ELanguages.EN;
        }

        public static void SetLanguage(ELanguages elang)
        {
            CultureInfo inf = CultureInfo.DefaultThreadCurrentUICulture;
            switch (elang)
            {
                case ELanguages.EN:
                {
                    inf = new CultureInfo("en");
                }
                    break;
                case ELanguages.RU:
                {
                    inf = new CultureInfo("ru-RU");
                }
                    break;
            }

            Thread.CurrentThread.CurrentUICulture = inf;
       
        }
    }
}