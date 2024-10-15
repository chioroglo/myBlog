export const getFlagEmoji = (iso639_2: string | null | undefined): string | null => {
    if (!iso639_2 || !isLanguage(iso639_2)) {
        return null;
    }
    
    const countryCode = languageCountryMap[iso639_2]; 
    const emoji = String.fromCodePoint(
        ...[...countryCode].map(char => 0x1F1E6 + char.charCodeAt(0) - 'A'.charCodeAt(0))
    );
    
    return emoji;
}

const isLanguage = (isoCode: string) => !!languageCountryMap[isoCode];
// ISO 639-2 to ISO 3166-1
const languageCountryMap: { [key: string]: string } = {
    'aar': 'ER',  // Afar
    'abk': 'GE',  // Abkhazian
    'ace': 'ID',  // Acehnese
    'ach': 'UG',  // Acholi
    'ada': 'GH',  // Adangme
    'akz': 'GH',  // Akan
    'als': 'CH',  // Alemannic
    'amh': 'ET',  // Amharic
    'ara': 'AE',  // Arabic
    'arg': 'ES',  // Aragonese
    'asm': 'IN',  // Assamese
    'ava': 'RU',  // Avaric
    'ave': 'RU',  // Avar
    'aym': 'BO',  // Aymara
    'aze': 'AZ',  // Azerbaijani
    'bak': 'RU',  // Bashkir
    'bel': 'BY',  // Belarusian
    'ben': 'IN',  // Bengali
    'bih': 'IN',  // Bihari
    'bis': 'VU',  // Bislama
    'bos': 'BA',  // Bosnian
    'bre': 'FR',  // Breton
    'bul': 'BG',  // Bulgarian
    'cat': 'ES',  // Catalan
    'cha': 'GU',  // Chamorro
    'che': 'RU',  // Chechen
    'chi': 'CN',  // Chinese
    'chr': 'US',  // Cherokee
    'chu': 'RU',  // Church Slavic
    'chv': 'RU',  // Chuvash
    'cor': 'GB',  // Cornish
    'cos': 'IT',  // Corsican
    'cre': 'CA',  // Cree
    'ces': 'CZ',  // Czech
    'dan': 'DK',  // Danish
    'deu': 'DE',  // German
    'div': 'MV',  // Divehi
    'dzo': 'BT',  // Dzongkha
    'ell': 'GR',  // Greek
    'eng': 'GB',  // English
    'epo': 'ZZ',  // Esperanto
    'est': 'EE',  // Estonian
    'ewe': 'TG',  // Ewe
    'fa': 'IR',   // Persian
    'fin': 'FI',  // Finnish
    'fij': 'FJ',  // Fijian
    'fre': 'FR',  // French
    'fra': 'FR',
    'ful': 'NG',  // Fulah
    'geo': 'GE',  // Georgian
    'gla': 'GB',  // Scottish Gaelic
    'gle': 'IE',  // Irish
    'glg': 'ES',  // Galician
    'grn': 'PY',  // Guarani
    'guj': 'IN',  // Gujarati
    'hat': 'HT',  // Haitian
    'hau': 'NG',  // Hausa
    'heb': 'IL',  // Hebrew
    'her': 'NA',  // Herero
    'hin': 'IN',  // Hindi
    'hmo': 'PG',  // Hiri Motu
    'hrv': 'HR',  // Croatian
    'hun': 'HU',  // Hungarian
    'hye': 'AM',  // Armenian
    'ibo': 'NG',  // Igbo
    'ice': 'IS',  // Icelandic
    'ido': 'ZZ',  // Ido
    'iii': 'CN',  // Sichuan Yi
    'iku': 'CA',  // Inuktitut
    'ile': 'ZZ',  // Interlingue
    'ilo': 'PH',  // Iloko
    'ind': 'ID',  // Indonesian
    'isl': 'IS',  // Icelandic
    'ita': 'IT',  // Italian
    'jav': 'ID',  // Javanese
    'jpn': 'JP',  // Japanese
    'jrd': 'IL',  // Judeo-Arabic
    'kan': 'IN',  // Kannada
    'kas': 'IN',  // Kashmiri
    'kat': 'GE',  // Georgian
    'kaw': 'ID',  // Kawi
    'kaz': 'KZ',  // Kazakh
    'khm': 'KH',  // Khmer
    'kik': 'KE',  // Kikuyu
    'kin': 'RW',  // Kinyarwanda
    'kir': 'KG',  // Kirghiz
    'kom': 'RU',  // Komi
    'kon': 'CD',  // Kongo
    'kor': 'KR',  // Korean
    'kua': 'AO',  // Kuanyama
    'kur': 'TR',  // Kurdish
    'lao': 'LA',  // Lao
    'lat': 'ZZ',  // Latin
    'lav': 'LV',  // Latvian
    'lim': 'BE',  // Limburgish
    'lin': 'CD',  // Lingala
    'lit': 'LT',  // Lithuanian
    'lub': 'CD',  // Luba-Katanga
    'lug': 'UG',  // Ganda
    'mac': 'MK',  // Macedonian
    'mah': 'MH',  // Marshallese
    'mal': 'IN',  // Malayalam
    'mao': 'NZ',  // Maori
    'mar': 'IN',  // Marathi
    'may': 'MY',  // Malay
    'mlg': 'MG',  // Malagasy
    'mlt': 'MT',  // Maltese
    'mon': 'MN',  // Mongolian
    'nau': 'NR',  // Nauruan
    'nav': 'US',  // Navajo
    'nbl': 'ZA',  // Ndebele
    'nde': 'ZW',  // North Ndebele
    'ndo': 'CD',  // Ndonga
    'nep': 'NP',  // Nepali
    'nno': 'NO',  // Norwegian Nynorsk
    'nor': 'NO',  // Norwegian
    'nrf': 'FR',  // Northern Frisian
    'oci': 'FR',  // Occitan
    'oji': 'CA',  // Ojibwe
    'ori': 'IN',  // Odia
    'orm': 'ET',  // Oromo
    'oss': 'RU',  // Ossetian
    'pan': 'IN',  // Punjabi
    'pap': 'CW',  // Papiamento
    'pas': 'AF',  // Pashto
    'per': 'IR',  // Persian
    'pln': 'PL',  // Polish
    'por': 'BR',  // Portuguese
    'que': 'PE',  // Quechua
    'roh': 'CH',  // Romansh
    'ron': 'RO',  // Romanian
    'run': 'BI',  // Rundi
    'rus': 'RU',  // Russian
    'sag': 'CF',  // Sango
    'sam': 'IL',  // Samaritan Aramaic
    'san': 'IN',  // Sanskrit
    'scc': 'RS',  // Serbian
    'sna': 'ZW',  // Shona
    'snd': 'PK',  // Sindhi
    'som': 'SO',  // Somali
    'sot': 'ZA',  // Sotho
    'spa': 'ES',  // Spanish
    'sqi': 'AL',  // Albanian
    'srd': 'IT',  // Sardinian
    'srp': 'RS',  // Serbian
    'ssw': 'ZA',  // Swati
    'swe': 'SE',  // Swedish
    'tah': 'PF',  // Tahitian
    'tam': 'IN',  // Tamil
    'tat': 'RU',  // Tatar
    'tel': 'IN',  // Telugu
    'tgk': 'TJ',  // Tajik
    'tgl': 'PH',  // Tagalog
    'tha': 'TH',  // Thai
    'tir': 'ER',  // Tigrinya
    'ton': 'TO',  // Tongan
    'tsn': 'ZA',  // Tswana
    'tso': 'ZA',  // Tsonga
    'tuk': 'TM',  // Turkmen
    'tur': 'TR',  // Turkish
    'twi': 'GH',  // Twi
    'uig': 'CN',  // Uyghur
    'ukr': 'UA',  // Ukrainian
    'urd': 'PK',  // Urdu
    'uzb': 'UZ',  // Uzbek
    'vie': 'VN',  // Vietnamese
    'vol': 'ZZ',  // Volapük
    'wln': 'BE',  // Walloon
    'wol': 'SN',  // Wolof
    'xho': 'ZA',  // Xhosa
    'yid': 'IL',  // Yiddish
    'yor': 'NG',  // Yoruba
    'zha': 'CN',  // Zhuang
    'zho': 'CN',  // Chinese
    'zul': 'ZA',  // Zulu
    'zun': 'US',  // Zuni
};