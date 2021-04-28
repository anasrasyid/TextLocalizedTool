using System;

namespace personaltools.textlocalizedtool
{
    [Serializable]
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }

        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }

    public enum Languages 
    {
        [StringValue("sq")]
        Albanian,

        [StringValue("am")]
        Amharic,

        [StringValue("ar")]
        Arabic,

        [StringValue("hy")]
        Armenian,

        [StringValue("az")]
        Azerbaijani,

        [StringValue("eu")]
        Basque,

        [StringValue("be")]
        Belarusian,

        [StringValue("bn")]
        Bengali,

        [StringValue("bs")]
        Bosnian,

        [StringValue("bg")]
        Bulgarian,

        [StringValue("ca")]
        Catalan,

        [StringValue("ceb")]
        Cebuano,

        [StringValue("zh")]
        Chinese_Simplified,

        [StringValue("zh-TW")]
        Chinese_Traditional,

        [StringValue("co")]
        Corsican,

        [StringValue("hr")]
        Croatian,

        [StringValue("cs")]
        Czech,

        [StringValue("da")]
        Danish,

        [StringValue("nl")]
        Dutch,

        [StringValue("en")]
        English,

        [StringValue("eo")]
        Esperanto,

        [StringValue("et")]
        Estonian,

        [StringValue("fi")]
        Finnish,

        [StringValue("fr")]
        French,

        [StringValue("fy")]
        Frisian,

        [StringValue("gl")]
        Galician,

        [StringValue("ka")]
        Georgian,

        [StringValue("de")]
        German,

        [StringValue("el")]
        Greek,

        [StringValue("gu")]
        Gujarati,

        [StringValue("ht")]
        Haitian_Creole,

        [StringValue("ha")]
        Hausa,

        [StringValue("haw")]
        Hawaiian,

        [StringValue("he")]
        Hebrew,

        [StringValue("hi")]
        Hindi,

        [StringValue("hmn")]
        Hmong,

        [StringValue("hu")]
        Hungarian,

        [StringValue("is")]
        Icelandic,

        [StringValue("ig")]
        Igbo,

        [StringValue("id")]
        Indonesian,

        [StringValue("ga")]
        Irish,

        [StringValue("it")]
        Italian,

        [StringValue("ja")]
        Japanese,

        [StringValue("jv")]
        Javanese,

        [StringValue("kn")]
        Kannada,

        [StringValue("kk")]
        Kazakh,

        [StringValue("km")]
        Khmer,

        [StringValue("rw")]
        Kinyarwanda,

        [StringValue("ko")]
        Korean,

        [StringValue("ku")]
        Kurdish,

        [StringValue("ky")]
        Kyrgyz,

        [StringValue("lo")]
        Lao,

        [StringValue("la")]
        Latin,

        [StringValue("lv")]
        Latvian,

        [StringValue("lt")]
        Lithuanian,

        [StringValue("lb")]
        Luxembourgish,

        [StringValue("mk")]
        Macedonian,

        [StringValue("mg")]
        Malagasy,

        [StringValue("ms")]
        Malay,

        [StringValue("ml")]
        Malayalam,

        [StringValue("mt")]
        Maltese,

        [StringValue("mi")]
        Maori,

        [StringValue("mr")]
        Marathi,

        [StringValue("mn")]
        Mongolian,

        [StringValue("my")]
        Myanmar_Burmese,

        [StringValue("ne")]
        Nepali,

        [StringValue("no")]
        Norwegian,

        [StringValue("ny")]
        Nyanja_Chichewa,

        [StringValue("or")]
        Odia_Oriya,

        [StringValue("ps")]
        Pashto,

        [StringValue("fa")]
        Persian,

        [StringValue("pl")]
        Polish,

        [StringValue("pt")]
        Portuguese_Portugal,

        [StringValue("pt")]
        Brazil,

        [StringValue("pa")]
        Punjabi,

        [StringValue("ro")]
        Romanian,

        [StringValue("ru")]
        Russian,

        [StringValue("sm")]
        Samoan,

        [StringValue("gd")]
        Scots_Gaelic,

        [StringValue("sr")]
        Serbian,

        [StringValue("st")]
        Sesotho,

        [StringValue("sn")]
        Shona,

        [StringValue("sd")]
        Sindhi,

        [StringValue("si")]
        Sinhala_Sinhalese,

        [StringValue("sk")]
        Slovak,

        [StringValue("sl")]
        Slovenian,

        [StringValue("so")]
        Somali,

        [StringValue("es")]
        Spanish,

        [StringValue("su")]
        Sundanese,

        [StringValue("sw")]
        Swahili,

        [StringValue("sv")]
        Swedish,

        [StringValue("tl")]
        Tagalog_Filipino,

        [StringValue("tg")]
        Tajik,

        [StringValue("ta")]
        Tamil,

        [StringValue("tt")]
        Tatar,

        [StringValue("te")]
        Telugu,

        [StringValue("th")]
        Thai,

        [StringValue("tr")]
        Turkish,

        [StringValue("tk")]
        Turkmen,

        [StringValue("uk")]
        Ukrainian,

        [StringValue("ur")]
        Urdu,

        [StringValue("ug")]
        Uyghur,

        [StringValue("uz")]
        Uzbek,

        [StringValue("vi")]
        Vietnamese,

        [StringValue("cy")]
        Welsh,

        [StringValue("xh")]
        Xhosa,

        [StringValue("yi")]
        Yiddish,

        [StringValue("yo")]
        Yoruba,

        [StringValue("zu")]
        Zulu,

        [StringValue("cst")]
        Custom_Language
    };
}
